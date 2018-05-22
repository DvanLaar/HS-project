using System;

class StormwindChampion : FriendlyMinionAuraMinion
{
    public StormwindChampion() : base(7, 6, 6)
    {
    }

    protected override void Aura(Minion m)
    {
        if (m == this)
            return;
        m.AddHealth(1);
        m.AlterAttack(1);
    }

    protected override void AuraInvert(Minion m)
    {
        if (m == this)
            return;
        m.ReduceHealth(1);
        m.AlterAttack(-1);
    }
}
