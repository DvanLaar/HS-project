using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Sets.Basic.Rogue.Tokens;

namespace HSSim.Classes
{
    internal class Rogue : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        public Rogue()
        {
            SetHeroPower();
        }

        public Rogue(bool id, bool nw) : base(id, nw)
        {
            SetHeroPower();
        }

        private void SetHeroPower()
        {
            HeroPower = (b =>
            {
                if (Mana < 2 || HeroPowerUsed)
                    return null;

                var clone = b.Clone();
                var me = Id == clone.Me.Id ? clone.Me : clone.Opp;
                me.Mana -= 2;
                me.EquipWeapon(new WickedKnife());
                return new SingleSubBoardContainer(clone, b, "Use Hero Power");
            });
        }
    }
}