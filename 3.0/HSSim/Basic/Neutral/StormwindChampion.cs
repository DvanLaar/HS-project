using System;

class StormwindChampion : MinionAuraMinion
{
    public StormwindChampion() : base(7, 6, 6)
    {
    }

    public override void AddAura()
    {
        base.AddAura();
        foreach(Minion m in owner.onBoard)
        {
            if (m == this)
                continue;
            Aura(m);
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
            m.ReduceHealth(1);
            m.AlterAttack(-1);
        }
        owner.Summon -= Aura;
    }

    protected override void Aura(Minion m)
    {
        if (m == this)
            return;
        m.AddHealth(1);
        m.AlterAttack(1);
    }
}
