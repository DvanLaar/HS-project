using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HSSim.Abstract_Cards;
using HSSim.Abstract_Cards.Minions;

namespace HSSim
{
    internal abstract class Hero : IDamagable
    {
        public readonly List<Card> Hand;
        public readonly Dictionary<Card, int> Deck;
        public abstract Dictionary<Card, int> DeckList { get; }
        public readonly List<Minion> OnBoard;
        protected int ManaProtected;
        protected int MaxMana;
        public bool Id, HeroPowerUsed;
        protected Func<Board, SubBoardContainer> HeroPower;
        public Weapon CurrentWeapon;

        public readonly List<Func<Board, SubBoardContainer>> EndTurnFuncs;
        public List<int> SingleEndTurnFuncs;

        public delegate void MinionHandler(Minion m);
        public event MinionHandler Summon;

        public delegate void WeaponHandler(Weapon w);
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event WeaponHandler DestroyWeapon;

        public int Health { get; set; }
        public int Armor { get; set; }
        public int Attack { get; set; }
        public int AttacksLeft { get; set; }
        public int Mana { get => ManaProtected; set => ManaProtected = value >= 10 ? 10 : value;
        }
        public int SpellDamage { get; set; }
        public double Value => 2 * Math.Sqrt(Health + Armor) +
                               (Hand.Count > 3 ? ((Hand.Count - 3) * 2) + 9 : Hand.Count * 3) + Math.Sqrt(CardsInDeck) +
                               MinionValue;

        private int CardsInDeck { get
        {
            return Deck.Sum(ci => ci.Value);
        } }

        private int MinionValue{ get
            {
                if (OnBoard.Count == 0)
                    return -2 - _maxMana;
                return OnBoard.Sum(m => m.Health + m.Attack);
            }
        }

        protected Hero(bool id, bool nw) : this()
        {
            Id = id;
            if (!nw) return;
            // ReSharper disable once VirtualMemberCallInConstructor
            Debug.Assert(DeckList != null, nameof(DeckList) + " != null");
            // ReSharper disable once VirtualMemberCallInConstructor
            foreach (var kvp in DeckList)
            {
                Deck.Add(kvp.Key, kvp.Value);
            }
        }

        protected Hero()
        {
            Hand = new List<Card>();
            Deck = new Dictionary<Card, int>();
            OnBoard = new List<Minion>();
            EndTurnFuncs = new List<Func<Board, SubBoardContainer>>();
            SingleEndTurnFuncs = new List<int>();
            Health = 30;
            Attack = 0;
            Armor = 0;
            SpellDamage = 0;
            ManaProtected = 0;
            _maxMana = 0;
            HeroPowerUsed = false;

            //TEMP
        }

        public Hero Clone()
        {
            var h = (Hero)GetType().InvokeMember("", BindingFlags.CreateInstance, null, null, null);
            foreach (var copy in Hand.Select(c => c.Clone()))
            {
                copy.SetOwner(h);
                h.Hand.Add(copy); //Clone
            }
            foreach (var kvp in Deck)
            {
                h.Deck.Add(kvp.Key, kvp.Value);
            }
            foreach (var copy in OnBoard.Select(m => (Minion)m.Clone()))
            {
                copy.SetOwner(h);
                h.OnBoard.Add(copy); //Clone
            }
            h.Health = Health;
            h.Attack = Attack;
            h.AttacksLeft = AttacksLeft;
            h.ManaProtected = ManaProtected;
            h._maxMana = _maxMana;
            h.Summon = Summon; //Correct?
            h.Id = Id;
            h.HeroPowerUsed = HeroPowerUsed;
            h.SpellDamage = SpellDamage;
            h.Armor = Armor;
            h.CurrentWeapon = (Weapon) CurrentWeapon?.Clone();
            h.CurrentWeapon?.SetOwner(h);
            foreach (var effect in EndTurnFuncs)
            {
                h.EndTurnFuncs.Add(effect);
            }
            foreach (var effect in SingleEndTurnFuncs)
            {
                h.SingleEndTurnFuncs.Add(effect);
            }

            return h;
        }

        public double CalcValue(int health = 0, int cards = 0, int deck = 0, int minions = 0)
        {
            var hp = Health + Armor + health;
            var cr = Hand.Count + cards;
            var dc = CardsInDeck + deck;
            var mn = MinionValue + minions;
            return (2 * Math.Sqrt(hp)) + (cr > 3 ? ((cr - 3) * 2) + 9 : cr * 3) + Math.Sqrt(dc) + mn;
        }

        public void StartSummon(Minion m)
        {
            Summon?.Invoke(m);
            if (m.Charge)
                m.AttacksLeft = Minion.MaxAttacks;
            OnBoard.Add(m);
        }

        public void StartDestroyWeapon(Weapon w)
        {
            DestroyWeapon?.Invoke(w);
            CurrentWeapon = null;
        }

