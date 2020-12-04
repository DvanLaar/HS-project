using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Classes
{
    internal class Mage : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        protected Mage()
        {
            SetHeropower();
        }

        protected Mage(bool id, bool nw) : base(id, nw)
        {
            SetHeropower();
        }

        private void SetHeropower()
        {
            HeroPower = b =>
            {
                if (ManaProtected < 2 || HeroPowerUsed)
                    return null;

                var results = new List<MasterBoardContainer>();
                var opponent = b.Me.Id == Id ? b.Opp : b.Me;
                foreach (var m in opponent.OnBoard)
                {
                    var clone = b.Clone();
                    var I = clone.Me.Id == Id ? clone.Me : clone.Opp;
                    (clone.Me.Id == opponent.Id ? clone.Me : clone.Opp).OnBoard[opponent.OnBoard.IndexOf(m)].TakeDamage(1);
                    I.Mana -= 2;
                    I.HeroPowerUsed = true;
                    results.Add(new MasterBoardContainer(clone) { Action = "Ping " + m });
                }
                foreach (var m in OnBoard)
                {
                    var clone = b.Clone();
                    var I = clone.Me.Id == Id ? clone.Me : clone.Opp;
                    I.Mana -= 2;
                    I.HeroPowerUsed = true;
                    (clone.Me.Id == Id ? clone.Me : clone.Opp).OnBoard[OnBoard.IndexOf(m)].TakeDamage(1);
                    results.Add(new MasterBoardContainer(clone) { Action = "Ping " + m });
                }

                var c = b.Clone();
                (c.Me.Id == opponent.Id ? c.Me : c.Opp).TakeDamage(1);
                var me = c.Me.Id == Id ? c.Me : c.Opp;
                me.Mana -= 2;
                me.HeroPowerUsed = true;
                results.Add(new MasterBoardContainer(c) { Action = "Ping Face" });

                c = b.Clone();
                (c.Me.Id == Id ? c.Me : c.Opp).TakeDamage(1);
                me = c.Me.Id == Id ? c.Me : c.Opp;
                me.Mana -= 2;
                me.HeroPowerUsed = true;
                results.Add(new MasterBoardContainer(c) { Action = "Ping Own Face" });

                return new ChoiceSubBoardContainer(results, b, "Use Hero Power");
            };
        }
    }
}