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
       
}
