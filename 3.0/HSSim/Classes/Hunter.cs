using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Classes
{
    internal class Hunter : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        protected Hunter()
        {
            SetHeropower();
        }

        protected Hunter(bool id, bool nw) : base(id, nw)
        {
            SetHeropower();
        }

        private void SetHeropower()
        {
            HeroPower = b =>
            {
                if (ManaProtected < 2 || HeroPowerUsed)
                    return null;

                var cln = b.Clone();
                (cln.Me.Id == Id ? cln.Opp : cln.Me).TakeDamage(2);
                (cln.Me.Id == Id ? cln.Me : cln.Opp).Mana -= 2;
                (cln.Me.Id == Id ? cln.Me : cln.Opp).HeroPowerUsed = true;
                return new SingleSubBoardContainer(cln, b, "Use Hero Power");
            };
        }
    }
}