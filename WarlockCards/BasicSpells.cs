using System;

class MortalCoil : Spell
{
    public MortalCoil() : base(1)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        IDamagable minion = owner.getMinionTarget();
        minion.Damage(owner.spellDamage + 1);
        if (minion.health <= 0)
            owner.DrawCards(1);
    }
}

class ShadowBolt : Spell
{
    public ShadowBolt() : base(3)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        Minion minion = owner.getMinionTarget();
        minion.Damage(owner.spellDamage + 4);
    }
}