using System;
using System.Collections.Generic;
using System.Linq;

delegate void AttackEventHandler(AttackEventArgs aea);

abstract class Card
{
    public Hero owner, opponent;
    public int mana;
    private static List<Card> cards;

    public Card()
    {
    }
    public Card (int mana)
    {
        this.mana = mana;
    }
    public Card (int mana, Hero owner, Hero opponent) : this(mana)
    {
        this.owner = owner;
        this.opponent = opponent;
    }

    public virtual void OnPlay()
    {
        owner.hand.Remove(this);
    }

    public virtual Card Clone()
    {
        Card card = (Card)this.GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        card.mana = this.mana;
        return card;
    }
    public override bool Equals(object obj)
    {
        if (this.GetType() != obj.GetType())
            return false;
        Card toCheck = (Card)obj;
        return (this.mana == toCheck.mana);
    }

    static public Card GetCard(string cardName)
    {
        if (cards == null)
        {
            createCards();
        }
        foreach (Card c in cards)
        {
            if (c.GetType().ToString() == cardName)
                return (Card) c.GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        }
        return null;
    }
    static private void createCards()
    {
        IEnumerable<System.Type> o = typeof(Card).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Card)) && !type.IsAbstract && type != typeof(UnknownCard));
        Card.cards = new List<Card>();
        foreach (System.Type c in o)
        {
            Card caccent = (Card)c.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
            Card.cards.Add(caccent);
        }
    }
    static public List<Card> createCards(Func<Type,bool> condition)
    {
        IEnumerable<System.Type> o = typeof(Card).Assembly.GetTypes().Where(condition);
        List<Card> result = new List<Card>();
        foreach (System.Type c in o)
        {
            Card c2 = (Card)c.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
            result.Add(c2);
        }
        return result;
    }
}

abstract class Minion : Card, IDamagable
{
    private int attackValue, healthValue, spelldmg;
    public int maxHealth;
    public int attack { get { return attackValue; } set { attackValue = value; } }
    public int health { get { return healthValue; } set { healthValue = Math.Min(value, maxHealth); } }
    public bool isFrozen { get { return frozen; } set { frozen = value; } }
    public int spellDamage { get { return this.spelldmg; } set { this.owner.spellDamage += (value - spelldmg); this.spelldmg = value; } }

    public List<Attribute> attributes;
    public bool hasTaunt { get { return attributes.Contains(Attribute.Taunt); } }

    public delegate void EmptyEventHandler();
    public event EmptyEventHandler Deathrattle;
    public event EmptyEventHandler Battlecry;

    public event AttackEventHandler Attack;
    public event AttackEventHandler Attacked;

    public List<Action> toSilence;

    public bool frozen, cantAttack;

    public Minion (int mana, int attack, int health) : base(mana)
    {
        this.attack = attack;
        this.health = health;
        this.maxHealth = health;
        this.cantAttack = false;
        this.toSilence = new List<Action>();
    }
    public Minion(int mana, int attack, int health, Attribute attribute) : this(mana, attack, health)
    {
        attributes = new List<Attribute>();
        attributes.Add(attribute);
    }
    public Minion(int mana, int attack, int health, Attribute[] attributes) : this(mana, attack, health)
    {
        this.attributes = new List<Attribute>(attributes);
    }
    public override Card Clone()
    {
        Minion minion = (Minion)base.Clone();
        minion.attack = this.attack;
        minion.health = this.health;
        minion.maxHealth = this.maxHealth;
        minion.cantAttack = this.cantAttack;
        minion.toSilence = new List<Action>();
        foreach (Action action in this.toSilence)
            minion.toSilence.Add(action);
        minion.Battlecry = this.Battlecry;
        minion.Deathrattle = this.Deathrattle;

        return minion;
    }
    public override bool Equals(object obj)
    {
        if (this.GetType() != obj.GetType())
            return false;
        Minion compareTo = (Minion)obj;
        if (!Effects.checkList(this.toSilence, compareTo.toSilence) || !Effects.checkList(this.attributes, compareTo.attributes))
            return false;
        if (this.Battlecry != compareTo.Battlecry || this.Deathrattle != compareTo.Deathrattle)
            return false;
        return (this.attack == compareTo.attack && this.health == compareTo.health && this.maxHealth == compareTo.maxHealth && this.mana == compareTo.mana
             && this.isFrozen == compareTo.isFrozen && this.cantAttack == compareTo.cantAttack);
    }
    public bool HasBattlecry()
    {
        return Battlecry != null;
    }

    public void Damage(int damage)
    {
        this.health -= damage;
    }
    public void Heal(int healFor)
    {
        this.health += healFor;
    }
    public void Freeze()
    {
        this.isFrozen = true;
    }

    public virtual void Destroy()
    {
        if (Deathrattle != null)
            Deathrattle();

        foreach (Action toDo in toSilence)
            toDo();

        owner.onBoard.Remove(this);
        // BIG todo // Is it though?
    }
    public virtual void OnSummon()
    {
        owner.InvokeSummonEvent(this);
        this.toSilence.Add(() => Deathrattle = null);
    }
    public override void OnPlay()
    {
        base.OnPlay();
        owner.onBoard.Add(this);
        this.OnSummon();
    }
}

abstract class Weapon : Card
{
    protected int attack, durability;
    public delegate void EmptyEventHandler();
    public event EmptyEventHandler Deathrattle;

    public Weapon(int mana, int attack, int durability) : base(mana)
    {
        this.attack = attack;
        this.durability = durability;
    }

    public override Card Clone()
    {
        Weapon weapon = (Weapon) base.Clone();
        weapon.attack = this.attack;
        weapon.durability = this.durability;
        weapon.Deathrattle = Deathrattle;
        return weapon;
    }

    public void Destroy()
    {
        if (Deathrattle != null)
            Deathrattle();

        owner.weapon = null;
    }
}

abstract class Spell : Card
{
    public Spell(int mana) : base(mana)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        owner.InvokeSpellEvent(this);
    }
}

abstract class Secret : Spell
{
    public Secret(int mana) : base(mana)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        owner.secrets.Add(this);
    }

    public void Triggered(Object objecthingy)
    {
        if (Resolve(objecthingy))
            RemoveFromPlay();
    }

    public virtual void RemoveFromPlay()
    {
        owner.secrets.Remove(this);
    }

    public abstract bool Resolve(Object thingyobject);
}

class UnknownCard : Card
{
    List<Card> possibilities;

    public UnknownCard(List<Card> possibilities, Hero hero)
    {
        this.possibilities = possibilities;
        for (int i = 0; i < possibilities.Count; i++)
            possibilities[i].owner = hero;
    }
    public UnknownCard(Func<Type, bool> condition, Hero hero)
    {
        this.possibilities = Card.createCards(condition);
        for (int i = 0; i < possibilities.Count; i++)
            possibilities[i].owner = hero;
    }
}