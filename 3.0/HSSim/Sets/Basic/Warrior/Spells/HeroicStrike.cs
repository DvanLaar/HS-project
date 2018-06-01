using System;

class HeroicStrike : Spell
{
    public HeroicStrike() : base(2)
    {
        SetSpell((b) =>
        {
            Board clone = b.Clone();
            Hero me = owner.id == clone.me.id ? clone.me : clone.opp;
            me.Attack += 4;
            Func<Board, SubBoardContainer> function = (brd) =>
            {
                Board cln = brd.Clone();
                Hero hero = owner.id == cln.me.id ? cln.me : cln.opp;
                hero.Attack -= 4;
                return new SingleSubBoardContainer(cln, brd, "End of " + this + " effect");
            };
            me.EndTurnFuncs.Add(function);
            me.SingleEndTurnFuncs.Add(me.EndTurnFuncs.IndexOf(function));
            return new SingleSubBoardContainer(clone, b, "Play " + this);
        });
    }
}