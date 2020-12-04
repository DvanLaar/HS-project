using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class GurubashiBerserker : Minion
    {
        public GurubashiBerserker() : base(5, 2, 7)
        {
            OnDamaged += () =>
            {
                AlterAttack(3);
            };
        }
    }
}