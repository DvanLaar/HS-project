using System;
using System.Collections.Generic;

class Polymorph : Spell
{
    public Polymorph() : base(4)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> result = new List<MasterBoardContainer>();
            Hero opponent = b.me.id == owner.id ? b.opp : b.me;
            foreach (Minion m in opponent.onBoard)
            {
                Board clone = b.Clone();
                Hero opp = clone.me.id == opponent.id ? clone.me : clone.opp;
                Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
                Minion sheep = new Sheep();
                sheep.SetOwner(opp);
                opp.onBoard[opponent.onBoard.IndexOf(m)].StartTransform();
                opp.onBoard[opponent.onBoard.IndexOf(m)] = sheep;
                result.Add(new MasterBoardContainer(clone) { action = "Transform " + m });
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
                result.Add(new MasterBoardContainer(clone) { action = "Transform " + m }) ;
            }
            return new ChoiceSubBoardContainer(result, b, "Play Polymorph");
        });

    }
    public override bool CanPlay(Board b)
    {
        if (!base.CanPlay(b))
            return false;

        return b.me.onBoard.Count > 0 || b.opp.onBoard.Count > 0;
    }
}