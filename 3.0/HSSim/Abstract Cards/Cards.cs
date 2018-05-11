using System;
using System.Collections.Generic;

abstract class Card
{
    public int baseCost { get; set; }
    protected int cost;
    public Hero owner;
    public abstract BoardContainer Play(Board curBoard);
    public virtual Card Clone()
    {
        Card c = (Card)GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        c.baseCost = baseCost;
        return c;
    }

    public virtual void SetOwner(Hero owner)
    {
        this.owner = owner;
    }

    public virtual bool CanPlay(Board b)
    {
        return cost <= owner.Mana;
    }

    public Card(int mana)
    {
        baseCost = mana;
        cost = mana;
    }
}

abstract class Minion : Card, IDamagable
{
    int baseAttack, baseHealth, maxHealth, curHealth, curAttack;
    protected int attacksLeft;
    public bool Taunt = false, charge = false, windfury = false, megaWindfury = false;
    public int maxAttacks { get { if (megaWindfury) return 4; if (windfury) return 2; return 1; } }

    public int Health
    {
        get => curHealth; set
        {
            curHealth = value;
            if (curHealth <= 0)
            {
                owner.onBoard.Remove(this);
            }
        }
    }
    public int Attack { get => curAttack; set => curAttack = value; }
    public int AttacksLeft { get => attacksLeft; set => attacksLeft = value; }

    public Minion(int mana, int attack, int health) : base(mana)
    {
        baseAttack = attack;
        curAttack = attack;
        baseHealth = health;
        curHealth = health;
        maxHealth = health;
        cost = mana;
        attacksLeft = 0;
    }

    public override Card Clone()
    {
        Minion m = (Minion)base.Clone();
        m.baseAttack = baseAttack;
        m.baseHealth = baseHealth;
        m.cost = cost;
        m.maxHealth = maxHealth;
        m.curHealth = curHealth;
        m.curAttack = curAttack;
        m.AttacksLeft = attacksLeft;

        return m;
    }

    public override bool CanPlay(Board b)
    {
        return owner.onBoard.Count < 7 && base.CanPlay(b);
    }

    public void AddHealth(int increase)
    {
        if (increase <= 0)
            return;
        maxHealth += increase;
        curHealth += increase;
    }

    public void ReduceHealth(int decrease)
    {
        if (decrease <= 0)
            return;
        maxHealth -= decrease;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
    }

    public void AlterAttack(int alteration)
    {
        if (curAttack + alteration <= 0)
        {
            curAttack = 0;
            return;
        }
        curAttack += alteration;
    }

    public override BoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        Board b = curBoard.Clone();

        Hero ownerClone = owner.id == b.me.id ? b.me : b.opp;
        Minion m = (Minion)ownerClone.hand[owner.hand.IndexOf(this)];

        ownerClone.hand.Remove(m);
        ownerClone.Mana -= cost;
        ownerClone.StartSummon(m);

        return new SingleBoardContainer(b, "play " + this);
    }

    public virtual BoardContainer PerformAttack(Board curBoard)
    {
        if (attacksLeft <= 0)
            return null;

        List<Board> results = new List<Board>();
        Hero opponent = curBoard.me.id == owner.id ? curBoard.opp : curBoard.me;
        int myIndex = owner.onBoard.IndexOf(this);
        if (opponent.onBoard.TrueForAll((m) => !m.Taunt)) //All minions don't have taunt => any minion and hero are valid targets)
        {
            foreach (Minion m in opponent.onBoard)
            {
                Board b = curBoard.Clone();
                int theirIndex = opponent.onBoard.IndexOf(m);
                Minion Attacker = b.me.id == owner.id ? b.me.onBoard[myIndex] : b.opp.onBoard[myIndex];
                Minion Defender = b.me.id == m.owner.id ? b.me.onBoard[theirIndex] : b.opp.onBoard[theirIndex];
                b.Attack(Attacker, Defender);
                results.Add(b);
            }

            Board clone = curBoard.Clone();
            Hero Opp = clone.me.id == opponent.id ? clone.me : clone.opp;
            Minion Att = clone.me.id == owner.id ? clone.me.onBoard[myIndex] : clone.opp.onBoard[myIndex];
            clone.Attack(Att, Opp);
            results.Add(clone);

            return new MultipleChoiceBoardContainer(results, this + " attacks");
        }

        foreach (Minion m in opponent.onBoard)
        {
            if (!m.Taunt)
                continue;

            Board b = curBoard.Clone();
            int theirIndex = opponent.onBoard.IndexOf(m);
            Minion Attacker = b.me.id == owner.id ? b.me.onBoard[myIndex] : b.opp.onBoard[myIndex];
            Minion Defender = b.me.id == m.owner.id ? b.me.onBoard[theirIndex] : b.opp.onBoard[theirIndex];
            b.Attack(Attacker, Defender);
            results.Add(b);
        }

        return new MultipleChoiceBoardContainer(results, this + " attacks");
    }
}

abstract class Spell : Card
{
    protected Func<Board, BoardContainer> Cast;

    public Spell(int mana) : base(mana)
    {

    }

    public void SetSpell(Func<Board, BoardContainer> effect)
    {
        Cast = effect;
    }

    public override BoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        Board clone = curBoard.Clone();
        Hero own = owner.id == clone.me.id ? clone.me : clone.opp; //Move to card
        own.hand.RemoveAt(owner.hand.IndexOf(this));

        return Cast.Invoke(clone);
    }
}