using System;
using System.Collections.Generic;

class Algorithm
{
    Board board;
    int maxDepth;
    float winValue = 1000f;
    float loseValue = -1000f;

    public Algorithm(Board startboard, int maxDepth)
    {
        board = startboard;
        this.maxDepth = maxDepth;
    }

    public void Perform()
    {

    }

    //public static List<MasterBoardContainer> getSuc(BoardContainer brdcntr)
    //{
    //    List<BoardContainer> result = new List<BoardContainer>();
    //    List<BoardContainer> realresult = new List<BoardContainer>();
    //    foreach (Board board in brdcntr.boards)
    //    {
    //        Hero currentPlayer = board.curr;

    //        foreach (Card c in currentPlayer.hand)
    //        {
    //            BoardContainer play = c.Play(board);
    //            if (play != null)
    //                result.Add(play);
    //        }
    //        foreach (Minion m in currentPlayer.onBoard)
    //        {
    //            BoardContainer attack = m.PerformAttack(board);
    //            if (attack != null)
    //                result.Add(attack);
    //        }
    //        BoardContainer hattack = currentPlayer.PerformAttack(board);
    //        if (hattack != null)
    //            result.Add(hattack);

    //        BoardContainer hp = currentPlayer.UseHeroPower(board);
    //        if (hp != null)
    //            result.Add(hp);

            
    //        Board clone = board.Clone();
    //        clone.EndTurn();
    //        result.Add(new SingleBoardContainer(clone, "End turn"));

    //        List<Board> list = new List<Board>();

    //        foreach (BoardContainer b in result)
    //        {
    //            list.AddRange(b.boards);
    //        }

    //        //foreach (BoardContainer b in result)
    //        //{
    //        //    realresult.AddRange(getSuc(b));
    //        //}
    //    }
    //    return result;
    //}
}