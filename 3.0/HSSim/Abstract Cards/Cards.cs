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
        c.cost = cost;
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

    public override string ToString()
    {
        string bs = base.ToString();
        for (int i = 1; i < bs.Length; i++)
        {
            if (bs[i] >= 'A' && bs[i] <= 'Z')
            {
                bs = bs.Insert(i, " ");
                i++;
            }
        }
        return bs;
    }

    public SubBoardContainer DealDamage(int dmg, Board b)
    {
        List<MasterBoardContainer> result = new List<MasterBoardContainer>();
        Hero opponent = b.me.id == owner.id ? b.opp : b.me;

        Board clone;

        clone = b.Clone();
        clone.me.Health -= dmg;
        (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
        result.Add(new MasterBoardContainer(clone) { action = "Hit Own Face" });

        clone = b.Clone();
        clone.opp.Health -= dmg;
        (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
        result.Add(new MasterBoardContainer(clone) { action = "Hit Face" });

        foreach (Minion m in owner.onBoard)
        {
            clone = b.Clone();
            Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
            me.Mana -= cost;
            Minion target = me.onBoard[owner.onBoard.IndexOf(m)];
            target.Health -= dmg;
            result.Add(new MasterBoardContainer(clone) { action = "Hit " + target });
        }

        foreach (Minion m in opponent.onBoard)
        {
            clone = b.Clone();
            Hero Opponent = clone.me.id == opponent.id ? clone.me : clone.opp;
            (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
            Minion target = Opponent.onBoard[opponent.onBoard.IndexOf(m)];
            target.Health -= dmg;
            result.Add(new MasterBoardContainer(clone) { action = "Hit " + target });
        }

        return new ChoiceSubBoardContainer(result, b, "play " + this);
    }
}

abstract class Minion : Card, IDamagable
{
    int baseAttack, baseHealth, maxHealth;
    protected int curHealth;
    public bool Taunt = false, charge = false, windfury = false, megaWindfury = false;
    public int maxAttacks { get { if (megaWindfury) return 4; if (windfury) return 2; return 1; } }

    public bool Beast = false;

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

        List<MasterBoardContainer> results = new List<MasterBoardContainer>();
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
                results.Add(new MasterBoardContainer(b) { action = "Attacks " + Defender });
            }

            Board clone = curBoard.Clone();
            Hero Opp = clone.me.id == opponent.id ? clone.me : clone.opp;
            Minion Att = clone.me.id == owner.id ? clone.me.onBoard[myIndex] : clone.opp.onBoard[myIndex];
            clone.Attack(Att, Opp);
            results.Add(new MasterBoardContainer(clone) { action = "Attacks Face" });

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
            results.Add(new MasterBoardContainer(b) { action = "Attacks " + Defender });
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
        bool debug2 = owner.hand.Contains(this);
        if (!CanPlay(curBoard))
            return null;

        Board clone = curBoard.Clone();
        Hero own = owner.id == clone.me.id ? clone.me : clone.opp; //Move to card
        own.hand.RemoveAt(owner.hand.IndexOf(this));
        own.Mana -= cost;

        return Cast.Invoke(clone);
    }
}

class UnknownCard : Card
{
    List<Card> options;

    public UnknownCard() : base(0)
    {
        options = new List<Card>();
    }

    public UnknownCard(List<Card> options) : base(0)
    {
        options.Sort((a, b) => a.baseCost.CompareTo(b.baseCost));
        baseCost = options[0].baseCost;
        cost = baseCost;
        this.options = options;
    }

    public UnknownCard(Dictionary<Card, int> DeckList) : base(0)
    {
        Card[] arr = new Card[DeckList.Keys.Count];
        DeckList.Keys.CopyTo(arr, 0);
        options = new List<Card>(arr);
        options.Sort((a, b) => a.baseCost.CompareTo(b.baseCost));
        baseCost = options[0].baseCost;
        cost = baseCost;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;
        List<SubBoardContainer> result = new List<SubBoardContainer>();
        foreach (Card c in options)
        {
            c.SetOwner(owner);
            if (c.CanPlay(curBoard))
            {
                Card toPlay = c.Clone();
                Board cln = curBoard.Clone();
                (cln.curr == cln.me.id ? cln.me : cln.opp).hand[owner.hand.IndexOf(this)] = toPlay;
                toPlay.SetOwner(cln.curr == cln.me.id ? cln.me : cln.opp);
                //bool debug = c.owner.hand.Contains(c);
                result.Add(toPlay.Play(cln));
            }
        }
        result.Sort((a, b) => b.value.CompareTo(a.value));
        if (result.Count == 0)
            return null;
        return new UnknownSubBoardContainer(result, curBoard, "Play Unknown Card");
    }

    public override Card Clone()
    {
        UnknownCard uc = (UnknownCard)base.Clone();
        uc.options = new List<Card>(options);

        return uc;
    }

    public override void SetOwner(Hero owner)
    {
        base.SetOwner(owner);
        foreach (Card c in options)
            c.owner = owner;
    }
}