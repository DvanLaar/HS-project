using System;
using System.Collections.Generic;

class Deathcharger : Minion
{
    public Deathcharger() : base(1, 2, 3, Attribute.Charge)
    {
        this.Deathrattle += deathrattle;
    }

    void deathrattle()
    {
        owner.Damage(3);
    }
}

class Deathlord : Minion
{
    public Deathlord() : base(3, 2, 8)
    {
        this.Deathrattle += deathrattle;
    }

    void deathrattle()
    {
        if (opponent.onBoard.Count >= 7)
            return;
        List<Card> target = new List<Card>();
        foreach (Card c in opponent.deck)
        {
            if (!c.GetType().IsSubclassOf(typeof(Minion)))
                continue;
            target.Add(c);
        }
        if (target.Count == 0)
            return;
        Random random = new Random();
        Minion pullTarget = (Minion)target[random.Next(0, target.Count)];
        Effects.Summon(opponent.onBoard, pullTarget);
        opponent.deck.Remove(pullTarget);
    }
}

class HauntedCreeper : Minion
{
    public HauntedCreeper() : base(2, 1, 2)
    {
        this.Deathrattle += deathrattle;
    }

    void deathrattle()
    {
        Effects.Summon(owner.onBoard, new SpectralSpider());
        Effects.Summon(owner.onBoard, new SpectralSpider());
    }
}

class NerubarWeblord : Minion
{
    public NerubarWeblord() : base(2, 1, 4)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        foreach (Card c in owner.hand)
        {
            auraEffect(c);
        }
        foreach (Card c in opponent.hand)
        {
            auraEffect(c);
        }
        owner.CardDrawn += auraEffect;
        opponent.CardDrawn += auraEffect;

        this.toSilence.Add(() =>
        {
            foreach (Card c in owner.hand)
                if (c.GetType().IsSubclassOf(typeof(Minion)) && ((Minion)c).HasBattlecry())
                    c.mana -= 2;
            foreach (Card c in opponent.hand)
                if (c.GetType().IsSubclassOf(typeof(Minion)) && ((Minion)c).HasBattlecry())
                    c.mana -= 2;
        });
    }

    private void auraEffect(Card c)
    {
        if (!c.GetType().IsSubclassOf(typeof(Minion)))
            return;
        Minion m = (Minion)c;
        if (m.HasBattlecry())
            m.mana += 2;
    }
}

class Nerubian : Minion
{
    public Nerubian() : base(4, 4, 4)
    {
    }
}

class NerubianAnub : Minion
{
    public NerubianAnub() : base(2, 3, 1)
    {
    }
}

class NerubianEgg : Minion
{
    public NerubianEgg() : base(2, 0, 2)
    {
        this.Deathrattle += deathrattle;
    }

    void deathrattle()
    {
        Effects.Summon(owner.onBoard, new Nerubian());
    }
}

class ShadeOfNaxxramas : Minion
{
    public ShadeOfNaxxramas() : base(3, 2, 2, Attribute.Stealth)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        owner.EndTurn += OnEndOfTurn;
        this.toSilence.Add(() => owner.EndTurn -= OnEndOfTurn);
    }

    public void OnEndOfTurn()
    {
        this.attack++;
        this.maxHealth++;
        this.health++;
    }
}

class SpectralSpider : Minion
{
    public SpectralSpider() : base(1, 1, 1)
    {
    }
}

class StoneskinGargoyle : Minion
{
    public StoneskinGargoyle() : base(3, 1, 4)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        owner.StartTurn += OnStartOfTurn;
    }

    public void OnStartOfTurn()
    {
        this.health = this.maxHealth;
    }
}