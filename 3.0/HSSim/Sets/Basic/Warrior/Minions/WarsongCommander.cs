

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
}