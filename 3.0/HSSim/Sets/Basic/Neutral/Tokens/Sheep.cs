using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Tokens
{
    internal class Sheep : Minion
    {
        public Sheep() : base(1, 1, 1)
        {
            Beast = true;
        }
    }
}