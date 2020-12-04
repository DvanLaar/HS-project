using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Classes
{
    internal class Warrior : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        protected Warrior()
        {
            SetHeropower();
        }

        protected Warrior(bool id, bool nw) : base(id, nw)
        {
            SetHeropower();
        }

        private void SetHeropower()
        {
            HeroPower = (b =>
            {
                if (ManaProtected < 2 || HeroPowerUsed)
                    return null;

                var cln = b.Clone();
                var me = cln.Me.Id == Id ? cln.Me : cln.Opp;
                me.Armor += 2;
                me.Mana -= 2;
                me.HeroPowerUsed = true;
                return new SingleSubBoardContainer(cln, b, "Use Hero Power");
            });
        }
    }
}