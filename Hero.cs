using System;
using System.Collections.Generic;
using System.Linq;

abstract class Hero : IDamagable
{
    protected int healthValue, armor, mana, attackValue, maxHealth;
    protected bool frozen;
    public int health { get {return healthValue;} set {healthValue = value;}}
    public int attack { get { return attackValue; } set { attackValue = value; } }
    public bool isFrozen { get { return frozen; } set { frozen = value; } }
    public int availableMana, spellDamage;
    public bool turn;
    public string name;

    public delegate void SpellEventHandler(Spell spell);
    public event SpellEventHandler SpellCast;

    public delegate void CardEventHandler(Card card);
    public event CardEventHandler CardDrawn;

    public delegate void MinionEventHandler(Minion minion);
    public event MinionEventHandler PlayMinion;
    public event MinionEventHandler SummonMinion;

    public delegate void EmptyEventHandler();
    public event EmptyEventHandler StartTurn;
    public event EmptyEventHandler EndTurn;

    public event AttackEventHandler Attack;
    public event AttackEventHandler Attacked;

    public List<Card> deck;
    public List<Card> hand;
    public List<Minion> onBoard;
    public List<Secret> secrets;
    public Weapon weapon;

    protected Hero opp;
    public Hero opponent { set { opp = value; updateOpp(); } }

    static private List<Hero> heroes;

    public Hero()
    {
        this.health = 30;
        this.maxHealth = 30;
        this.armor = 0;
        this.mana = 0;
        this.spellDamage = 0;
        this.deck = new List<Card>();
        this.hand = new List<Card>();
        this.onBoard = new List<Minion>();
        this.secrets = new List<Secret>();
        this.frozen = false;
    }
    public Hero Clone()
    {
        Hero copyTo = (Hero)this.GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        copyTo.health = this.health;
        copyTo.maxHealth = this.maxHealth;
        copyTo.armor = this.armor;
        copyTo.mana = this.mana;
        copyTo.spellDamage = this.spellDamage;
        copyTo.deck = new List<Card>();
        foreach (Card c in deck)
        {
            Card copy = c.Clone();
            copy.owner = copyTo;
            copyTo.deck.Add(copy);
        }
        copyTo.hand = new List<Card>();
        foreach (Card c in hand)
        {
            Card copy = c.Clone();
            copy.owner = copyTo;
            copyTo.hand.Add(copy);
        }
        copyTo.onBoard = new List<Minion>();
        foreach (Minion m in onBoard)
        {
            Minion copy = (Minion)m.Clone();
            copy.owner = copyTo;
            copyTo.onBoard.Add(copy);
        }
        copyTo.secrets = new List<Secret>();
        foreach (Secret s in secrets)
        {
            Secret copy = (Secret)s.Clone();
            copy.owner = copyTo;
            copyTo.secrets.Add(copy);
        }
        copyTo.frozen = frozen;
        copyTo.SpellCast = this.SpellCast;
        copyTo.CardDrawn = this.CardDrawn;
        copyTo.PlayMinion = this.PlayMinion;
        copyTo.StartTurn = this.StartTurn;
        copyTo.EndTurn = this.EndTurn;
        return copyTo;
    }
    public override bool Equals(object obj)
    {
        if (obj.GetType() != this.GetType())
            return false;
        Hero toCheck = (Hero)obj;
        if (!Effects.checkList(this.deck, toCheck.deck) || !Effects.checkList(this.hand, toCheck.hand) || !Effects.checkList(this.onBoard, toCheck.onBoard) || !Effects.checkList(this.secrets, toCheck.secrets))
            return false;
        if (this.SpellCast != toCheck.SpellCast || this.PlayMinion != toCheck.PlayMinion || this.CardDrawn != toCheck.CardDrawn || this.StartTurn != toCheck.StartTurn || this.EndTurn != toCheck.EndTurn)
            return false;

        return (this.health == toCheck.health && this.armor == toCheck.armor && this.mana == toCheck.mana && this.maxHealth == toCheck.maxHealth
             && this.attack == toCheck.attack && this.isFrozen == toCheck.isFrozen && this.availableMana == toCheck.availableMana
             && this.spellDamage == toCheck.spellDamage && this.turn == toCheck.turn && this.name == toCheck.name);
    }
    protected void updateOpp()
    {
        foreach (Card c in this.deck)
            c.opponent = opp;
        foreach (Card c in this.hand)
            c.opponent = opp;
        foreach (Minion m in this.onBoard)
            m.opponent = opp;
        foreach (Secret s in this.secrets)
            s.opponent = opp;
    }

    public Minion getFriendlyMinionTarget(bool random = false)
    {
        return Effects.GetTarget(onBoard, random);
    }
    public Minion getMinionTarget(bool random = false)
    {
        return Effects.GetTarget(onBoard, opp.onBoard, random);
    }
    public IDamagable getTarget(bool random = false)
    {
        List<IDamagable> list = new List<IDamagable>();
        list.Add(this);
        list.Add(opp);
        list.AddRange(onBoard);
        list.AddRange(opp.onBoard);
        return Effects.GetTarget(list, random);
    }

