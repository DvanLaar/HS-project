﻿class Coin : Spell
{
    public Coin() : base(0)
    {
        SetSpell((b) =>
        {
            Board clone = b.Clone();
            Hero ownerClone = b.me.id == owner.id ? clone.me : clone.opp;
            ownerClone.Mana -= cost;
            ownerClone.Mana += 1;
            return new SingleSubBoardContainer(clone, b, "Play The Coin");
        });
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        return owner.CalcValue(cards: -1) - owner.CalcValue();
    }
}