        public void EquipWeapon(Weapon w)
        {
            if (CurrentWeapon != null)
                StartDestroyWeapon(CurrentWeapon);
            CurrentWeapon = w;
        }
        public SubBoardContainer PerformAttack(Board b)
        {
            if (Attack <= 0 || AttacksLeft <= 0)
                return null;

            var results = new List<MasterBoardContainer>();

            var opp = b.Me.Id == Id ? b.Opp : b.Me;

            if(opp.OnBoard.TrueForAll(m => !m.Taunt)) //All minions don't have taunt => no minion has taunt
            {
                foreach (var m in opp.OnBoard)
                {
                    var clone = b.Clone();
                    var attacker = clone.Me.Id == Id ? clone.Me : clone.Opp;
                    var defender = clone.Me.Id == opp.Id ? clone.Me.OnBoard[opp.OnBoard.IndexOf(m)] : clone.Opp.OnBoard[opp.OnBoard.IndexOf(m)];
                    Board.Attack(attacker, defender);
                    attacker.AttacksLeft--;
                    results.Add(new MasterBoardContainer(clone) { Action = "Attacks " + m });
                }

                var c = b.Clone();
                var att = c.Me.Id == Id ? c.Me : c.Opp;
                var def = c.Me.Id == opp.Id ? c.Me : c.Opp;
                Board.Attack(att, def);
                att.AttacksLeft--;
                results.Add(new MasterBoardContainer(c) { Action = "Attacks Face" });

                return new ChoiceSubBoardContainer(results, b, this + " attacks");
            }
            // At least one minion has taunt
            foreach (var m in opp.OnBoard)
            {
                if (!m.Taunt)
                    continue;

                var clone = b.Clone();
                var attacker = clone.Me.Id == Id ? clone.Me : clone.Opp;
                var defender = clone.Me.Id == opp.Id ? clone.Me.OnBoard[opp.OnBoard.IndexOf(m)] : clone.Opp.OnBoard[opp.OnBoard.IndexOf(m)];
                Board.Attack(attacker, defender);
                results.Add(new MasterBoardContainer(clone) { Action = "Attacks " + m });
            }

            return new ChoiceSubBoardContainer(results, b, this + " attacks");
        }

        public void TakeDamage(int amount)
        {
            if (amount > Armor)
            {
                var newAmount = amount - Armor;
                Armor = 0;
                Health -= newAmount;
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
            var clone = b.Clone();
            var me = b.Me.Id == Id ? clone.Me : clone.Opp;
            me.Deck[c]--;
            if (me.Deck[c] == 0)
                me.Deck.Remove(c);
            var cln = c.Clone();
            me.Hand.Add(cln);
            cln.Owner = me;
            return new MasterBoardContainer(clone) { Action = cln + "" };

            //return new MultipleBoardContainer(result, this + " draws card");
        }

        public SubBoardContainer DrawOneCard(Board b)
        {
            var result = new List<(MasterBoardContainer, int)>();
            foreach (var c in Deck)
            {
                result.Add((DrawCard(b, c.Key), c.Value));
            }
            return new RandomSubBoardContainer(result, b, "Draw One Card");
        }

        public SubBoardContainer DrawTwoCards(Board b)
        {
            var result = new List<(MasterBoardContainer, int)>();
            var seen = new List<Card>();
            foreach (var c in Deck)
            {
                var drawOne = DrawCard(b, c.Key);
                foreach (var c2 in (drawOne.Board.Me.Id == Id ? drawOne.Board.Me : drawOne.Board.Opp).Deck)
                {
                    if (seen.Contains(c2.Key))
                        continue;
                    var mbc = DrawCard(drawOne.Board, c2.Key);
                    mbc.Action = c.Key + " + " + c2.Key;
                    result.Add((mbc, c.Key == c2.Key ? 1 : c.Value * c2.Value));
                }
                seen.Add(c.Key);
            }
            return new RandomSubBoardContainer(result, b, "Draw Two Cards");
        }

        // ReSharper disable once UnusedMember.Global
        public SubBoardContainer DrawThreeCards(Board b)
        {
            var result = new List<(MasterBoardContainer, int)>();
            var seen = new List<Card>();
            foreach (var c in Deck)
            {
                var seen2 = new List<Card>();
                var drawOne = DrawCard(b, c.Key);
                foreach (var c2 in (drawOne.Board.Me.Id == Id ? drawOne.Board.Me : drawOne.Board.Opp).Deck)
                {
                    if (seen.Contains(c2.Key))
                        continue;
                    var drawTwo = DrawCard(drawOne.Board, c2.Key);
                    foreach (var c3 in (drawTwo.Board.Me.Id == Id ? drawTwo.Board.Me : drawTwo.Board.Opp).Deck)
                    {
                        if (seen2.Contains(c3.Key) || seen.Contains(c3.Key))
                            continue;
                        var mbc = DrawCard(drawTwo.Board, c3.Key);
                        mbc.Action = "Draw " + c + " + " + c2 + " + " + c3;
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
            for (var i = EndTurnFuncs.Count - 1; i >= 0; i--)
            {
                mbc.ToPerform.Push(EndTurnFuncs[i]);
            }
            for (var i = SingleEndTurnFuncs.Count - 1; i >= 0; i--)
            {
                EndTurnFuncs.RemoveAt(SingleEndTurnFuncs[i]);
            }

            if (mbc.ToPerform.Count > 0)
                Console.Write("");
            SingleEndTurnFuncs = new List<int>();
            if (CurrentWeapon != null)
                CurrentWeapon.Active = false;

            //mbc.children = new RandomSubBoardContainer(boards, mbc.board, )
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        public SubBoardContainer StartTurn(Board b)
        {
            _maxMana++;
            ManaProtected = _maxMana;
            AttacksLeft = 1;
            if (CurrentWeapon != null)
                CurrentWeapon.Active = true;
            foreach (var m in OnBoard)
                m.AttacksLeft = Minion.MaxAttacks;
            return DrawOneCard(b);
        }
    }
}