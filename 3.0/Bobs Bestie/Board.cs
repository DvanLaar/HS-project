using System;
using System.Collections.Generic;

class Board
{
    public Minion[] Minions;

    public List<Board> Children;
    public bool Expanded, IAttack;
    public Queue<(string, Args)> EventQueue;
    public int MyAttackIndex, TheirAttackIndex;
    public string Event;

    public int MyMinCount { get { int result = 0; for (int i = 0; i < 7; i++) if (Minions[i] != null) result++; else break; return result;} }
    public int TheirMinCount { get { int result = 0; for (int i = 7; i < 14; i++) if (Minions[i] != null) result++; else break; return result; } }
    public IntArgs NextAttack { get { return new IntArgs(IAttack ? MyAttackIndex : TheirAttackIndex); } }

    public Board()
    {
        Minions = new Minion[14];
        Children = new List<Board>();
        Expanded = false;
        EventQueue = new Queue<(string, Args)>();
        MyAttackIndex = 0;
        TheirAttackIndex = 7;
    }

    public Board Clone()
    {
        Board clone = new Board()
        {
            IAttack = IAttack,
            MyAttackIndex = MyAttackIndex,
            TheirAttackIndex = TheirAttackIndex
        };
        for(int i = 0; i < 14; i++)
        {
            if (Minions[i] == null)
                continue;
            clone.Minions[i] = Minions[i].Clone();
        }
        foreach ((string, Args) NextEvent in EventQueue)
        {
            clone.EventQueue.Enqueue(NextEvent);
        }
        return clone;
    }

    public void StartSim()
    {
        if (MyMinCount > TheirMinCount)
            IAttack = true;
        else if (MyMinCount < TheirMinCount)
            IAttack = false;
        else
            EventQueue.Enqueue(("determine IAttack", new EmptyArgs()));

        Expand();
    }

    public void Expand()
    {
        if (Expanded)
            return;

        Expanded = true;

        if (EventQueue.Count == 0)
        {
            EventQueue.Enqueue(("attack", NextAttack));
        }

        (string, Args) NextEvent = EventQueue.Dequeue();
        Event = NextEvent.Item1;
        switch (Event)
        {
            case "determine IAttack":
                DI();
                break;
            case "attack":
                Event += " " + ((IntArgs)NextEvent.Item2).Attacker;
                A((IntArgs)NextEvent.Item2);
                break;
            case "give divine shield":
                Event += " " + ((BoolArgs)NextEvent.Item2).Boolean;
                GDS((BoolArgs)NextEvent.Item2);
                break;
            default:
                Console.WriteLine("PANIEK");
                break;
        }

        if (Children.Count == 0)
            return;

        Console.WriteLine(Event);

        foreach (Board child in Children)
        {
            child.Expand();
        }
    }

    public void GDS(BoolArgs ba)
    {
        int index;
        if (ba.Boolean)
            index = 0;
        else
            index = 7;

        for (int i = index; i < index + 7; i++)
        {
            if (Minions[i] == null)
                break;
            if (Minions[i].DivineShield)
                continue;
            Board b = Clone();
            b.Minions[i].DivineShield = true;
            Children.Add(b);
        }
        if (Children.Count == 0)
            Children.Add(Clone());
    }

    public void A(IntArgs aa)
    {
        int[] attacks = Attack(aa);
        foreach (int attack in attacks)
        {
            Board b = Clone();
            b.PerformAttack(aa, attack);
            Children.Add(b);
        }
    }

    public void DI()
    {
        Board b = Clone();
        b.IAttack = true;
        Children.Add(b);
        Board b2 = Clone();
        b2.IAttack = false;
        Children.Add(b2);
    }

