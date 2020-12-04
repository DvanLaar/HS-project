using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Tokens
{
    internal class Boar : Minion
    {
        public Boar() : base(1, 1, 1)
        {
            Beast = true;
        }
    }
}