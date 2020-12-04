using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Classes
{
    internal class Warlock : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        public Warlock()
        {
            SetHeroPower();
        }

        public Warlock(bool id, bool nw) : base(id, nw)
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
                var sbc = me.DrawOneCard(clone);
                foreach (var mbc in sbc.Children)
                    (mbc.Board.Me.Id == Id ? mbc.Board.Me : mbc.Board.Opp).TakeDamage(2);
                return sbc;
            });
        }
    }
}