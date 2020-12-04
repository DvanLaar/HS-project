using System;

class Flamestrike : Spell
{
    public Flamestrike() : base(7)
    {
        SetSpell((b) =>
        {
            Board cln = b.Clone();
            Hero opp = cln.me.id == owner.id ? cln.opp : cln.me;
            foreach (Minion m in opp.onBoard)
            {
                m.ReduceHealth(4 + owner.SpellDamage);
            }
            return new SingleSubBoardContainer(cln, b, "Play Flamestrike");
        });
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        Hero opp = b.me.id == owner.id ? b.opp : b.me;
        int damage = 4 + owner.SpellDamage;
        bool allDead = true;
        int damages = 0;
        foreach (Minion m in opp.onBoard)
        {
            if (m.Health <= damage)
            {
                damages += m.Health + m.Attack;
            }
            else
            {
                damages += damage;
                allDead = false;
            }
        }
        if (allDead)
            damages += 2 + opp.maxMana;
        return opp.CalcValue() + owner.CalcValue(cards: -1) - opp.CalcValue(minions: 0 - damages) - owner.CalcValue();
    }
}
