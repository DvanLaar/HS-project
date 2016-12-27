using System;

class ArchmageAntonidas : Minion
{
    public ArchmageAntonidas() : base(7, 5, 7)
    {

    }

    public override void OnSummon()
    {
        base.OnSummon();
        owner.SpellCast += this.onSpellCast;
        toSilence.Add(() => owner.SpellCast -= this.onSpellCast);
    }

    private void onSpellCast(Spell casted)
    {
        if (owner.hand.Count >= 10)
            return;
        Fireball fireball = new Fireball(owner, opponent);
        owner.hand.Add(fireball);
    }
}

class ManaWyrm : Minion
{
    public ManaWyrm() : base(1, 1, 3)
    {

    }

    public override void OnSummon()
    {
        base.OnSummon();
        owner.SpellCast += this.onSpellCast;
        toSilence.Add(() => owner.SpellCast -= this.onSpellCast);
    }

    private void onSpellCast(Spell casted)
    {
        this.attack += 1;
    }
}

class SorcerersApprentice : Minion
{
    public SorcerersApprentice() : base(2, 3, 2)
    {
    }

    public override void OnSummon()
    {
        base.OnSummon();
        foreach (Card card in owner.hand)
        {
            if (card.GetType().IsSubclassOf(typeof(Spell)))
                card.mana -= 1;
        }
        owner.CardDrawn += this.onCardDrawn;
        toSilence.Add(() => { owner.CardDrawn -= this.onCardDrawn; foreach (Card card in owner.hand) if (card.GetType().IsSubclassOf(typeof(Spell))) card.mana++; });
    }

    private void onCardDrawn(Card card)
    {
        if (card.GetType().IsSubclassOf(typeof(Spell)))
            card.mana -= 1;
    }
}