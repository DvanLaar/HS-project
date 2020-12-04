using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Hunter.Spells
{
    internal class ArcaneShot : Spell
    {
        public ArcaneShot() : base(1)
        {
            SetSpell(b => DealDamage(2 + Owner.SpellDamage, b));
        }
    }
}