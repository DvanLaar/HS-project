using System;

class RaidLeader : FriendlyMinionAuraMinion
{
    public RaidLeader() : base(3, 2, 2)
    {
    }

    public override void AddAura()
    {
        base.AddAura();
        foreach (Minion m in owner.onBoard)
        {
            if (m == this)
                continue;
            m.AlterAttack(1);
        }
        owner.Summon += Aura;
    }

    public override void RemoveAura()
    {
        base.RemoveAura();
        foreach (Minion m in owner.onBoard)
        {
            if (m == this)
                continue;
            m.AlterAttack(-1);
        }
        owner.Summon -= Aura;
    }

    protected override void Aura(Minion m)
    {
        if (m == this)
            return;
        m.AlterAttack(1);
    }

    protected override void AuraInvert(Minion m)
    {
        if (m == this)
            return;
        m.AlterAttack(-1);
    }
}