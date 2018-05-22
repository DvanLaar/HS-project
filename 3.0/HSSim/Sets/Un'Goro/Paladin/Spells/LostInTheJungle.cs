using System.Collections.Generic;

class LostInTheJungle : Spell
{
    public LostInTheJungle() : base(1)
    {
        SetSpell((b) =>
        {
            Board cln = b.Clone();
            Hero me = owner.id == cln.me.id ? cln.me : cln.opp;
            me.StartSummon(new SilverHandRecruit());
            me.StartSummon(new SilverHandRecruit());
            me.Mana -= cost;
            return new SingleSubBoardContainer(cln, b, "Play " + this);
        });
    }

    public override bool CanPlay(Board b)
    {
        return base.CanPlay(b) && owner.onBoard.Count < 7;
    }
}