using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class StonetuskBoar : Minion
    {
        public StonetuskBoar() : base(1, 1, 1)
        {
            Charge = true;
            Beast = true;
        }
    }
}