using System;
using System.Collections.Generic;

class ArcaneIntellect : Spell
{
    public ArcaneIntellect() : base(3)
    {
        SetSpell((b) =>
        {
            List<(Board, int)> result = new List<(Board, int)>();
            Board clone = b.Clone();
            Hero me = owner.id == clone.me.id ? clone.me : clone.opp;
            me.Mana -= cost;
            foreach (Board brd in me.DrawCard(clone).boards)
            {
                Hero own = owner.id == brd.me.id ? brd.me : brd.opp;
                BoardContainer bc = own.DrawCard(brd);
                foreach (Board toAdd in bc.boards)
                    result.Add((toAdd, 1));
            }

            return new MultipleBoardContainer(result, "play arcane intellect");
        });
    }
}