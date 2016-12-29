using System;
using System.IO;
using System.Collections.Generic;

class Simulation
{
    public List<MyList<Board>> Staircase;

    public Simulation(Board board)
    {
        Staircase = new List<MyList<Board>>();
        Staircase.Add(new MyList<Board>() { board });
        while (Staircase[Staircase.Count - 1] != Staircase[Staircase.Count - 2])
            walkDownOneStep();
        for (int i = Staircase.Count - 2; i >= 0; i--)
        {
            foreach (Board b in Staircase[i])
            {
                b.computeValue();
            }
        }
    }
    public void walkDownOneStep()
    {
        MyList<Board> lastStep = Staircase[Staircase.Count - 1];
        MyList<Board> nextStep = new MyList<Board>();

        foreach(Board board in lastStep)
        {
            nextStep.AddRange(filter(board.getChildren()));
        }
        Staircase.Add(nextStep);
    }
    private MyList<Board> filter(MyList<Board> toFilter)
    {
        if (toFilter.innerList.Exists(board => board.chance == 1 && board.value == 1))
            return new MyList<Board>() { toFilter.innerList.Find(board => board.chance == 1 && board.value == 1) };
        toFilter.innerList.RemoveAll(board => board.chance == 1 && board.value == 0);
        return toFilter;
    }
}
