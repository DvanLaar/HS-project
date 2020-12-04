class WickedKnife : Weapon
{
    public WickedKnife() : base(1, 1, 2)
    {
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        return owner.CalcValue(cards: -1) - owner.CalcValue();
    }
}