using System.Collections.Generic;

class MultiShot : Spell
{
    public MultiShot() : base(4)
    {
        SetSpell((b) =>
        {
            List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
            Hero opp = b.me.id == owner.id ? b.opp : b.me;
            for (int i = 0; i < opp.onBoard.Count; i++)
            {
                for (int j = i + 1; j < opp.onBoard.Count; j++)
                {
                    Board cln = b.Clone();
                    Hero oppCln = cln.me.id == owner.id ? cln.opp : cln.me;
                    string targets = oppCln.onBoard[j] + " + " + oppCln.onBoard[i];
                    oppCln.onBoard[j].Health -= (3 + owner.SpellDamage);
                    oppCln.onBoard[i].Health -= (3 + owner.SpellDamage);

                    result.Add((new MasterBoardContainer(cln) { action = targets }, 1));
                }
            }
            return new RandomSubBoardContainer(result, b, "Play " + this);
        });
    }

    public override bool CanPlay(Board b)
    {
        return base.CanPlay(b) && (owner.id == b.me.id ? b.opp : b.me).onBoard.Count >= 2;
    }
}