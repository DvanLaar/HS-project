using System.Collections.Generic;
using System.Linq;
using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Hunter.Minions
{
    internal class Houndmaster : BattlecryMinion
    {
        public Houndmaster() : base(4, 4, 3)
        {
            SetBattlecry(b =>
            {
                var beasts = Owner.OnBoard.Where(m => m.Beast).ToList();

                if (beasts.Count == 0)
                    return new SingleSubBoardContainer(b, b, "Play " + this);

                var result = new List<MasterBoardContainer>();

                foreach (var m in beasts)
                {
                    var cln = b.Clone();
                    var me = cln.Me.Id == Owner.Id ? cln.Me : cln.Opp;
                    var target = me.OnBoard[Owner.OnBoard.IndexOf(m)];
                    target.AlterAttack(2);
                    target.AddHealth(2);
                    target.Taunt = true;
                    result.Add(new MasterBoardContainer(cln) { Action = "Target " + target });
                }

                return new ChoiceSubBoardContainer(result, b, "Play " + this);
            });
        }
    }
}