    public int[] Attack(IntArgs aa)
    {
        List<int> result = new List<int>();
        if (MyMinCount == 0 || TheirMinCount == 0)
            return new int[0];
        int DefMinIndex = IAttack ? 7 : 0;
        int DefenderCount = IAttack ? TheirMinCount : MyMinCount;

        int Tauntcount = 0;
        for (int i = DefMinIndex; i < DefenderCount + DefMinIndex; i++)
        {
            if (Minions[i].Taunt)
                Tauntcount++;
        }
        if (Tauntcount == 0)
        {
            for (int i = DefMinIndex; i < DefenderCount + DefMinIndex; i++)
            {
                result.Add(i);
            }
        }
        else
        {
            for (int i = DefMinIndex; Tauntcount > 0; i++)
            {
                if (!Minions[i].Taunt)
                    continue;

                result.Add(i);
                Tauntcount--;
            }
        }
        return result.ToArray();
    }

    public void PerformAttack(IntArgs aa, int DefenderIndex)
    {
        Minion Attacker = Minions[aa.Attacker];
        Minion Defender = Minions[DefenderIndex];
        int AttackerAIndex, DefenderAIndex;
        if (aa.Attacker < 7)
        {
            AttackerAIndex = MyAttackIndex;
            DefenderAIndex = TheirAttackIndex;
        }
        else
        {
            AttackerAIndex = TheirAttackIndex;
            DefenderAIndex = MyAttackIndex;
        }
        AttackerAIndex++;

        if (Attacker.DivineShield && Defender.Attack > 0)
            Attacker.DivineShield = false;
        else
            Attacker.Health -= Defender.Attack;
        if (Defender.DivineShield && Attacker.Attack > 0)
            Defender.DivineShield = false;
        else
            Defender.Health -= Attacker.Attack;

        if (Attacker.Health <= 0)
        {
            AttackerAIndex--;
            for (int i = aa.Attacker; i % 7 != 6; i++)
            {
                Minions[i] = Minions[i + 1];
            }
            Minions[aa.Attacker / 7 * 7 +6 ] = null;
            (string, Args)[] a = Attacker.OnDeath(aa.Attacker);
            foreach ((string, Args) sa in a)
                EventQueue.Enqueue(sa);
        }
        if (Defender.Health <= 0)
        {
            for (int i = DefenderIndex; i % 7 != 6; i++)
            {
                Minions[i] = Minions[i + 1];
            }
            Minions[(DefenderIndex+1) / 7 * 7 + 6] = null;
            if (DefenderAIndex > DefenderIndex)
                DefenderAIndex--;
            (string, Args)[] a = Defender.OnDeath(DefenderIndex);
            foreach ((string, Args) sa in a)
                EventQueue.Enqueue(sa);
        }
        if (aa.Attacker < 7)
        {
            if (AttackerAIndex >= MyMinCount)
                AttackerAIndex = 0;
            MyAttackIndex = AttackerAIndex;
            if (DefenderAIndex - 7 >= TheirMinCount)
                DefenderAIndex = 7;
            TheirAttackIndex = DefenderAIndex;
        }
        else
        {
            if (AttackerAIndex - 7 >= TheirMinCount)
                AttackerAIndex = 7;
            MyAttackIndex = DefenderAIndex;
            if (DefenderAIndex >= MyMinCount)
                DefenderAIndex = 0;
            TheirAttackIndex = AttackerAIndex;
        }

        IAttack = !IAttack;
    }

    public double[] CalcScore()
    {
        if (!Expanded)
            return null;
        if (Children.Count == 0)
        {
            if (MyMinCount > 0 && TheirMinCount == 0)
                return new double[] { 0, 100, 0, 0, 0 };
            else if (MyMinCount == 0 && TheirMinCount > 0)
                return new double[] { 0, 0, 0, 100, 0 };
            else
                return new double[] { 0, 0, 100, 0, 0 };
        }
        double[] result = new double[] { 0, 0, 0, 0, 0 };
        foreach (Board b in Children)
        {
            double[] ChildScore = b.CalcScore();
            for (int i = 0; i < 5; i++)
            {
                result[i] += ChildScore[i] / Children.Count;
            }
        }
        return result;
    }
}
