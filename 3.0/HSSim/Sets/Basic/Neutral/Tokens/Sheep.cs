using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Tokens
{
    internal class Sheep : Minion
    {
        public Sheep() : base(1, 1, 1)
        {
            Beast = true;
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            var min = 2;
            if (Owner.OnBoard.Count == 0)
                min += 2 + Owner.MaxMana;
            return Owner.CalcValue(cards: -1, minions: min);
        }
    }
}