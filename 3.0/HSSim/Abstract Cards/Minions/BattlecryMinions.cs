using System;

abstract class BattlecryMinion : Minion
{
    Func<Board, SubBoardContainer> Battlecry;

    public BattlecryMinion(int mana, int attack, int health) : base(mana, attack, health)
    {

    }

    public void SetBattlecry(Func<Board, SubBoardContainer> bc)
    {
        Battlecry = bc;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        SubBoardContainer b = base.Play(curBoard);
        return Battlecry.Invoke(b.children[0].board);        
    }
}