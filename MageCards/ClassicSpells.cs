using System;

class MirrorEntity : Secret
{
    public MirrorEntity() : base(3)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        opponent.PlayMinion += this.Triggered;
    }

    public override bool Resolve(object thingyobject)
    {
        Minion toCopy = (Minion)thingyobject;
        return (Effects.Summon(owner.onBoard, toCopy));
    }

    public override void RemoveFromPlay()
    {
        base.RemoveFromPlay();
        this.opponent.PlayMinion -= this.Triggered;
    }
}

class Pyroblast : Spell
{
    public Pyroblast() : base(10)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        IDamagable target = owner.getTarget();
        target.Damage(owner.spellDamage + 10);
    }
}