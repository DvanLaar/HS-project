using System;
using System.Collections.Generic;

class ArcaneMissiles : Spell
{
    public ArcaneMissiles() : base(1)
    {
        SetSpell((b) =>
        {
            List<(MasterBoardContainer, int)> temp = new List<(MasterBoardContainer, int)>();
            (MasterBoardContainer, int)[] result = new(MasterBoardContainer, int)[1] { (new MasterBoardContainer(b), 1) };
            for (int i = 0; i < (3 + owner.SpellDamage); i++)
            {
                foreach((MasterBoardContainer, int) brd in result)
                {
                    temp.AddRange(RandomDamage(brd.Item1.board, brd.Item1.action));
                }
                result = temp.ToArray();
                temp.Clear();
            }
            return new RandomSubBoardContainer(new List<(MasterBoardContainer, int)>(result), b, "Play Arcane Missiles");
        });
    }

    private (MasterBoardContainer, int)[] RandomDamage(Board b, string prevAction)
    {
        List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
        Hero opponent = b.me.id == owner.id ? b.opp : b.me;

        foreach (Minion m in opponent.onBoard)
        {
            Board clone = b.Clone();
            Hero Opp = clone.me.id == opponent.id ? clone.me : clone.opp;
            Opp.onBoard[opponent.onBoard.IndexOf(m)].TakeDamage(1);
            result.Add((new MasterBoardContainer(clone) {action = prevAction + " Hit " + m}, 1));
        }

        Board cln = b.Clone();
        Hero guy = cln.me.id == opponent.id ? cln.me : cln.opp;
        guy.TakeDamage(1);
        result.Add((new MasterBoardContainer(cln) { action = prevAction + " Hit Face" }, 1));

        return result.ToArray();
    }
}