class TimberWolf : FriendlyMinionAuraMinion
{
    public TimberWolf() : base(1, 1, 1)
    {
        Beast = true;
    }

    protected override void Aura(Minion m)
    {
        if (m.Beast && m != this)
            m.AlterAttack(1);
    }

    protected override void AuraInvert(Minion m)
    {
        if (m.Beast && m != this)
            m.AlterAttack(-1);
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero me = owner.id == b.me.id ? b.me : b.opp;
        int buffs = 2;
        foreach (Minion m in me.onBoard)
        {
            if (m.Beast)
                buffs += 1;
        }
        if (me.onBoard.Count == 0)
        {
            buffs += 2 + me.maxMana;
        }
        return me.CalcValue() - me.CalcValue(minions: buffs);
    }
}