using System;
using System.Collections.Generic;

class ArcaneExplosion : Spell
{
    public ArcaneExplosion() : base(2)
    {
        SetSpell((b) =>
        {
            Board clone = b.Clone();
            Hero Opp = clone.me.id == owner.id ? clone.opp : clone.me;
            Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
            for (int i = 0; i < Opp.onBoard.Count; i++)
            {
                Opp.onBoard[i].TakeDamage(1 + owner.SpellDamage);
            }
            return new SingleSubBoardContainer(clone, b, "Play Arcane Explosion");
        });
    }
}