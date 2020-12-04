using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Shaman.Tokens
{
    internal class WrathOfAirTotem : Minion
    {
        public WrathOfAirTotem() : base(1, 0, 2)
        {
            Totem = true;
            Owner.Summon += m =>
            {
                if (m != this) return;
                Owner.SpellDamage++; m.Transform += () => Owner.SpellDamage--; m.Destroy += () => Owner.SpellDamage--;
            };
        }

        public override string ToString()
        {
            return "Wrath of Air Totem";
        }
    }
}