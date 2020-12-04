using System;
using System.Collections.Generic;

class ArcaneIntellect : Spell
{
    public ArcaneIntellect() : base(3)
    {
        SetSpell((b) =>
        {
            Board b2 = b.Clone();
            return owner.DrawTwoCards(b2);
        });
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero h = b.me.id == owner.id ? b.me : b.opp;
        int hnd = 0;
        if (h.hand.Count == 10)
            hnd = 0;
        else
            hnd = 1;

        return h.CalcValue(cards: hnd, deck: -2) - h.CalcValue();
    }
}