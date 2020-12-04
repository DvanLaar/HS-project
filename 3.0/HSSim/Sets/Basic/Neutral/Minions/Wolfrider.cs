using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class Wolfrider : Minion
    {
        public Wolfrider() : base(3, 3, 1)
        {
            Charge = true;
        }
    }
}