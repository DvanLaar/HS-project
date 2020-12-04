using System;
using System.Collections.Generic;

class ArcaneExplosion : Spell
{
    public ArcaneExplosion() : base(2)
    {
        SetSpell((b) =>
        {
            Board clone = b.Clone();
            Hero Opp = clone.me.id == owner.id ? clone.opp : clone.me;
            Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
            for (int i = 0; i < Opp.onBoard.Count; i++)
            {
                Opp.onBoard[i].TakeDamage(1 + owner.SpellDamage);
            }
            return new SingleSubBoardContainer(clone, b, "Play Arcane Explosion");
        });
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero opp = b.me.id == owner.id ? b.opp : b.me;
        Hero me = b.me.id == owner.id ? b.me : b.opp;
        int damage = 1 + owner.SpellDamage;
        int minionIncrease = 0;
        bool allDead = true;

        foreach (Minion m in opp.onBoard)
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
            minionIncrease += 2 + opp.maxMana;

        return opp.CalcValue() + me.CalcValue(cards: -1) - opp.CalcValue(minions: -1 * minionIncrease) - me.CalcValue();
    }
}