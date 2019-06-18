abstract class MinionAuraMinion : AuraMinion
{
    public MinionAuraMinion(int mana, int attack, int health) : base(mana, attack, health)
    {
    }

    protected abstract void Aura(Minion m);
    protected abstract void AuraInvert(Minion m);
    public override void SetOwner(Hero newOwner)
    {
        if (auraActive)
        {
            newOwner.Summon += Aura;
        }
        base.SetOwner(newOwner);
    }
}