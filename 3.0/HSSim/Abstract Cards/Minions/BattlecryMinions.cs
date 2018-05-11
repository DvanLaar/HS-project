using System;

abstract class BattlecryMinion : Minion
{
    Func<Board, BoardContainer> Battlecry;

    public BattlecryMinion(int mana, int attack, int health) : base(mana, attack, health)
    {

    }

    public void SetBattlecry(Func<Board, BoardContainer> bc)
    {
        Battlecry = bc;
    }

    public override BoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        SingleBoardContainer b = (SingleBoardContainer)base.Play(curBoard);
        return Battlecry.Invoke(b.b);        
    }
}