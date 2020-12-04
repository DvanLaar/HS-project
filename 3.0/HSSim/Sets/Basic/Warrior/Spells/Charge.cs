using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Warrior.Spells
{
    internal class Charge : Spell
    {
        public Charge() : base(1)
        {
            SetSpell(b =>
            {
                var result = new List<MasterBoardContainer>();
                for (var i = 0; i < Owner.OnBoard.Count; i++)
                {
                    var clone = b.Clone();
                    var me = Owner.Id == clone.Me.Id ? clone.Me : clone.Opp;
                    var m = me.OnBoard[i];

                    m.Charge = true;
                    m.CantAttackHeroes = true;
                    SubBoardContainer Func(Board brd)
                    {
                        m.CantAttackHeroes = false;
                        return new SingleSubBoardContainer(brd, brd, m + "loses can't attack heroes");
                    }
                    me.EndTurnFuncs.Add(Func);
                    me.SingleEndTurnFuncs.Add(me.EndTurnFuncs.IndexOf(Func));
                    result.Add(new MasterBoardContainer(clone) { Action = "Target " + m });
                }
                return new ChoiceSubBoardContainer(result, b, "Play " + this);
            });
        }

        public override bool CanPlay(Board b)
        {
            return base.CanPlay(b) && Owner.OnBoard.Count > 0;
        }
    }
}