using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class ArcaneIntellect : Spell
    {
        public ArcaneIntellect() : base(3)
        {
            SetSpell(b =>
            {
                var b2 = b.Clone();
                return Owner.DrawTwoCards(b2);
            });
        }

        public override double DeltaBoardValue(Board b)
        {
            var h = b.Me.Id == Owner.Id ? b.Me : b.Opp;
            var hnd = h.Hand.Count == 10 ? 0 : 1;

            return h.CalcValue(cards: hnd, deck: -2) - h.CalcValue();
        }
    }
}