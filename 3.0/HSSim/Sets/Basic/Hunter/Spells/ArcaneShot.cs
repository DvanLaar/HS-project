using System;

class ArcaneShot : Spell
{
    public ArcaneShot() : base(1)
    {
        SetSpell((b) =>
        {
            return DealDamage(2 + owner.SpellDamage, b);
        });
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero me = owner.id == b.me.id ? b.me : b.opp;
        Hero opp = owner.id == b.me.id ? b.opp : b.opp;
        int damage = 2 + me.SpellDamage;
        double max = 0;
        max = Math.Max(me.CalcValue(health: -1 * damage, cards: -1) - me.CalcValue(), max);
        max = Math.Max(opp.CalcValue() - opp.CalcValue(health: -1 * damage, cards: -1), max);

        foreach (Minion m in me.onBoard)
        {
            if (m.Health <= damage)
            {
                if (me.onBoard.Count == 1)
                {
                    max = Math.Max(me.CalcValue(minions: -1 * (m.Health + m.Attack + 2 + me.maxMana), cards: -1) - me.CalcValue(), max);
                }
                else
                {
                    max = Math.Max(me.CalcValue(minions: -1 * (m.Health + m.Attack), cards: -1) - me.CalcValue(), max);
                }
            }
            else
            {
                max = Math.Max(me.CalcValue(minions: -1 * damage, cards: -1) - me.CalcValue(), max);
            }
        }

        foreach (Minion m in opp.onBoard)
        {
            if (m.Health <= damage)
            {
                if (opp.onBoard.Count == 1)
                {
                    max = Math.Max(opp.CalcValue() - opp.CalcValue(minions: -1 * (m.Health + m.Attack + 2 + opp.maxMana), cards: -1), max);
                }
                else
                {
                    max = Math.Max(opp.CalcValue() - opp.CalcValue(minions: -1 * (m.Health + m.Attack), cards: -1), max);
                }
            }
            else
            {
                max = Math.Max(opp.CalcValue() - opp.CalcValue(minions: -1 * damage, cards: -1), max);
            }
        }

        return max;
    }
}