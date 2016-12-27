using System;

class ForgottenTorch : Spell
{
    public ForgottenTorch() : base(3)
    {

    }

    public override void OnPlay()
    {
        IDamagable target = owner.getTarget();
        target.Damage(owner.spellDamage + 3);
        RoaringTorch torch = new RoaringTorch();
        torch.owner = owner;
        torch.opponent = opponent;
        owner.deck.Add(torch);
    }
}

class RoaringTorch : Spell
{
    public RoaringTorch() : base(3)
    {

    }

    public override void OnPlay()
    {
        IDamagable target = owner.getTarget();
        target.Damage(owner.spellDamage + 6);
    }
}