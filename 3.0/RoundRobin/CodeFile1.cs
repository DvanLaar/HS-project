using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        List<(int, int, int)> played = new List<(int, int, int)>();
        for (int i = 0; i < n; i++)
        {
            string[] line = Console.ReadLine().Split();
            int p1 = int.Parse(line[0]);
            int p2 = int.Parse(line[1]);
            int winner = int.Parse(line[2]);
            played.Add((p1, p2, winner));
        }

        List<(int, int)> matches = new List<(int, int)>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = i + 1; j < 8; j++)
            {
                matches.Add((i, j));
            }
        }

        //construct tree
        Node root = new Node(played, matches);

        //give output
        (int, int)[] bottop = new (int, int)[8];
        for (int i = 0; i < 8; i++)
        {
            bottop[i] = (8, 1);
        }
        bottop = root.calcDiff(bottop);
        for (int i = 0; i < 8; i++)
        {
            Console.Write(i + ": " + bottop[i].Item1 + "-" + bottop[i].Item2 + " ... ");
            for (int j = 0; j < 8; j++)
            {
                Console.Write((j + 1) + ": " + root.opts[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.ReadLine();
    }
}

abstract class Tree
{
    public List<(int, int, int, List<int>)> data;
    public int[,] opts;
    protected Tree[] children;
    public abstract (int, int)[] calcDiff((int, int)[] bottop);
    public (int, int) match;
}

class Node : Tree
{
    public Node(List<(int, int, int)> played, List<(int, int)> matches, int count = 0)
    {
        data = new List<(int, int, int, List<int>)>();
        for (int i = 0; i < 8; i++)
        {
            data.Add((0, 0, 0, new List<int>()));
        }

        if (count == 28)
            return;
        match = matches[count];
        if (played.TrueForAll((x) => { return !(x.Item1 == match.Item1 && x.Item2 == match.Item2); }))
        {
            //match nog niet gespeeld
            //player 1 wint
            List<(int, int, int, List<int>)> clonedata = cloneData();
            (int, int, int, List<int>) p1 = clonedata[match.Item1];
            (int, int, int, List<int>) p2 = clonedata[match.Item2];

            //p1 heeft 1 extra win
            p1.Item1++;

            //p1 heeft tb2 extra van p2 wins
            p1.Item3 += p2.Item1;

            //p1 heeft p2 verslagen
            p1.Item4.Add(match.Item2);

            //iedereen met p1 in list heeft 1 extra tb2
            for (int i = 0; i < 8; i++)
            {
                if (clonedata[i].Item4.Contains(match.Item1))
                {
                    (int, int, int, List<int>) temp = clonedata[i];
                    temp.Item3++;
                    clonedata[i] = temp;
                }
            }

            clonedata[match.Item1] = p1;
            clonedata[match.Item2] = p2;

            Node p1win = new Node(played, matches, clonedata, count + 1);

            //player 2 wint
            clonedata = cloneData();
            p1 = clonedata[match.Item1];
            p2 = clonedata[match.Item2];

            //p2 heeft 1 extra win
            p2.Item1++;

            //p2 heeft tb2 extra van p1 wins
            p2.Item3 += p1.Item1;

            //p2 heeft p1 verslagen
            p2.Item4.Add(match.Item1);

            //iedereen met p2 in list heeft 1 extra tb2
            for (int i = 0; i < 8; i++)
            {
                if (clonedata[i].Item4.Contains(match.Item2))
                {
                    (int, int, int, List<int>) temp = clonedata[i];
                    temp.Item3++;
                    clonedata[i] = temp;
                }
            }

            clonedata[match.Item1] = p1;
            clonedata[match.Item2] = p2;

            Node p2win = new Node(played, matches, clonedata, count + 1);

            children = new Tree[2] { p1win, p2win };
        }
        else
        {
            // match al gespeeld
            List<(int, int, int, List<int>)> clonedata = cloneData();
            int winner = played.Contains((match.Item1, match.Item2, match.Item1)) ? match.Item1 : match.Item2;
            int loser = winner == match.Item1 ? match.Item2 : match.Item1;
            (int, int, int, List<int>) w = clonedata[winner];
            (int, int, int, List<int>) l = clonedata[loser];

            //w heeft 1 extra win
            w.Item1++;

            //w heeft tb2 extra van l wins
            w.Item3 += l.Item1;

            //w heeft l verslagen
            w.Item4.Add(loser);

            //iedereen met w in list heeft 1 extra tb2
            for (int i = 0; i < 8; i++)
            {
                if (clonedata[i].Item4.Contains(winner))
                {
                    (int, int, int, List<int>) temp = clonedata[i];
                    temp.Item3++;
                    clonedata[i] = temp;
                }
            }

            clonedata[match.Item1] = w;
            clonedata[match.Item2] = l;

            Node n = new Node(played, matches, clonedata, count + 1);

            children = new Tree[1] { n };
        }
    }

    public Node (List<(int, int, int)> played, List<(int, int)> matches, List<(int, int, int, List<int>)> data, int count = 0)
    {
        this.data = data;

        if (count == 28)
            return;
        match = matches[count];
        if (played.TrueForAll((x) => { return !(x.Item1 == match.Item1 && x.Item2 == match.Item2); }))
        {
            //match nog niet gespeeld
            //player 1 wint
            List<(int, int, int, List<int>)> clonedata = cloneData();
            (int, int, int, List<int>) p1 = clonedata[match.Item1];
            (int, int, int, List<int>) p2 = clonedata[match.Item2];

            //p1 heeft 1 extra win
            p1.Item1++;

            //p1 heeft tb2 extra van p2 wins
            p1.Item3 += p2.Item1;

            //p1 heeft p2 verslagen
            p1.Item4.Add(match.Item2);

            //iedereen met p1 in list heeft 1 extra tb2
            for (int i = 0; i < 8; i++)
            {
                if (clonedata[i].Item4.Contains(match.Item1))
                {
                    (int, int, int, List<int>) temp = clonedata[i];
                    temp.Item3++;
                    clonedata[i] = temp;
                }
            }

            clonedata[match.Item1] = p1;
            clonedata[match.Item2] = p2;

            Node p1win = new Node(played, matches, clonedata, count + 1);

            //player 2 wint
            clonedata = cloneData();
            p1 = clonedata[match.Item1];
            p2 = clonedata[match.Item2];

            //p2 heeft 1 extra win
            p2.Item1++;

            //p2 heeft tb2 extra van p1 wins
            p2.Item3 += p1.Item1;

            //p2 heeft p1 verslagen
            p2.Item4.Add(match.Item1);

            //iedereen met p2 in list heeft 1 extra tb2
            for (int i = 0; i < 8; i++)
            {
                if (clonedata[i].Item4.Contains(match.Item2))
                {
                    (int, int, int, List<int>) temp = clonedata[i];
                    temp.Item3++;
                    clonedata[i] = temp;
                }
            }

            clonedata[match.Item1] = p1;
            clonedata[match.Item2] = p2;

            Node p2win = new Node(played, matches, clonedata, count + 1);

            children = new Tree[2] { p1win, p2win };
        }
        else
        {
            // match al gespeeld
            List<(int, int, int, List<int>)> clonedata = cloneData();
            int winner = played.Contains((match.Item1, match.Item2, match.Item1)) ? match.Item1 : match.Item2;
            int loser = winner == match.Item1 ? match.Item2 : match.Item1;
            (int, int, int, List<int>) w = clonedata[winner];
            (int, int, int, List<int>) l = clonedata[loser];

            //w heeft 1 extra win
            w.Item1++;

            //w heeft tb2 extra van l wins
            w.Item3 += l.Item1;

            //w heeft l verslagen
            w.Item4.Add(loser);

            //iedereen met w in list heeft 1 extra tb2
            for (int i = 0; i < 8; i++)
            {
                if (clonedata[i].Item4.Contains(winner))
                {
                    (int, int, int, List<int>) temp = clonedata[i];
                    temp.Item3++;
                    clonedata[i] = temp;
                }
            }

            clonedata[winner] = w;
            clonedata[loser] = l;

            Node n = new Node(played, matches, clonedata, count + 1);

            children = new Tree[1] { n };
        }
    }

    private List<(int, int, int, List<int>)> cloneData()
    {
        List<(int, int, int, List<int>)> output = new List<(int, int, int, List<int>)>();
        foreach ((int, int, int, List<int>) tuple in data)
        {
            (int, int, int, List<int>) newtuple = (tuple.Item1, tuple.Item2, tuple.Item3, new List<int>(tuple.Item4.ToArray()));
            output.Add(newtuple);
        }
        return output;
    }

    public override (int, int)[] calcDiff((int, int)[] bottop)
    {
        (int, int)[] rtrn = ((int, int)[])bottop.Clone();
        if (children == null)
        {
            opts = new int[8, 8];
            List<(int, (int, int, int, List<int>))>[] tup = new List<(int, (int, int, int, List<int>))>[8];
            for (int wins = 0; wins < 8; wins++)
            {
                List<(int, (int, int, int, List<int>))> list = new List<(int, (int, int, int, List<int>))>();
                for (int player = 0; player < 8; player++)
                {
                    if (data[player].Item1 == wins)
                    {
                        list.Add((player, data[player]));
                    }
                }
                tup[wins] = list;
            }

            int pos = 1;
            for (int wins = 7; wins >= 0; wins--)
            {
                if (tup[wins].Count == 0)
                    continue;
                else if (tup[wins].Count == 1)
                {
                    rtrn[tup[wins][0].Item1] = (pos, pos);
                    opts[tup[wins][0].Item1, pos - 1]++;
                    pos++;
                }
                else if (tup[wins].Count == 2)
                {
                    if (tup[wins][0].Item2.Item4.Contains(tup[wins][1].Item1))
                    {
                        rtrn[tup[wins][0].Item1] = (pos, pos);
                        opts[tup[wins][0].Item1, pos - 1]++;
                        pos++;
                        rtrn[tup[wins][1].Item1] = (pos, pos);
                        opts[tup[wins][1].Item1, pos - 1]++;
                        pos++;
                    }
                    else
                    {
                        rtrn[tup[wins][1].Item1] = (pos, pos);
                        opts[tup[wins][1].Item1, pos - 1]++;
                        pos++;
                        rtrn[tup[wins][0].Item1] = (pos, pos);
                        opts[tup[wins][0].Item1, pos - 1]++;
                        pos++;
                    }
                }
                else
                {
                    tup[wins].Sort((a, b) =>
                    {
                        if (a.Item2.Item3 < b.Item2.Item3) return 1; if (a.Item2.Item3 < b.Item2.Item3) return -1;
                        return 0;
                    });
                    foreach ((int, (int, int, int, List<int>)) x in tup[wins])
                    {
                        rtrn[x.Item1] = (pos, pos);
                        opts[x.Item1, pos - 1]++;
                        pos++;
                    }
                }
            }
            return rtrn;
        }

        foreach (Tree t in children)
        {
            (int, int)[] temp = t.calcDiff(bottop);
            for (int i = 0; i < 8; i++)
            {
                if (temp[i].Item1 < rtrn[i].Item1)
                {
                    rtrn[i].Item1 = temp[i].Item1;
                    if (temp[i].Item1 == 1)
                        ;
                }

                if (temp[i].Item2 > rtrn[i].Item2)
                {
                    rtrn[i].Item2 = temp[i].Item2;
                    if (temp[i].Item2 == 8)
                        ;
                }
            }
        }

        opts = new int[8, 8];

        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                foreach (Node n in children)
                    opts[x, y] += n.opts[x,y];

        return rtrn;
    }
}
