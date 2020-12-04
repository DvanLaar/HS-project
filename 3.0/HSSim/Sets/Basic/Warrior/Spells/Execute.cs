using System.Collections.Generic;
using HSSim.Abstract_Cards;

 namespace HSSim.Sets.Basic.Warrior.Spells
{
    internal class Execute : Spell
    {
        public Execute() : base(2)
        {
            SetSpell(b =>
            {
                var result = new List<MasterBoardContainer>();
                var opponent = Owner.Id == b.Me.Id ? b.Opp : b.Me;
                foreach (var m in opponent.OnBoard)
                {
                    if (!m.Damaged)
                        continue;

                    var clone = b.Clone();
                    var opp = Owner.Id == clone.Me.Id ? clone.Opp : clone.Me;
                    opp.OnBoard[opponent.OnBoard.IndexOf(m)].StartDestroy();
                    result.Add(new MasterBoardContainer(clone) { Action = "Target " + m });
                }

                if (result.Count == 0)
                    return null;

                return new ChoiceSubBoardContainer(result, b, "Play " + this);
            });
        }

        public override bool CanPlay(Board b)
        {
            return base.CanPlay(b) && !(b.Me.Id == Owner.Id ? b.Opp : b.Me).OnBoard.TrueForAll(m => !m.Damaged);
        }

        public override double DeltaBoardValue(Board b)
        {
            Hero me = b.Me.Id == Owner.Id ? b.me : b.opp;
            Hero opp = b.me.id == owner.id ? b.opp : b.me;
            int max = 0;
            
            foreach (Minion m in opp.onBoard)
            {
                if (!m.Damaged)
                    continue;

                if (opp.onBoard.Count == 1)
                {
                    max = m.Health + m.Attack + 2 + opp.maxMana;
                }
                else
                {
                    max = Math.Max(max, m.Health + m.Attack);
                }
            }

            return opp.CalcValue() + me.CalcValue(cards: -1) - opp.CalcValue(minions: -1 * max) - me.CalcValue();
        }
    }
}