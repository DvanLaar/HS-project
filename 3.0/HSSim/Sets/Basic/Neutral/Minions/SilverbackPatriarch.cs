using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class SilverbackPatriarch : Minion
    {
        public SilverbackPatriarch() : base(3, 1, 4)
        {
            Taunt = true;
            Beast = true;
        }
    }
}