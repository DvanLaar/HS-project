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

<<<<<<< HEAD
    public override bool CanPlay(Board b)
    {
        return base.CanPlay(b) && (owner.id == b.me.id ? b.opp : b.me).onBoard.Count >= 1;
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return 0;

        Hero opp = b.me.id == owner.id ? b.opp : b.me;
        int damage = 3 + owner.SpellDamage, cases = 0;
        double count = 0;

        if (opp.onBoard.Count == 1)
        {
            if (opp.onBoard[0].Health <= damage)
            {
                return opp.CalcValue() - opp.CalcValue(minions: -2 - opp.onBoard[0].Health - opp.onBoard[0].Attack - opp.maxMana);
            }
            else
            {
                return opp.CalcValue() - opp.CalcValue(minions: -1 * damage);
            }
        }

        for (int i = 0; i < opp.onBoard.Count - 1; i++)
        {
            for (int j = i + 1; j < opp.onBoard.Count; j++)
            {
                if (opp.onBoard[i].Health <= damage && opp.onBoard[j].Health <= damage && opp.onBoard.Count == 2)
                {
                    return opp.CalcValue() - opp.CalcValue(minions: -2 - opp.onBoard[0].Health - opp.onBoard[0].Attack - opp.onBoard[1].Health - opp.onBoard[1].Attack - opp.maxMana);
                }

                int minionival, minionjval = 0;
                if (opp.onBoard[i].Health <= damage)
                    minionival = opp.onBoard[i].Health + opp.onBoard[i].Attack;
                else
                    minionival = damage;

                if (opp.onBoard[j].Health <= damage)
                    minionjval = opp.onBoard[j].Health + opp.onBoard[j].Attack;
                else
                    minionjval = damage;

                count += opp.CalcValue() - opp.CalcValue(minions: 0 - minionival - minionjval, cards: -1);
                cases++;
            }
        }

        return count / cases;
=======
        public override bool CanPlay(Board b)
        {
            return base.CanPlay(b) && (Owner.Id == b.Me.Id ? b.Opp : b.Me).OnBoard.Count >= 2;
        }
>>>>>>> master
    }
}