using System;
using System.Collections.Generic;

class MasterBoardContainer
{
    private static readonly double winValue = 1000, loseValue = -1000;
    public string action;

    public List<SubBoardContainer> children;
    public bool expanded;
    public double value { get { if (board.opp.Health <= 0) return winValue; if (board.me.Health <= 0) return loseValue; return expanded ? children[0].value : board.value; } }
    public Board board;

    

    public MasterBoardContainer(Board b)
    {
        board = b;
        expanded = false;
        children = new List<SubBoardContainer>();
    }

    public void Expand()
    {
        if (expanded)
            return;

        if (value > 999 || value < -999)
        {
            expanded = true;
            return;
        }
        expanded = true;
        children = new List<SubBoardContainer>();

        if (board.toPerform.Count > 0)
        {
            Func<Board, SubBoardContainer> action = board.toPerform.Pop();
            Board cln = board.Clone();
            SubBoardContainer sbc = action(cln);
            children.Add(sbc);
            return;
        }

        Hero currentPlayer = board.me.id == board.curr ? board.me : board.opp;

        foreach (Card c in currentPlayer.hand)
        {
            SubBoardContainer play = c.Play(board);
            if (play != null)
                children.Add(play);
        }
        foreach (Minion m in currentPlayer.onBoard)
        {
            SubBoardContainer attack = m.PerformAttack(board);
            if (attack != null)
                children.Add(attack);
        }
        SubBoardContainer hattack = currentPlayer.PerformAttack(board);
        if (hattack != null)
            children.Add(hattack);

        SubBoardContainer hp = currentPlayer.UseHeroPower(board);
        if (hp != null)
            children.Add(hp);


        Board clone = board.Clone();
        (clone.me.id == clone.curr ? clone.me : clone.opp).EndTurn(board);
        clone.curr = !clone.curr;
        (clone.me.id == clone.curr ? clone.me : clone.opp).StartTurn(board);
        children.Add(new SingleSubBoardContainer(clone, board, "End turn"));
    }

    public void Sort()
    {
        if (!expanded)
            return;
        foreach (SubBoardContainer sbc in children)
        {
            foreach (MasterBoardContainer mbc in sbc.children)
            {
                mbc.Sort();
            }
            sbc.children.Sort((x, y) => board.curr ? y.value.CompareTo(x.value) : x.value.CompareTo(y.value));
        }
        children.Sort((x, y) => board.curr ? y.value.CompareTo(x.value) : x.value.CompareTo(y.value));
    }
}

abstract class SubBoardContainer
{
    public List<MasterBoardContainer> children;
    public abstract double value { get; }
    public Board parent;
    public string action;

    public SubBoardContainer(Board parent, string action)
    {
        this.action = action;
        this.parent = parent;
    }
}

class SingleSubBoardContainer : SubBoardContainer
{
    public override double value => children[0].value;

    public SingleSubBoardContainer(MasterBoardContainer b, Board parent, string action) : base(parent, action)
    {
        children = new List<MasterBoardContainer>() { b };
    }

    public SingleSubBoardContainer(Board b, Board parent, string action) : this(new MasterBoardContainer(b), parent, action)
    {
    }
}

class ChoiceSubBoardContainer : SubBoardContainer
{
    public override double value { get { double res = double.MinValue; foreach (MasterBoardContainer mbc in children) if (mbc.value > res) res = mbc.value; return res; } }

    public ChoiceSubBoardContainer(List<Board> boards, Board parent, string action) : base(parent, action)
    {
        children = new List<MasterBoardContainer>();

        foreach (Board b in boards)
            children.Add(new MasterBoardContainer(b));
    }

    public ChoiceSubBoardContainer(List<MasterBoardContainer> mbcs, Board parent, string action) : base(parent, action)
    {
        children = mbcs;
    }
}

class RandomSubBoardContainer : SubBoardContainer
{
    public List<int> occurences;

    public override double value { get { double res = 0; int total = 0; foreach (int i in occurences) total += i; for (int i = 0; i < children.Count; i++) res += children[i].value * occurences[i] / total; return res; } }

    public RandomSubBoardContainer(List<Board> boards, List<int> occurences, Board parent, string action) : base(parent, action)
    {
        children = new List<MasterBoardContainer>();
        foreach (Board b in boards)
            children.Add(new MasterBoardContainer(b));
        this.occurences = occurences;
    }

    public RandomSubBoardContainer(List<(Board, int)> input, Board parent, string action) : base(parent, action)
    {
        children = new List<MasterBoardContainer>();
        occurences = new List<int>();

        foreach ((Board, int) kvp in input)
        {
            children.Add(new MasterBoardContainer(kvp.Item1));
            occurences.Add(kvp.Item2);
        }
    }

    public RandomSubBoardContainer(List<(MasterBoardContainer, int)> input, Board parent, string action) : base(parent, action)
    {
        children = new List<MasterBoardContainer>();
        occurences = new List<int>();

        foreach ((MasterBoardContainer, int) kvp in input)
        {
            children.Add(kvp.Item1);
            occurences.Add(kvp.Item2);
        }
    }

}

class UnknownSubBoardContainer : SubBoardContainer
{
    readonly List<SubBoardContainer> options;

    public UnknownSubBoardContainer(List<SubBoardContainer> options, Board parent, string action) : base(parent, action)
    {
        this.options = options;
        children = new List<MasterBoardContainer>();
        foreach (SubBoardContainer sbc in options)
            foreach (MasterBoardContainer mbc in sbc.children)
            {
                mbc.action = sbc.action + " " + mbc.action;
                children.Add(mbc);
            }
    }

    public override double value { get { double res = double.MinValue; foreach (SubBoardContainer sbc in options) if (sbc.value > res) res = sbc.value; return res; } }
}

class RandomChoiceSubBoardContainer : SubBoardContainer
{
    public List<(List<MasterBoardContainer>, int, string)> options;

    public RandomChoiceSubBoardContainer(List<(List<MasterBoardContainer>, int, string)> input, Board parent, string action) : base(parent, action)
    {
        children = new List<MasterBoardContainer>();
        options = input;
        foreach ((List<MasterBoardContainer>, int, string) results in input)
            foreach (MasterBoardContainer mbc in results.Item1)
                children.Add(mbc);

    }

    public override double value { get { double res = 0; foreach ((List<MasterBoardContainer>, int, string) kvp in options) { double mbcValue = double.MinValue; foreach (MasterBoardContainer mbc in kvp.Item1) if (mbc.value > mbcValue) mbcValue = mbc.value; res += mbcValue * kvp.Item2; } return res; } }
}