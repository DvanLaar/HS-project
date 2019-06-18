abstract class FriendlyMinionAuraMinion : MinionAuraMinion
{
    public FriendlyMinionAuraMinion(int mana, int attack, int health) : base(mana, attack, health)
    {

    }

    public override void AddAura()
    {
        base.AddAura();
        foreach (Minion m in owner.onBoard)
            Aura(m);
        owner.Summon += Aura;
    }

    public override void RemoveAura()
    {
        base.RemoveAura();
        foreach (Minion m in owner.onBoard)
            AuraInvert(m);
        owner.Summon -= Aura;
    }
}