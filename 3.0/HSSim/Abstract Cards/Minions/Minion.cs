using System.Collections.Generic;

abstract class Minion : Card, IDamagable
{
    public int baseAttack, baseHealth, maxHealth;
    protected int curHealth;
    public bool Taunt = false, windfury = false, megaWindfury = false, cantAttackHeroes = false;
    public int maxAttacks { get { if (megaWindfury) return 4; if (windfury) return 2; return 1; } }

    public bool Beast = false, Totem = false, Mech = false, Murloc = false;
    private bool charge;

    public delegate void EmptyHandler();
    public event EmptyHandler Transform;
    public event EmptyHandler Destroy;
    public event EmptyHandler OnDamaged;

    public virtual int Health
    {
        get => curHealth; set
        {
            if (value < curHealth)
                OnDamaged?.Invoke();
            curHealth = value;
            if (curHealth <= 0)
            {
                StartDestroy();
            }
        }
    }
    public int Attack { get; set; }
    public int AttacksLeft { get; set; }
    public bool Charge { get => charge; set { if (value) if (charge) charge = value; else { charge = value; AttacksLeft = maxAttacks; } else charge = value; } }
    public bool Damaged { get => Health != maxHealth; }

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

    public void TakeDamage(int dmg)
    {
        Health -= dmg;
    }

    public override bool CanPlay(Board b)
    {
        return owner.onBoard.Count < 7 && base.CanPlay(b);
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        int min = Attack + Health;
        if (owner.onBoard.Count == 0)
            min += 2 + owner.maxMana;
        return owner.CalcValue(cards: -1, minions: min) - owner.CalcValue();
    }

    public void StartTransform() //Maybe include target, perform transform too
    {
        Transform?.Invoke();
    }

    public void StartDestroy()
    {
        Destroy?.Invoke();
        owner.onBoard.Remove(this);
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

            if (!cantAttackHeroes)
            {
                Board clone = curBoard.Clone();
                Hero Opp = clone.me.id == opponent.id ? clone.me : clone.opp;
                Minion Att = clone.me.id == owner.id ? clone.me.onBoard[myIndex] : clone.opp.onBoard[myIndex];
                clone.Attack(Att, Opp);
                results.Add(new MasterBoardContainer(clone) { action = "Attacks Face" });
            }

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