

abstract class AuraMinion : Minion
{
    protected bool auraActive;
    public AuraMinion(int mana, int attack, int health) : base(mana, attack, health)
    {
        auraActive = false;
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
}

abstract class MinionAuraMinion : AuraMinion
{
    public MinionAuraMinion(int mana, int attack, int health) : base(mana, attack, health)
    {

    }

    protected abstract void Aura(Minion m);
    public override void SetOwner(Hero newOwner)
    {
        if (auraActive)
        {
            newOwner.Summon += Aura;
        }
        base.SetOwner(newOwner);
    }
}