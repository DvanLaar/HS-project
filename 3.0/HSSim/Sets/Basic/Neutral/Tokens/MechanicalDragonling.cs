using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Tokens
{
    internal class MechanicalDragonling : Minion
    {
        public MechanicalDragonling() : base(1, 2, 1)
        {
            Mech = true;
        }
    }
}