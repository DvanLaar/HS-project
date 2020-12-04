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

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        Hero opp = b.me.id == owner.id ? b.opp : b.me;
        double max = -100, val;
        int damage = 6 + owner.SpellDamage;

        foreach (Minion m in owner.onBoard)
        {
            if (m.Health <= damage)
            {
                if (owner.onBoard.Count == 1)
                {
                    val = owner.CalcValue(cards: -1, minions: 0 - m.Health - m.Attack - 2 - owner.maxMana) - owner.CalcValue();
                    if (val > max)
                        max = val;
                }
                else
                {
                    val = owner.CalcValue(cards: -1, minions: 0 - m.Health - m.Attack) - owner.CalcValue();
                    if (val > max)
                        max = val;
                }
            }
            else
            {
                val = owner.CalcValue(cards: -1, minions: 0 - damage) - owner.CalcValue();
                if (val > max)
                    max = val;
            }
        }

        foreach (Minion m in opp.onBoard)
        {
            if (m.Health <= damage)
            {
                if (opp.onBoard.Count == 1)
                {
                    val = opp.CalcValue() + owner.CalcValue(cards: -1) - opp.CalcValue(minions: -2 - m.Health - m.Attack - opp.maxMana) - owner.CalcValue();
                    if (val > max)
                        max = val;
                }
                else
                {
                    val = opp.CalcValue() + owner.CalcValue(cards: -1) - opp.CalcValue(minions: 0 - m.Health - m.Attack) - owner.CalcValue();
                    if (val > max)
                        max = val;
                }
            }
            else
            {
                val = opp.CalcValue() + owner.CalcValue(cards: -1) - opp.CalcValue(minions: 0 - damage) - owner.CalcValue();
                if (val > max)
                    max = val;
            }
        }

        val = owner.CalcValue(health: 0 - damage, cards: -1) - owner.CalcValue();
        if (val > max)
            max = val;
        val = owner.CalcValue(cards: -1) + opp.CalcValue() - owner.CalcValue() - opp.CalcValue(health: 0 - damage);
        if (val > max)
            max = val;

        return max;
    }
}