using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class Fireball : Spell
    {
        public Fireball() : base(4)
        {
            SetSpell(b => DealDamage(6 + Owner.SpellDamage, b));
        }
    }
}