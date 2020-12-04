using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Hunter.Spells
{
    internal class MultiShot : Spell
    {
        public MultiShot() : base(4)
        {
            SetSpell(b =>
            {
                var result = new List<(MasterBoardContainer, int)>();
                var opp = b.Me.Id == Owner.Id ? b.Opp : b.Me;
                for (var i = 0; i < opp.OnBoard.Count; i++)
                {
                    for (var j = i + 1; j < opp.OnBoard.Count; j++)
                    {
                        var cln = b.Clone();
                        var oppCln = cln.Me.Id == Owner.Id ? cln.Opp : cln.Me;
                        var targets = oppCln.OnBoard[j] + " + " + oppCln.OnBoard[i];
                        oppCln.OnBoard[j].TakeDamage(3 + Owner.SpellDamage);
                        oppCln.OnBoard[i].TakeDamage(3 + Owner.SpellDamage);

                        result.Add((new MasterBoardContainer(cln) { Action = targets }, 1));
                    }
                }
                return new RandomSubBoardContainer(result, b, "Play " + this);
            });
        }

        public override bool CanPlay(Board b)
        {
            return base.CanPlay(b) && (Owner.Id == b.Me.Id ? b.Opp : b.Me).OnBoard.Count >= 2;
        }
    }
}