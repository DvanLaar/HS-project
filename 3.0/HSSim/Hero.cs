using System;
using System.Collections.Generic;
using System.Collections.Specialized;

abstract class Hero : IDamagable
{
    public List<Card> hand;
    public Dictionary<Card, int> deck;
    public abstract Dictionary<Card, int> DeckList { get; set; }
    public List<Minion> onBoard;
    protected int mana;
    public int maxMana;
    public bool id, HeroPowerUsed;
    protected Func<Board, SubBoardContainer> HeroPower;
    public Weapon CurrentWeapon;

    public List<Func<Board, SubBoardContainer>> EndTurnFuncs;
    public List<int> SingleEndTurnFuncs;

    public delegate void MinionHandler(Minion m);
    public event MinionHandler Summon;

    public delegate void WeaponHandler(Weapon w);
    public event WeaponHandler DestroyWeapon;

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Attack { get; set; }
    public int AttacksLeft { get; set; }
    public int Mana { get => mana; set { if (value >= 10) mana = 10; else mana = value; } }
    public int SpellDamage { get; set; }
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

    public Hero(bool id, bool nw) : this()
    {
        this.id = id;
        if (nw)
        {
            foreach (KeyValuePair<Card, int> kvp in DeckList)
            {
                deck.Add(kvp.Key, kvp.Value);
            }
        }
    }

    public Hero()
    {
        hand = new List<Card>();
        deck = new Dictionary<Card, int>();
        onBoard = new List<Minion>();
        EndTurnFuncs = new List<Func<Board, SubBoardContainer>>();
        SingleEndTurnFuncs = new List<int>();
        Health = 30;
        Attack = 0;
        Armor = 0;
        SpellDamage = 0;
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
        h.SpellDamage = SpellDamage;

        return h;
    }

    public void StartSummon(Minion m)
    {
        Summon?.Invoke(m);
        if (m.charge)
          m.AttacksLeft = m.maxAttacks;
        onBoard.Add(m);
    }

    public void StartDestroyWeapon(Weapon w)
    {
        DestroyWeapon?.Invoke(w);
        CurrentWeapon = null;
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

    public void TakeDamage(int amount)
    {
        if (amount > Armor)
        {
            int newAmount = amount - Armor;
            Armor = 0;
            Health -= amount;
        }
        else
        {
            Armor -= amount;
        }
    }

    public SubBoardContainer UseHeroPower(Board b)
    {
        return HeroPower.Invoke(b);
    }

    public MasterBoardContainer DrawCard(Board b, Card c)
    {
        Board clone = b.Clone();
        Hero me = b.me.id == id ? clone.me : clone.opp;
        me.deck[c]--;
        if (me.deck[c] == 0)
            me.deck.Remove(c);
        Card cln = c.Clone();
        me.hand.Add(cln);
        cln.owner = me;
        return new MasterBoardContainer(clone) { action = cln + "" } ;

        //return new MultipleBoardContainer(result, this + " draws card");
    }

    public SubBoardContainer DrawOneCard(Board b)
    {
        List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
        foreach (KeyValuePair<Card, int> c in deck)
        {
            result.Add((DrawCard(b, c.Key), c.Value));
        }
        return new RandomSubBoardContainer(result, b, "Draw One Card");
    }

    public SubBoardContainer DrawTwoCards(Board b)
    {
        List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
        List<Card> seen = new List<Card>();
        foreach (KeyValuePair<Card, int> c in deck)
        {
            MasterBoardContainer drawOne = DrawCard(b, c.Key);
            foreach (KeyValuePair<Card, int> c2 in (drawOne.board.me.id == id ? drawOne.board.me : drawOne.board.opp).deck)
            {
                if (seen.Contains(c2.Key))
                    continue;
                MasterBoardContainer mbc = DrawCard(drawOne.board, c2.Key);
                mbc.action = c.Key + " + " + c2.Key;
                result.Add((mbc, c.Key == c2.Key ? 1 : c.Value * c2.Value));
            }
            seen.Add(c.Key);
        }
        return new RandomSubBoardContainer(result, b, "Draw Two Cards");
    }

    public SubBoardContainer DrawThreeCards(Board b)
    {
        List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
        List<Card> seen = new List<Card>();
        foreach (KeyValuePair<Card, int> c in deck)
        {
            List<Card> seen2 = new List<Card>();
            MasterBoardContainer drawOne = DrawCard(b, c.Key);
            foreach (KeyValuePair<Card, int> c2 in (drawOne.board.me.id == id ? drawOne.board.me : drawOne.board.opp).deck)
            {
                if (seen.Contains(c2.Key))
                    continue;
                MasterBoardContainer drawTwo = DrawCard(drawOne.board, c2.Key);
                foreach (KeyValuePair<Card, int> c3 in (drawTwo.board.me.id == id ? drawTwo.board.me : drawTwo.board.opp).deck)
                {
                    if (seen2.Contains(c3.Key) || seen.Contains(c3.Key))
                        continue;
                    MasterBoardContainer mbc = DrawCard(drawTwo.board, c3.Key);
                    mbc.action = "Draw " + c + " + " + c2 + " + " + c3;
                    result.Add((mbc, c.Value * c2.Value * c3.Value));
                }
                seen2.Add(c2.Key);
            }
            seen.Add(c.Key);
        }
        return new RandomSubBoardContainer(result, b, "Draw Three Cards");
    }



    public void EndTurn(Board mbc)
    {
        for (int i = EndTurnFuncs.Count - 1; i >= 0; i--)
        {
            mbc.toPerform.Push(EndTurnFuncs[i]);
        }
        for (int i = SingleEndTurnFuncs.Count - 1; i >= 0; i--)
        {
            EndTurnFuncs.RemoveAt(SingleEndTurnFuncs[i]);
        }
        SingleEndTurnFuncs = new List<int>();

        //mbc.children = new RandomSubBoardContainer(boards, mbc.board, )
    }

    public SubBoardContainer StartTurn(Board b)
    {
        maxMana++;
        mana = maxMana;
        return DrawOneCard(b);
    }
}