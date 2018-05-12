using System;
using System.Collections.Generic;

class ArcaneIntellect : Spell
{
    public ArcaneIntellect() : base(3)
    {
        SetSpell((b) =>
        {
            Board b2 = b.Clone();
            (owner.id == b2.me.id ? b2.me : b2.opp).Mana -= cost;
            return owner.DrawTwoCards(b2);
        });
    }
}