using System.Collections.Generic;

class FrostShock : Spell
{
    public FrostShock() : base(1)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> results = new List<MasterBoardContainer>();
            Hero opp = owner.id == b.me.id ? b.opp : b.me;
            foreach (Minion m in opp.onBoard)
            {
                Board clone = b.Clone();
                Hero opponent = clone.me.id == opp.id ? clone.me : clone.opp;
                Minion target = opponent.onBoard[opp.onBoard.IndexOf(m)];
                target.Frozen = true;
                target.TakeDamage(1 + owner.SpellDamage);
                results.Add(new MasterBoardContainer(clone) { action = "Target " + target });
            }
            Board cln = b.Clone();
            Hero trgt = cln.me.id == opp.id ? cln.me : cln.opp;
            trgt.Frozen = true;
            trgt.TakeDamage(1 + owner.SpellDamage);
            results.Add(new MasterBoardContainer(cln) { action = "Target face" });

            return new ChoiceSubBoardContainer(results, b, "Play " + this);
        });
    }
}