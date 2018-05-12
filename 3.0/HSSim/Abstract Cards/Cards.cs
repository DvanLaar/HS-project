using System;
using System.Collections.Generic;

abstract class Card
{
    public int baseCost { get; set; }
    protected int cost;
    public Hero owner;
    public abstract SubBoardContainer Play(Board curBoard);
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
    int baseAttack, baseHealth, maxHealth;
    protected int curHealth;
    public bool Taunt = false, charge = false, windfury = false, megaWindfury = false;
    public int maxAttacks { get { if (megaWindfury) return 4; if (windfury) return 2; return 1; } }

    public virtual int Health
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
    public int Attack { get; set; }
    public int AttacksLeft { get; set; }

    public Minion(int mana, int attack, int health) : base(mana)
    {
        baseAttack = attack;
        Attack = attack;
        baseHealth = health;
        curHealth = health;
        maxHealth = health;
        cost = mana;
        AttacksLeft = 0;
    }

    public override Card Clone()
    {
        Minion m = (Minion)base.Clone();
        m.baseAttack = baseAttack;
        m.baseHealth = baseHealth;
        m.cost = cost;
        m.maxHealth = maxHealth;
        m.curHealth = curHealth;
        m.Attack = Attack;
        m.AttacksLeft = AttacksLeft;

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
        if (Attack + alteration <= 0)
        {
            Attack = 0;
            return;
        }
        Attack += alteration;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        Board b = curBoard.Clone();

        Hero ownerClone = owner.id == b.me.id ? b.me : b.opp;
        Minion m = (Minion)ownerClone.hand[owner.hand.IndexOf(this)];

        ownerClone.hand.Remove(m);
        ownerClone.Mana -= cost;
        ownerClone.StartSummon(m);

        return new SingleSubBoardContainer(new MasterBoardContainer(b), curBoard, "Play " + this);
    }

    public virtual SubBoardContainer PerformAttack(Board curBoard)
    {
        if (AttacksLeft <= 0)
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

            return new ChoiceSubBoardContainer(results, curBoard, this + " attacks");
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

        return new ChoiceSubBoardContainer(results, curBoard, this + " attacks");
    }
}

abstract class Spell : Card
{
    protected Func<Board, SubBoardContainer> Cast;

    public Spell(int mana) : base(mana)
    {

    }

    public void SetSpell(Func<Board, SubBoardContainer> effect)
    {
        Cast = effect;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        Board clone = curBoard.Clone();
        Hero own = owner.id == clone.me.id ? clone.me : clone.opp; //Move to card
        own.hand.RemoveAt(owner.hand.IndexOf(this));

        return Cast.Invoke(clone);
    }
}