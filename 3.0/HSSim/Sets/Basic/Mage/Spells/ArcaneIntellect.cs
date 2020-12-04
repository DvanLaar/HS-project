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
    }
}