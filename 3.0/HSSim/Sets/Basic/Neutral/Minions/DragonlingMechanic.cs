using HSSim.Abstract_Cards.Minions;
using HSSim.Sets.Basic.Neutral.Tokens;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class DragonlingMechanic : BattlecryMinion
    {
        public DragonlingMechanic() : base(4, 2, 4)
        {
            SetBattlecry(b =>
            {
                var clone = b.Clone();
                var me = Owner.Id == clone.Me.Id ? clone.Me : clone.Opp;
                me.StartSummon(new MechanicalDragonling());
                return new SingleSubBoardContainer(clone, b, "Play " + this);
            });
        }
    }
}