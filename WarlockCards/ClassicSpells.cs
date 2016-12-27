using System;

class Shadowflame : Spell
{
    public Shadowflame() : base(4)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        Minion minion = owner.getFriendlyMinionTarget();
        int damage = minion.attack + owner.spellDamage;
        foreach (IDamagable target in opponent.onBoard)
            target.Damage(damage);
    }
}