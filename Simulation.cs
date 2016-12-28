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
    }
    public void walkDownOneStep()
    {
        MyList<Board> lastStep = Staircase[Staircase.Count - 1];
        MyList<Board> nextStep = new MyList<Board>();

        foreach(Board board in lastStep)
        {
            MyList<Board> kids = board.getChildren();
            nextStep.AddRange(kids);
        }
        Staircase.Add(nextStep);
    }
}
