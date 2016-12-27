using System;

class TheCoin : Spell
{
    public TheCoin() : base(0)
    {
    }

    public override void OnPlay()
    {
        owner.ExtraAvailableMana(1);
    }
}