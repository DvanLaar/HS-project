using HSSim.Abstract_Cards.Minions;
using HSSim.Sets.Basic.Neutral.Tokens;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class MurlocTidehunter : BattlecryMinion
    {
        public MurlocTidehunter() : base(2, 2, 1)
        {
            SetBattlecry(b =>
            {
                var cln = b.Clone();
                var me = cln.Me.Id == Owner.Id ? cln.Me : cln.Opp;
                me.StartSummon(new MurlocScout());
                return new SingleSubBoardContainer(cln, b, "Play " + this);
            });
        }
    }
}