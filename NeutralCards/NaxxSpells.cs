using System;

class LocustSwarm : Spell
{
    public LocustSwarm() : base(7)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        foreach (IDamagable minion in opponent.onBoard)
        {
            minion.Damage(owner.spellDamage + 3);
        }

    }
}