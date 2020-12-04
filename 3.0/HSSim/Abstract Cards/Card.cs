using System.Collections.Generic;
using System.Reflection;

namespace HSSim.Abstract_Cards
{
    internal abstract class Card
    {
        public int BaseCost { get; set; }
        protected int Cost;
        public Hero Owner;
        public abstract SubBoardContainer Play(Board curBoard);
        public abstract double DeltaBoardValue(Board b);
        public virtual Card Clone()
        {
            var c = (Card)GetType().InvokeMember("", BindingFlags.CreateInstance, null, null, null);
            c.BaseCost = BaseCost;
            c.Cost = Cost;
            return c;
        }

        public virtual void SetOwner(Hero owner)
        {
            Owner = owner;
        }

        public virtual bool CanPlay(Board b)
        {
            return Cost <= Owner.Mana;
        }

        protected Card(int mana)
        {
            BaseCost = mana;
            Cost = mana;
        }

        public override string ToString()
        {
            var bs = base.ToString();
            for (var i = 1; i < bs.Length; i++)
            {
                if (bs[i] >= 'A' && bs[i] <= 'Z')
                {
                    bs = bs.Insert(i, " ");
                    i++;
                }
            }
            return bs;
        }

        protected SubBoardContainer DealDamage(int dmg, Board b)
        {
            var result = new List<MasterBoardContainer>();
            var opponent = b.Me.Id == Owner.Id ? b.Opp : b.Me;

            var clone = b.Clone();
            clone.Me.TakeDamage(dmg);
            (clone.Me.Id == Owner.Id ? clone.Me : clone.Opp).Mana -= Cost;
            result.Add(new MasterBoardContainer(clone) { Action = "Hit Own Face" });

            clone = b.Clone();
            clone.Opp.TakeDamage(dmg);
            (clone.Me.Id == Owner.Id ? clone.Me : clone.Opp).Mana -= Cost;
            result.Add(new MasterBoardContainer(clone) { Action = "Hit Face" });

            foreach (var m in Owner.OnBoard)
            {
                clone = b.Clone();
                var me = clone.Me.Id == Owner.Id ? clone.Me : clone.Opp;
                me.Mana -= Cost;
                var target = me.OnBoard[Owner.OnBoard.IndexOf(m)];
                target.TakeDamage(dmg);
                result.Add(new MasterBoardContainer(clone) { Action = "Hit " + target });
            }

            foreach (var m in opponent.OnBoard)
            {
                clone = b.Clone();
                var cloneOpp = clone.Me.Id == opponent.Id ? clone.Me : clone.Opp;
                (clone.Me.Id == Owner.Id ? clone.Me : clone.Opp).Mana -= Cost;
                var target = cloneOpp.OnBoard[opponent.OnBoard.IndexOf(m)];
                target.TakeDamage(dmg);
                result.Add(new MasterBoardContainer(clone) { Action = "Hit " + target });
            }

            return new ChoiceSubBoardContainer(result, b, "play " + this);
        }
    }
}