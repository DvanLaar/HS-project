using System;
using System.Collections.Generic;

class MasterBoardContainer
{
    static double winValue = 1000, loseValue = -1000;

    public List<SubBoardContainer> children;
    public bool expanded;
    public double value { get { if (board.opp.Health <= 0) return 1000; if (board.me.Health <= 0) return -1000; return expanded ? children[0].value : board.value; } }
    public Board board;

    public MasterBoardContainer(Board b)
    {
        board = b;
        expanded = false;
        children = new List<SubBoardContainer>();
    }

    public void Expand()
    {
        expanded = true;
        Hero currentPlayer = board.curr;

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
        clone.EndTurn();
        children.Add(new SingleSubBoardContainer(clone, board, "End Turn"));
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
            sbc.children.Sort((x, y) => y.value.CompareTo(x.value));
        }
        children.Sort((x, y) => y.value.CompareTo(x.value));
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

}