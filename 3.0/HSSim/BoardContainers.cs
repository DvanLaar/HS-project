using System.Collections.Generic;

abstract class BoardContainer
{
    public abstract double value { get; }
    public abstract List<Board> boards { get; }
    public string play;
    public bool useOwnValue = true;
    public BoardContainer succ;
}

class SingleBoardContainer : BoardContainer
{
    public SingleBoardContainer(Board b, string play)
    {
        this.b = b;
        this.play = play;
        this.succ = null;
    }

    public override double value => b.value;
    public override List<Board> boards => new List<Board>() { b };
    public Board b;
}

class MultipleBoardContainer : BoardContainer
{
    public override double value { get
        {
            int total = 0;
            foreach ((BoardContainer, int) bi in list)
                total += bi.Item2;

            double result = 0;
            foreach ((BoardContainer, int) bi in list)
                result += bi.Item1.value * (((double)bi.Item2) / total);
            return result;
        } }
    public override List<Board> boards { get
        {
            List<Board> res = new List<Board>();
            foreach ((BoardContainer, int) bi in list)
                res.AddRange(bi.Item1.boards);
            return res;
        } }
    public List<(BoardContainer, int)> list = new List<(BoardContainer, int)>();

    public MultipleBoardContainer(List<(Board, int)> list, string play)
    {
        this.play = play;
        this.list = new List<(BoardContainer, int)>();
        foreach ((Board, int) bi in list)
        {
            this.list.Add((new SingleBoardContainer(bi.Item1, ""), bi.Item2));
        }
        this.succ = null;
    }
}

class MultipleChoiceBoardContainer : BoardContainer
{
    List<BoardContainer> list = new List<BoardContainer>();
    public override double value { get
        {
            double result = double.MinValue;
            foreach (BoardContainer bi in list)
                if (bi.value > result)
                    result = bi.value;
            return result;
        } }
    public override List<Board> boards { get
        {
            List<Board> lst = new List<Board>();
            foreach (BoardContainer b in list)
                lst.AddRange(b.boards);
            return lst;
        } }

    public MultipleChoiceBoardContainer(List<Board> list, string play)
    {
        this.play = play;
        this.succ = null;
        foreach (Board b in list)
            this.list.Add(new SingleBoardContainer(b, ""));
    }
}