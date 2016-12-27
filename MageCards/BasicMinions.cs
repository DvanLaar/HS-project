using System;

class WaterElemental : Minion
{
    public WaterElemental() : base(4, 3, 6)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        this.Attack += OnAttack;
        this.Attacked += OnAttack;
        toSilence.Add(() => this.Attack -= OnAttack);
        toSilence.Add(() => this.Attacked -= OnAttack);
    }

    private void OnAttack(AttackEventArgs aea)
    {
        if (aea.attacker == this)
            aea.defender.Freeze();
        else if (aea.defender == this)
            aea.attacker.Freeze();
    }
}

class MirrorImageToken : Minion
{
    public MirrorImageToken() : base(0, 0, 2, Attribute.Taunt)
    {
    }
}

class Sheep : Minion
{
    public Sheep() : base(1, 1, 1)
    {
    }
}