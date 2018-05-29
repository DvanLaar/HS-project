

abstract class AuraMinion : Minion
{
    protected bool auraActive;
    public AuraMinion(int mana, int attack, int health) : base(mana, attack, health)
    {
        auraActive = false;
        Transform += RemoveAura;
    }

    public virtual void AddAura()
    {
        auraActive = true;
    }
    public virtual void RemoveAura()
    {
        auraActive = false;
    }
    public override Card Clone()
    {
        AuraMinion am = (AuraMinion)base.Clone();
        am.auraActive = auraActive;
        return am;
    }

    public override int Health
    {
        get => curHealth; set
        {
            curHealth = value;
            if (curHealth <= 0)
            {
                owner.onBoard.Remove(this);
                RemoveAura();
            }
        }
    }

    public override void SetOwner(Hero owner)
    {
        base.SetOwner(owner);
        owner.Summon += (m) => { if (m == this) AddAura(); };
    }
}

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