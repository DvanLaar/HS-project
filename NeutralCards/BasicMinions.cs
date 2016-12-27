using System;

class AcidicSwampOoze : Minion
{
    public AcidicSwampOoze() : base (2, 3, 2)
    {
        this.Battlecry += battlecry;
    }

    void battlecry()
    {
        opponent.weapon.Destroy();
    }
}

class Archmage : Minion
{
    public Archmage() : base(6, 4, 7)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        owner.spellDamage++;
        this.toSilence.Add(() => owner.spellDamage--);
    }
}

class BloodfenRaptor : Minion
{
    public BloodfenRaptor() : base(2, 3, 2)
    {
    }
}

class BoulderfistOgre : Minion
{
    public BoulderfistOgre() : base(6, 6, 7)
    {
    }
}

class MurlocRaider : Minion
{
    public MurlocRaider()
        : base(1, 2, 1)
    {
    }
}

class Nightblade : Minion
{
    public Nightblade() : base(5, 4, 4)
    {
        this.Battlecry += battlecry;
    }

    private void battlecry()
    {
        opponent.Damage(4);
    }
}

class NoviceEngineer : Minion
{
    public NoviceEngineer() : base(2, 1, 1)
    {
        this.Battlecry += battlecry;
    }

    private void battlecry()
    {
        owner.DrawCards(1);
    }
}

class OasisSnapjaw : Minion
{
    public OasisSnapjaw() : base(4, 2, 7)
    {
    }
}

class RiverCrocolisk : Minion
{
    public RiverCrocolisk() : base(2, 3, 2)
    {
    }
}

class SenjinShieldmasta : Minion
{
    public SenjinShieldmasta() : base(4, 3, 5, Attribute.Taunt)
    {
    }
}

class Wolfrider : Minion
{
    public Wolfrider() : base(3, 3, 1, Attribute.Charge)
    {
    }
}

class RaidLeader : Minion
{
    public RaidLeader() : base(3, 2, 2)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        foreach (Minion m in owner.onBoard)
            m.attack++;
        owner.SummonMinion += ownerOnMinionSummoned;

        this.toSilence.Add(() => { foreach (Minion m in owner.onBoard) m.attack--; });
    }

    private void ownerOnMinionSummoned(Minion m)
    {
        m.attack++;
    }
}