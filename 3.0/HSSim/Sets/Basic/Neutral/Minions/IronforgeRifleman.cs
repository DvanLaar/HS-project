using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class IronforgeRifleman : BattlecryMinion
    {
        public IronforgeRifleman() : base(3, 2, 2)
        {
            SetBattlecry(b => DealDamage(1, b));
        }
    }
}