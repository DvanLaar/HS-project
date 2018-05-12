using System;
using System.Collections.Generic;
using System.Collections.Specialized;

abstract class Hero : IDamagable
{
    public List<Card> hand;
    public Dictionary<Card, int> deck;
    public List<Minion> onBoard;
    protected int mana;
    public int maxMana;
    public bool id, HeroPowerUsed;
    protected Func<Board, SubBoardContainer> HeroPower;

    public delegate void MinionHandler(Minion m);
    public event MinionHandler Summon;

    public int Health { get; set; }
    public int Attack { get; set; }
    public int AttacksLeft { get; set; }
    public int Mana { get => mana; set { if (value >= 10) mana = 10; mana = value; } }
    public double value { get
        {
            return 2 * Math.Sqrt(Health) + (hand.Count > 3 ? (hand.Count - 3) * 2 + 9 : hand.Count * 3) + Math.Sqrt(cardsInDeck) + minionValue ;
        } }
    public int cardsInDeck { get
        {
            int res = 0;
            foreach (KeyValuePair<Card, int> ci in deck)
                res += ci.Value;
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
        deck = new Dictionary<Card, int>();
        onBoard = new List<Minion>();
        Health = 30;
        Attack = 0;
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
        foreach (KeyValuePair<Card, int> kvp in deck)
        {
            h.deck.Add(kvp.Key, kvp.Value);
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

    public SubBoardContainer PerformAttack(Board b)
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

            return new ChoiceSubBoardContainer(results, b, this + " attacks");
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

        return new ChoiceSubBoardContainer(results, b, this + " attacks");
    }

    public SubBoardContainer UseHeroPower(Board b)
    {
        return HeroPower.Invoke(b);
    }

    public Board DrawCard(Board b, Card c)
    {
        Board clone = b.Clone();
        Hero me = b.me.id == id ? clone.me : clone.opp;
        me.deck[c]--;
        if (me.deck[c] == 0)
            me.deck.Remove(c);
        Card cln = c.Clone();
        me.hand.Add(cln);
        cln.owner = me;
        return clone;

        //return new MultipleBoardContainer(result, this + " draws card");
    }

    public SubBoardContainer DrawOneCard(Board b)
    {
        List<(Board, int)> result = new List<(Board, int)>();
        foreach (KeyValuePair<Card, int> c in deck)
        {
            result.Add((DrawCard(b, c.Key), c.Value));
        }
        return new RandomSubBoardContainer(result, b, "Draw One Card");
    }

    public SubBoardContainer DrawTwoCards(Board b)
    {
        List<(Board, int)> result = new List<(Board, int)>();
        List<Card> seen = new List<Card>();
        foreach (KeyValuePair<Card, int> c in deck)
        {
            Board drawOne = DrawCard(b, c.Key);
            foreach (KeyValuePair<Card, int> c2 in (drawOne.me.id == id ? drawOne.me : drawOne.opp).deck)
            {
                if (seen.Contains(c2.Key))
                    continue;
                result.Add((DrawCard(drawOne, c2.Key), c.Key == c2.Key ? 1 : c.Value * c2.Value));
            }
            seen.Add(c.Key);
        }
        return new RandomSubBoardContainer(result, b, "Draw Two Cards");
    }

}