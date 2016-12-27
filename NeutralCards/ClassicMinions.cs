using System;
using System.Collections.Generic;

class Abomination : Minion
{
    public Abomination()
        : base(5, 4, 4, Attribute.Taunt)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        this.Deathrattle += deathrattle;
    }

    void deathrattle()
    {
        opponent.Damage(2);
        owner.Damage(2);
        foreach (Minion m in opponent.onBoard)
            m.Damage(2);
        foreach (Minion m in owner.onBoard)
            m.Damage(2);
    }
}

class AzureDrake : Minion
{
    public AzureDrake() : base(5, 4, 4)
    {
        this.Battlecry += battlecry;
    }

    void battlecry()
    {
        owner.DrawCards(1);
    }

    public override void OnPlay()
    {
        base.OnPlay();
        owner.spellDamage++;
        this.toSilence.Add(() => owner.spellDamage--);
    }
}

class CrazedAlchemist : Minion
{
    public CrazedAlchemist()
        : base(2, 2, 2)
    {
        this.Battlecry += battlecry;
    }

    void battlecry()
    {
        Minion minion = owner.getMinionTarget();
        int temp = minion.attack;
        minion.attack = minion.health;
        minion.health = temp;
        minion.maxHealth = temp;
    }
}

class StampedingKodo : Minion
{
    public StampedingKodo() : base(5, 3, 5)
    {
        this.Battlecry += battlecry;
    }

    void battlecry()
    {
        List<Minion> targets = new List<Minion>();
        foreach (Minion potentialTarget in opponent.onBoard)
        {
            if (potentialTarget.attack <= 2)
                targets.Add(potentialTarget);
        }
        if (targets.Count == 0)
            return;
        Random random = new Random();
        targets[random.Next(0, targets.Count)].Destroy(); //DESTROY
    }
}