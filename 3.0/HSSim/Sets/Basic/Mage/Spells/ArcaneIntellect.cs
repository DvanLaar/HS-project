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
}