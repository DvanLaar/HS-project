using HSSim.Abstract_Cards.Minions;
using HSSim.Sets.Basic.Neutral.Tokens;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class RazorfenHunter : BattlecryMinion
    {
        public RazorfenHunter() : base(3, 2, 3)
        {
            SetBattlecry(b =>
            {
                var clone = b.Clone();
                (clone.Me.Id == Owner.Id ? clone.Me : clone.Opp).StartSummon(new Boar { Owner = Owner });
                return new SingleSubBoardContainer(clone, b, "Play " + this);
            });
        }
    }
}