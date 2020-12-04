using System;
using System.Collections.Generic;

class Polymorph : Spell
{
    public Polymorph() : base(4)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> result = new List<MasterBoardContainer>();
            Hero opponent = b.me.id == owner.id ? b.opp : b.me;
            foreach (Minion m in opponent.onBoard)
            {
                Board clone = b.Clone();
                Hero opp = clone.me.id == opponent.id ? clone.me : clone.opp;
                Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
                Minion sheep = new Sheep();
                sheep.SetOwner(opp);
                opp.onBoard[opponent.onBoard.IndexOf(m)].StartTransform();
                opp.onBoard[opponent.onBoard.IndexOf(m)] = sheep;
                result.Add(new MasterBoardContainer(clone) { action = "Transform " + m });
            }

            foreach (Minion m in owner.onBoard)
            {
                Board clone = b.Clone();
                Hero opp = clone.me.id == opponent.id ? clone.me : clone.opp;
                Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
                me.Mana -= cost;
                Minion sheep = new Sheep();
                sheep.SetOwner(me);
                me.onBoard[owner.onBoard.IndexOf(m)] = sheep;
                result.Add(new MasterBoardContainer(clone) { action = "Transform " + m });
            }
            return new ChoiceSubBoardContainer(result, b, "Play Polymorph");
        });
    }
    public override bool CanPlay(Board b)
    {
        if (!base.CanPlay(b))
            return false;

        return b.me.onBoard.Count > 0 || b.opp.onBoard.Count > 0;
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        Hero opp = b.me.id == owner.id ? b.opp : b.me;
        double max = -100;
        foreach (Minion m in owner.onBoard)
        {
            double val = owner.CalcValue(cards: -1, minions: 2 - m.Health - m.Attack) - owner.CalcValue();
            if (val > max)
                max = val;
        }
        foreach (Minion m in opp.onBoard)
        {
            double val = opp.CalcValue() + owner.CalcValue(cards: -1) - opp.CalcValue(minions: 2 - m.Health - m.Attack) - owner.CalcValue();
            if (val > max)
                max = val;
        }
        return max;
    }
}