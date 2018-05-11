using System;
using System.Collections.Generic;

class ArcaneMissiles : Spell
{
    public ArcaneMissiles() : base(1)
    {
        SetSpell((b) =>
        {
            List<(Board, int)> temp = new List<(Board, int)>();
            (Board, int)[] result = new(Board, int)[1] { (b, 1) };
            for (int i = 0; i < 3; i++)
            {
                foreach((Board, int) brd in result)
                {
                    temp.AddRange(RandomDamage(brd.Item1));
                }
                result = temp.ToArray();
                temp.Clear();
            }
            foreach ((Board, int) brd in result)
            {
                (brd.Item1.me.id == owner.id ? brd.Item1.me : brd.Item1.opp).Mana -= cost;
            }
            return new MultipleBoardContainer(new List<(Board, int)>(result), "Play Missiles");
        });
    }

    private (Board, int)[] RandomDamage(Board b)
    {
        List<(Board, int)> result = new List<(Board, int)>();
        Hero opponent = b.me.id == owner.id ? b.opp : b.me;

        foreach(Minion m in opponent.onBoard)
        {
            Board clone = b.Clone();
            Hero Opp = clone.me.id == opponent.id ? clone.me : clone.opp;
            (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
            Opp.onBoard[opponent.onBoard.IndexOf(m)].Health--;
            result.Add((clone, 1));
        }

        Board cln = b.Clone();
        Hero guy = cln.me.id == opponent.id ? cln.me : cln.opp;
        guy.Health--;
        result.Add((cln, 1));

        return result.ToArray();
    }
}