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

                        result.Add((new MasterBoardContainer(cln) {Action = targets}, 1));
                    }
                }

                return new RandomSubBoardContainer(result, b, "Play " + this);
            });
        }

        public override bool CanPlay(Board b)
        {
            return base.CanPlay(b) && (Owner.Id == b.Me.Id ? b.Opp : b.Me).OnBoard.Count >= 1;
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return 0;

            var opp = b.Me.Id == Owner.Id ? b.Opp : b.Me;
            int damage = 3 + Owner.SpellDamage, cases = 0;
            double count = 0;

            if (opp.OnBoard.Count == 1)
            {
                if (opp.OnBoard[0].Health <= damage)
                {
                    return opp.CalcValue() -
                           opp.CalcValue(minions: -2 - opp.OnBoard[0].Health - opp.OnBoard[0].Attack - opp.MaxMana);
                }
                return opp.CalcValue() - opp.CalcValue(minions: -1 * damage);
            }

            for (var i = 0; i < opp.OnBoard.Count - 1; i++)
            {
                for (var j = i + 1; j < opp.OnBoard.Count; j++)
                {
                    if (opp.OnBoard[i].Health <= damage && opp.OnBoard[j].Health <= damage && opp.OnBoard.Count == 2)
                    {
                        return opp.CalcValue() - opp.CalcValue(minions: -2 - opp.OnBoard[i].Health -
                                                                        opp.OnBoard[i].Attack - opp.OnBoard[j].Health -
                                                                        opp.OnBoard[j].Attack - opp.MaxMana);
                    }

                    int firstMinionVal;
                    if (opp.OnBoard[i].Health <= damage)
                        firstMinionVal = opp.OnBoard[i].Health + opp.OnBoard[i].Attack;
                    else
                        firstMinionVal = damage;
                    int secondMinionVal;
                    if (opp.OnBoard[j].Health <= damage)
                        secondMinionVal = opp.OnBoard[j].Health + opp.OnBoard[j].Attack;
                    else
                        secondMinionVal = damage;

                    count += opp.CalcValue() - opp.CalcValue(minions: 0 - firstMinionVal - secondMinionVal, cards: -1);
                    cases++;
                }
            }

            return count / cases;
        }
    }
}