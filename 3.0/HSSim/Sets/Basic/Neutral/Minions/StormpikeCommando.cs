using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class StormpikeCommando : BattlecryMinion
    {
        public StormpikeCommando() : base(5, 4, 2)
        {
            SetBattlecry(b => DealDamage(2, b));
        }
    }
}