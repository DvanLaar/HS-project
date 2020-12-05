using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class Fireball : Spell
    {
        public Fireball() : base(4)
        {
            SetSpell(b => DealDamage(6 + Owner.SpellDamage, b));
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            var opp = b.Me.Id == Owner.Id ? b.Opp : b.Me;
            double max = -100, val;
            var damage = 6 + Owner.SpellDamage;

            foreach (var m in Owner.OnBoard)
            {
                if (m.Health <= damage)
                {
                    if (Owner.OnBoard.Count == 1)
                    {
                        val = Owner.CalcValue(cards: -1, minions: 0 - m.Health - m.Attack - 2 - Owner.MaxMana) - Owner.CalcValue();
                        if (val > max)
                            max = val;
                    }
                    else
                    {
                        val = Owner.CalcValue(cards: -1, minions: 0 - m.Health - m.Attack) - Owner.CalcValue();
                        if (val > max)
                            max = val;
                    }
                }
                else
                {
                    val = Owner.CalcValue(cards: -1, minions: 0 - damage) - Owner.CalcValue();
                    if (val > max)
                        max = val;
                }
            }

            foreach (var m in opp.OnBoard)
            {
                if (m.Health <= damage)
                {
                    if (opp.OnBoard.Count == 1)
                    {
                        val = opp.CalcValue() + Owner.CalcValue(cards: -1) - opp.CalcValue(minions: -2 - m.Health - m.Attack - opp.MaxMana) - Owner.CalcValue();
                        if (val > max)
                            max = val;
                    }
                    else
                    {
                        val = opp.CalcValue() + Owner.CalcValue(cards: -1) - opp.CalcValue(minions: 0 - m.Health - m.Attack) - Owner.CalcValue();
                        if (val > max)
                            max = val;
                    }
                }
                else
                {
                    val = opp.CalcValue() + Owner.CalcValue(cards: -1) - opp.CalcValue(minions: 0 - damage) - Owner.CalcValue();
                    if (val > max)
                        max = val;
                }
            }

            val = Owner.CalcValue(0 - damage, -1) - Owner.CalcValue();
            if (val > max)
                max = val;
            val = Owner.CalcValue(cards: -1) + opp.CalcValue() - Owner.CalcValue() - opp.CalcValue(0 - damage);
            if (val > max)
                max = val;

            return max;
        }
    }
}