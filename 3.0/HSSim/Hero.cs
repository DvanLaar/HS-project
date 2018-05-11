using System;
using System.Collections.Generic;
using System.Collections.Specialized;

abstract class Hero : IDamagable
{
    public List<Card> hand;
    public List<(Card, int)> deck;
    public List<Minion> onBoard;
    private int health, attack, attacksLeft;
    protected int mana;
    public int maxMana;
    public bool id, HeroPowerUsed;
    protected Func<Board, BoardContainer> HeroPower;

    public delegate void MinionHandler(Minion m);
    public event MinionHandler Summon;

    public int Health { get => health; set => health = value; }
    public int Attack { get => attack; set => attack = value; }
    public int AttacksLeft { get => attacksLeft; set => attacksLeft = value; }
    public int Mana { get => mana; set { if (value >= 10) mana = 10; mana = value; } }
    public double value { get
        {
            return 2 * Math.Sqrt(Health) + (hand.Count > 3 ? (hand.Count - 3) * 2 + 9 : hand.Count * 3) + Math.Sqrt(cardsInDeck) + minionValue ;
        } }
    public int cardsInDeck { get
        {
            int res = 0;
            foreach ((Card, int) ci in deck)
                res += ci.Item2;
            return res;
        } }
    public int minionValue{ get
        {
            if (onBoard.Count == 0)
                return -2 - maxMana;
            int res = 0;
            foreach (Minion m in onBoard)
                res += m.Health + m.Attack;
            return res;
        }
    }

    public Hero(bool id) : this()
    {
        this.id = id;
    }

    public Hero()
    {
        hand = new List<Card>();
        deck = new List<(Card, int)>();
        onBoard = new List<Minion>();
        health = 30;
        attack = 0;
        mana = 0;
        maxMana = 0;
        HeroPowerUsed = false;


        //TEMP
    }

    public Hero Clone()
    {
        Hero h = (Hero)GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        foreach (Card c in hand)
        {
            Card copy = c.Clone();
            copy.SetOwner(h);
            h.hand.Add(copy); //Clone
        }
        foreach ((Card, int) kvp in deck)
        {
            Card copy = kvp.Item1.Clone();
            copy.SetOwner(h);
            h.deck.Add((copy, kvp.Item2)); //Clone
        }
        foreach (Minion m in onBoard)
        {
            Minion copy = (Minion)m.Clone();
            copy.SetOwner(h);
            h.onBoard.Add(copy); //Clone
        }
        h.Health = Health;
        h.Attack = Attack;
        h.AttacksLeft = AttacksLeft;
        h.mana = mana;
        h.maxMana = maxMana;
        h.Summon = Summon; //Correct?
        h.id = id;
        h.HeroPowerUsed = HeroPowerUsed;

        return h;
    }

    public void StartSummon(Minion m)
    {
        Summon?.Invoke(m);
        if (m.charge)
          m.AttacksLeft = m.maxAttacks;
        onBoard.Add(m);
    }

    public BoardContainer PerformAttack(Board b)
    {
        if (Attack <= 0)
            return null;

        List<Board> results = new List<Board>();

        Hero me = b.me.id == id ? b.me : b.opp;
        Hero opp = b.me.id == id ? b.opp : b.me;

        if(opp.onBoard.TrueForAll((m) => !m.Taunt)) //All minions don't have taunt => no minion has taunt
        {
            foreach (Minion m in opp.onBoard)
            {
                Board clone = b.Clone();
                Hero Attacker = clone.me.id == id ? clone.me : clone.opp;
                Minion Defender = clone.me.id == opp.id ? clone.me.onBoard[opp.onBoard.IndexOf(m)] : clone.opp.onBoard[opp.onBoard.IndexOf(m)];
                clone.Attack(Attacker, Defender);
                results.Add(clone);
            }

            Board c = b.Clone();
            Hero Att = c.me.id == id ? c.me : c.opp;
            Hero Def = c.me.id == opp.id ? c.me : c.opp;
            c.Attack(Att, Def);
            results.Add(c);

            return new MultipleChoiceBoardContainer(results, this + " attacks");
        }
        // At least one minion has taunt
        foreach (Minion m in opp.onBoard)
        {
            if (!m.Taunt)
                continue;

            Board clone = b.Clone();
            Hero Attacker = clone.me.id == id ? clone.me : clone.opp;
            Minion Defender = clone.me.id == opp.id ? clone.me.onBoard[opp.onBoard.IndexOf(m)] : clone.opp.onBoard[opp.onBoard.IndexOf(m)];
            clone.Attack(Attacker, Defender);
            results.Add(clone);
        }

        return new MultipleChoiceBoardContainer(results, this + " attacks");
    }

    public BoardContainer UseHeroPower(Board b)
    {
        return HeroPower.Invoke(b);
    }

    public BoardContainer DrawCard(Board b)
    {
        List<(Board, int)> result = new List<(Board, int)>();
        foreach((Card, int) kvp in deck)
        {
            Board clone = b.Clone();
            Hero me = b.me.id == id ? clone.me : clone.opp;
            (Card, int) kopie = me.deck[deck.IndexOf(kvp)];
            int inDeck = kopie.Item2;
            kopie = (kopie.Item1, kopie.Item2 - 1);
            me.deck[deck.IndexOf(kvp)] = kopie;
            if (kopie.Item2 == 0)
                me.deck.Remove(kopie);
            me.hand.Add(kopie.Item1);
            result.Add((clone, kopie.Item2));
        }

        return new MultipleBoardContainer(result, this + " draws card");
    }

}