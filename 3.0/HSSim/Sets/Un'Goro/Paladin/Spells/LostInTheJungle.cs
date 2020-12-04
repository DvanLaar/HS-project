using HSSim.Abstract_Cards;
using HSSim.Sets.Basic.Paladin.Tokens;

namespace HSSim.Sets.Paladin.Spells
{
    internal class LostInTheJungle : Spell
    {
        public LostInTheJungle() : base(1)
        {
            SetSpell(b =>
            {
                var cln = b.Clone();
                var me = Owner.Id == cln.Me.Id ? cln.Me : cln.Opp;
                me.StartSummon(new SilverHandRecruit());
                me.StartSummon(new SilverHandRecruit());
                me.Mana -= Cost;
                return new SingleSubBoardContainer(cln, b, "Play " + this);
            });
        }

        public override bool CanPlay(Board b)
        {
            return base.CanPlay(b) && Owner.OnBoard.Count < 7;
        }
    }
}