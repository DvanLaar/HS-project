using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Shaman.Tokens
{
    internal class StoneclawTotem : Minion
    {
        public StoneclawTotem() : base(1, 0, 2)
        {
            Taunt = true;
            Totem = true;
        }
    }
}