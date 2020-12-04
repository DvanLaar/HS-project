using System;
using System.Collections.Generic;

class Execute : Spell
{
    public Execute() : base(2)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> result = new List<MasterBoardContainer>();
            Hero opponent = owner.id == b.me.id ? b.opp : b.me;
            foreach (Minion m in opponent.onBoard)
            {
                if (!m.Damaged)
                    continue;

                Board clone = b.Clone();
                Hero opp = owner.id == clone.me.id ? clone.opp : clone.me;
                opp.onBoard[opponent.onBoard.IndexOf(m)].StartDestroy();
                result.Add(new MasterBoardContainer(clone) { action = "Target " + m });
            }

            if (result.Count == 0)
                return null;

            return new ChoiceSubBoardContainer(result, b, "Play " + this);
        });
    }

    public override bool CanPlay(Board b)
    {
        return base.CanPlay(b) && !(b.me.id == owner.id ? b.opp : b.me).onBoard.TrueForAll((m) => !m.Damaged);
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero me = b.me.id == owner.id ? b.me : b.opp;
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