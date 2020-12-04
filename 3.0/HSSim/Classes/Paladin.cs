using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Sets.Basic.Paladin.Tokens;

namespace HSSim.Classes
{
    internal class Paladin : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        public Paladin()
        {
            SetHeroPower();
        }

        public Paladin(bool id, bool nw)
        {
            SetHeroPower();
        }

        private void SetHeroPower()
        {
            HeroPower = (b =>
            {
                if (Mana < 2 || HeroPowerUsed || OnBoard.Count >= 7)
                    return null;

                var clone = b.Clone();
                var me = Id == clone.Me.Id ? clone.Me : clone.Opp;
                me.StartSummon(new SilverHandRecruit());
                me.Mana -= 2;
                return new SingleSubBoardContainer(clone, b, "Use Hero Power");
            });
        }
    }
}