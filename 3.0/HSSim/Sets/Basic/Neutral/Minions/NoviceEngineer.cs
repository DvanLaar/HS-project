using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class NoviceEngineer : BattlecryMinion
    {
        public NoviceEngineer() : base(2, 1, 1)
        {
            SetBattlecry(b => Owner.DrawOneCard(b));
        }
    }
}