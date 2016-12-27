using System;

class VelensChosen : Spell
{
    public VelensChosen() : base(3)
    {
    }

    public override void OnPlay()
    {
        Minion target = owner.getMinionTarget();
        target.attack += 2;
        target.health += 4;
        target.maxHealth += 4;
        target.spellDamage++;
        target.toSilence.Add(() => target.spellDamage--);
    }
}