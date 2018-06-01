using System.Collections.Generic;

class Execute : Spell
{
    public Execute() : base(2)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> result = new List<MasterBoardContainer>();
            Hero opponent = owner.id == b.me.id ? b.opp : b.me;
            foreach (Minion m in opponent.onBoard)
            {
                if (!m.Damaged)
                    continue;

                Board clone = b.Clone();
                Hero opp = owner.id == clone.me.id ? clone.opp : clone.me;
                opp.onBoard[opponent.onBoard.IndexOf(m)].StartDestroy();
                result.Add(new MasterBoardContainer(clone) { action = "Target " + m });
            }

            if (result.Count == 0)
                return null;

            return new ChoiceSubBoardContainer(result, b, "Play " + this);
        });
    }
}