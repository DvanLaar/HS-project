using System;
using System.Collections.Generic;

class LostInTheJungle : Spell
{
    public LostInTheJungle() : base(1)
    {
        SetSpell((b) =>
        {
            Board cln = b.Clone();
            Hero me = owner.id == cln.me.id ? cln.me : cln.opp;
            me.StartSummon(new SilverHandRecruit());
            me.StartSummon(new SilverHandRecruit());
            me.Mana -= cost;
            return new SingleSubBoardContainer(cln, b, "Play " + this);
        });
    }

    public override bool CanPlay(Board b)
    {
        return base.CanPlay(b) && owner.onBoard.Count < 7;
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero me = b.me.id == owner.id ? b.me : b.opp;
        int summoned = Math.Max(7 - me.onBoard.Count, 2);
        if (me.onBoard.Count == 0)
        {
            return me.CalcValue(cards: -1, minions: 6 + me.maxMana) - me.CalcValue();
        }
        else
        {
            return me.CalcValue(cards: -1, minions: summoned * 2) - me.CalcValue();
        }
    }
}