    public void OnStartOfTurn()
    {
        if (mana < 10)
            mana++;
        availableMana = mana;
    }

    public override string ToString()
    {
        return name;
    }

    public void Damage(int damage)
    {
        health -= damage;
        //hero damaged event
    }
    void IDamagable.Heal(int healFor)
    {
        health += healFor;
        if (health > maxHealth)
            health = maxHealth;
    }
    void IDamagable.Freeze()
    {
        this.frozen = true;
    }

    abstract public void HeroPower();

    public void ExtraAvailableMana(int amount)
    {
        this.availableMana += amount;
    }
    public virtual void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!drawCard())
                i--;
        }
    }
    private bool drawCard()
    {
        Card result = null;
        Console.WriteLine("What card did you draw?");
        Card card = Card.GetCard(Console.ReadLine());
        if (card == null)
        {
            Console.WriteLine("I don't know that card... Please try again");
            return false;
        }

        foreach (Card deckCard in this.deck)
        {
            if (deckCard.ToString() == card.ToString())
            {
                result = deckCard;
                break;
            }
        }
        if (result == null)
        {
            Console.WriteLine("That card is not in your deck. Try again.");
            return false;
        }
        this.deck.Remove(result);
        this.hand.Add(result);
        if (CardDrawn != null)
            CardDrawn(result);
        return true;
    }

    public void InvokeSpellEvent(Spell spell)
    {
        if (SpellCast != null)
            SpellCast(spell);
    }
    public void InvokeDrawEvent(Card card)
    {
        if (CardDrawn != null)
            CardDrawn(card);
    }
    public void InvokeSummonEvent(Minion minion)
    {
        if (SummonMinion != null)
            SummonMinion(minion);
    }

    public static Hero GetHero(string heroName)
    {
        if (heroes == null)
        {
            heroes = new List<Hero>();
            createHeroes();
        }
        foreach(Hero hero in heroes)
        {
            if (hero.GetType().ToString() == heroName)
                return (Hero)hero.GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        }
        return null;
    }

    private static void createHeroes()
    {
        IEnumerable<System.Type> o = typeof(Hero).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Hero)) && !type.IsAbstract);
        foreach (System.Type hero in o)
        {
            Hero newHero = (Hero) hero.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
            heroes.Add(newHero);
        }
    }
}

class Mage : Hero
{
    public override void HeroPower()
    {
        IDamagable target = getTarget();
        target.Damage(1);
    }
}

abstract class Boss : Hero
{
    public Boss() : base()
    {
        createDeck();
    }

    public static List<Boss> createBosses()
    {
        IEnumerable<System.Type> o = typeof(Boss).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Boss)) && !type.IsAbstract);
        List<Boss> result = new List<Boss>();
        foreach (System.Type b in o)
        {
            Boss boss = (Boss)b.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
            result.Add(boss);
        }
        return result;
    }
    protected abstract void createDeck();
    protected void addDeck(string card, int amount)
    {
        Card cardy;
        for (int i = 0; i < amount; i++)
        {
            cardy = Card.GetCard(card);
            cardy.owner = this;
            deck.Add(cardy);
        }
    }

    public override void DrawCards(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            UnknownCard uc = new UnknownCard(this.deck);

        }
    }
}

class AnubRekhan : Boss
{
    public AnubRekhan() : base()
    {
        this.name = "Anub'rekhan";
    }

    public override void HeroPower()
    {
        this.availableMana -= 2;
        Effects.Summon(this.onBoard, new NerubianAnub());
    }

    protected override void createDeck()
    {
        addDeck("ShadeOfNaxxramas", 2);
        addDeck("Abomination", 2);
        addDeck("CrazedAlchemist", 2);
        addDeck("Deathlord", 2);
        addDeck("NerubianEgg", 3);
        addDeck("HauntedCreeper", 2);
        addDeck("StoneskinGargoyle", 2);
        addDeck("Deathcharger", 2);
        addDeck("NerubarWeblord", 2);
        addDeck("AnubarAmbusher", 1);

        addDeck("Shadowflame", 1);
        addDeck("Frostbolt", 2);
        addDeck("MortalCoil", 2);
        addDeck("ShadowBolt", 2);
        addDeck("LocustSwarm", 3);
    }
}

class MageEasy : Boss
{
    public override void HeroPower()
    {
        throw new NotImplementedException();
    }

    protected override void createDeck()
    {
        addDeck("MurlocRaider", 2);
        addDeck("BloodfenRaptor", 2);
        addDeck("OasisSnapjaw", 2);
        addDeck("RiverCrocolisk", 2);
        addDeck("Wolfrider", 2);
        addDeck("RaidLeader", 2);
        addDeck("BoulderfistOgre", 2);
        addDeck("Nightblade", 2);
        addDeck("NoviceEngineer", 2);
        addDeck("SenjinShieldmasta", 2);

        addDeck("ArcaneMissiles", 2);
        addDeck("Fireball", 2);
        addDeck("Polymorph", 2);
        addDeck("ArcaneIntellect", 2);
        addDeck("ArcaneExplosion", 2);
    }
}