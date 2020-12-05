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

        public override double DeltaBoardValue(Board b)
        {
            var opp = b.Me.Id == Owner.Id ? b.Opp : b.Me;
            var me = b.Me.Id == Owner.Id ? b.Me : b.Opp;
            var damage = 1 + Owner.SpellDamage;
            var minionIncrease = 0;
            var allDead = true;

            foreach (var m in opp.OnBoard)
            {
                if (m.Health <= damage)
                {
                    minionIncrease += m.Health + m.Attack;
                }
                else
                {
                    minionIncrease += damage;
                    allDead = false;
                }
            }

            if (allDead)
                minionIncrease += 2 + opp.MaxMana;

            return opp.CalcValue() + me.CalcValue(cards: -1) - opp.CalcValue(minions: -1 * minionIncrease) - me.CalcValue();
        }
    }
}