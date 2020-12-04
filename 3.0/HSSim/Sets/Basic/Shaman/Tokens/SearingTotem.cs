using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Shaman.Tokens
{
    internal class SearingTotem : Minion
    {
        public SearingTotem() : base(1, 1, 1)
        {
            Totem = true;
        }
    }
}