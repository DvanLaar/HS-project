using System;
using System.Collections.Generic;

class Polymorph : Spell
{
    public Polymorph() : base(4)
    {
        SetSpell((b) =>
        {
            List<Board> result = new List<Board>();
            Hero opponent = b.me.id == owner.id ? b.opp : b.me;
            foreach (Minion m in opponent.onBoard)
            {
                Board clone = b.Clone();
                Hero opp = clone.me.id == opponent.id ? clone.me : clone.opp;
                Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
                me.Mana -= cost;
                Minion sheep = new Sheep();
                sheep.SetOwner(opp);
                opp.onBoard[opponent.onBoard.IndexOf(m)] = sheep;
                result.Add(clone);
            }

            foreach (Minion m in owner.onBoard)
            {
                Board clone = b.Clone();
                Hero opp = clone.me.id == opponent.id ? clone.me : clone.opp;
                Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
                me.Mana -= cost;
                Minion sheep = new Sheep();
                sheep.SetOwner(me);
                me.onBoard[owner.onBoard.IndexOf(m)] = sheep;
                result.Add(clone);
            }
            return new MultipleChoiceBoardContainer(result, "play polymorph");
        });
    }

    public override bool CanPlay(Board b)
    {
        if (!base.CanPlay(b))
            return false;

        return b.me.onBoard.Count > 0 || b.opp.onBoard.Count > 0;
    }
}