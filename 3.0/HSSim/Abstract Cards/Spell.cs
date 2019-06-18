using System;

abstract class Spell : Card
{
    protected Func<Board, SubBoardContainer> Cast;

    public Spell(int mana) : base(mana)
    {
    }

    public void SetSpell(Func<Board, SubBoardContainer> effect)
    {
        Cast = effect;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        Board clone = curBoard.Clone();
        Hero own = owner.id == clone.me.id ? clone.me : clone.opp; //Move to card
        own.hand.RemoveAt(owner.hand.IndexOf(this));
        own.Mana -= cost;

        return Cast.Invoke(clone);
    }
}