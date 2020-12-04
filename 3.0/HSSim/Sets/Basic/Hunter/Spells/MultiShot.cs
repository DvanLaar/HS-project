using System.Collections.Generic;

class MultiShot : Spell
{
    public MultiShot() : base(4)
    {
        SetSpell((b) =>
        {
            List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
            Hero opp = b.me.id == owner.id ? b.opp : b.me;
            for (int i = 0; i < opp.onBoard.Count; i++)
            {
                for (int j = i + 1; j < opp.onBoard.Count; j++)
                {
                    Board cln = b.Clone();
                    Hero oppCln = cln.me.id == owner.id ? cln.opp : cln.me;
                    string targets = oppCln.onBoard[j] + " + " + oppCln.onBoard[i];
                    oppCln.onBoard[j].TakeDamage(3 + owner.SpellDamage);
                    oppCln.onBoard[i].TakeDamage(3 + owner.SpellDamage);

                    result.Add((new MasterBoardContainer(cln) { action = targets }, 1));
                }
            }
            return new RandomSubBoardContainer(result, b, "Play " + this);
        });
    }

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
    }
}