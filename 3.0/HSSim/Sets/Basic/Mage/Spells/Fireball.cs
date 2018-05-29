using System;
using System.Collections.Generic;

class Fireball : Spell
{
    public Fireball() : base(4)
    {
        SetSpell((b) =>
        {
            return DealDamage(6 + owner.SpellDamage, b);
        });
    }
}