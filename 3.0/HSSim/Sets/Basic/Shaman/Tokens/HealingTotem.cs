using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Shaman.Tokens
{
    internal class HealingTotem : EndOfTurnMinion
    {
        public HealingTotem() : base(1, 0, 2)
        {
            Totem = true;
        }

        protected override SubBoardContainer EoTEffect(Board b)
        {
            var cln = b.Clone();
            var own = Owner.Id == cln.Me.Id ? cln.Me : cln.Opp;
            foreach (var m in own.OnBoard)
                m.Health += 1;
            return new SingleSubBoardContainer(cln, b, this + " heals");
        }
    }
}