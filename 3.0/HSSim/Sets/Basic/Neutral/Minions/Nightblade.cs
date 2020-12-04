using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class Nightblade : BattlecryMinion 
    {
        public Nightblade() : base(5, 4, 4)
        {
            SetBattlecry(b =>
            {
                var clone = b.Clone();
                var oppClone = Owner.Id != clone.Me.Id ? clone.Me : clone.Opp;
                oppClone.TakeDamage(3);
                return new SingleSubBoardContainer(clone, b, "Play Nightblade");
            });
        }
    }
}