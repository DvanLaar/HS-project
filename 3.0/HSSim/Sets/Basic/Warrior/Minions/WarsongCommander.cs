class WarsongCommander : MinionAuraMinion
{
    public WarsongCommander() : base(3, 2, 3)
    {
    }

    protected override void AuraInvert(Minion m)
    {
        if (!m.Charge)
            return;

        m.Attack -= 1;
    }

    protected override void Aura(Minion m)
    {
        if (!m.Charge)
            return;

        m.Attack += 1;
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        int buffs = 5;
        Hero h = b.me.id == owner.id ? b.me : b.opp;
        if (h.onBoard.Count == 0)
            buffs += 2 + h.maxMana;
        foreach (Minion m in h.onBoard)
        {
            if (m.Charge)
                buffs += 1;
        }
        return h.CalcValue(cards: -1, minions: buffs) - h.CalcValue();
    }
}