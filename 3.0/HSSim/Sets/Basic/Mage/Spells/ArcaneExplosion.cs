using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class ArcaneExplosion : Spell
    {
        public ArcaneExplosion() : base(2)
        {
            SetSpell(b =>
            {
                var clone = b.Clone();
                var opp = clone.Me.Id == Owner.Id ? clone.Opp : clone.Me;
                foreach (var minion in opp.OnBoard)
                {
                    minion.TakeDamage(1 + Owner.SpellDamage);
                }
                return new SingleSubBoardContainer(clone, b, "Play Arcane Explosion");
            });
        }
    }
}