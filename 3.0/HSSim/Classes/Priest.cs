using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Classes
{
    internal class Priest : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        public Priest()
        {
            SetHeroPower();
        }

        public Priest(bool id, bool nw) : base(id, nw)
        {
            SetHeroPower();
        }

        private void SetHeroPower()
        {
            HeroPower = (b =>
            {
                if (Mana < 2 || HeroPowerUsed)
                    return null;

                var results = new List<MasterBoardContainer>();
                var opponent = b.Me.Id == Id ? b.Opp : b.Me;
                foreach (var m in opponent.OnBoard)
                {
                    var clone = b.Clone();
                    var I = clone.Me.Id == Id ? clone.Me : clone.Opp;
                    (clone.Me.Id == opponent.Id ? clone.Me : clone.Opp).OnBoard[opponent.OnBoard.IndexOf(m)].Health += 2;
                    I.Mana -= 2;
                    I.HeroPowerUsed = true;
                    results.Add(new MasterBoardContainer(clone) { Action = "Heal " + m });
                }
                foreach (var m in OnBoard)
                {
                    var clone = b.Clone();
                    var I = clone.Me.Id == Id ? clone.Me : clone.Opp;
                    I.Mana -= 2;
                    I.HeroPowerUsed = true;
                    (clone.Me.Id == Id ? clone.Me : clone.Opp).OnBoard[OnBoard.IndexOf(m)].Health += 2;
                    results.Add(new MasterBoardContainer(clone) { Action = "Heal " + m });
                }

                var c = b.Clone();
                (c.Me.Id == opponent.Id ? c.Me : c.Opp).Health += 2;
                var me = c.Me.Id == Id ? c.Me : c.Opp;
                me.Mana -= 2;
                me.HeroPowerUsed = true;
                results.Add(new MasterBoardContainer(c) { Action = "Heal Opponent's Face" });

                c = b.Clone();
                (c.Me.Id == Id ? c.Me : c.Opp).Health += 2;
                me = c.Me.Id == Id ? c.Me : c.Opp;
                me.Mana -= 2;
                me.HeroPowerUsed = true;
                results.Add(new MasterBoardContainer(c) { Action = "Heal Own Face" });

                return new ChoiceSubBoardContainer(results, b, "Use Hero Power");
            });
        }
    }
}