using System;
using System.Collections.Generic;

class Fireball : Spell
{
    public Fireball() : base(4)
    {
        SetSpell((b) =>
        {
            List<Board> result = new List<Board>();
            Hero opponent = b.me.id == owner.id ? b.opp : b.me;

            Board clone;

            clone = b.Clone();
            clone.me.Health -= 6;
            (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
            result.Add(clone);

            clone = b.Clone();
            clone.opp.Health -= 6;
            (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
            result.Add(clone);

            foreach (Minion m in owner.onBoard)
            {
                clone = b.Clone();
                Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
                me.Mana -= 4;
                Minion target = me.onBoard[owner.onBoard.IndexOf(m)];
                target.Health -= 3;
                result.Add(clone);
            }

            foreach (Minion m in opponent.onBoard)
            {
                clone = b.Clone();
                Hero Opponent = clone.me.id == opponent.id ? clone.me : clone.opp;
                (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
                Minion target = Opponent.onBoard[opponent.onBoard.IndexOf(m)];
                target.Health -= 3;
                result.Add(clone);
            }

            return new MultipleChoiceBoardContainer(result, "play Fireball");
        });
    }
}