using System;
using System.Collections.Generic;

class UI
{
    public static void Main()
    {
        Board current = new Board();

        while(true)
        {
            //Console.Clear();
            Console.WriteLine("Enter a command here");

            string s = Console.ReadLine().ToLower();
            Console.Clear();
            switch (s)
            {
                case "add minion":
                    AddMinionInit(current);
                    break;
                case "start sim":
                    current.StartSim();
                    break;
                case "calc score":
                    double[] score = current.CalcScore();
                    for (int i = 0; i < 5; i++) Console.Write(score[i] + ", ");
                    Console.WriteLine();
                    break;
                case "exit":
                    return;
            }
            
        }
    }

    public static void AddMinionInit(Board current)
    {
        Console.Clear();
        Console.WriteLine("For You or Opponent?");
        string s = Console.ReadLine().ToLower();
        switch (s)
        {
            case "me":
                AddMinion(current.Minions, current.MyMinCount);
                break;
            case "opponent":
                AddMinion(current.Minions, current.TheirMinCount + 7);
                break;
        }
    }

    public static void AddMinion(Minion[] mins, int index)
    {
        Console.Clear();
        Console.WriteLine("Which minion?");
        string s = Console.ReadLine().ToLower();
        switch (s)
        {
            case "vanilla":
                mins[index] = Vanilla.MakeMinion();
                break;
            case "selfless hero":
                mins[index] = SelflessHero.MakeMinion();
                break;
        }
    }
}