using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Rogue.Tokens
{
    internal class WickedKnife : Weapon
    {
        public WickedKnife() : base(1, 1, 2)
        {
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            return Owner.CalcValue(cards: -1) - Owner.CalcValue();
        }
    }
}