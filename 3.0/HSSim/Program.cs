using System;
using System.Collections.Generic;
using System.Timers;

class Program
{
    static void Main()
    {
        Hero me = new BasicHunter(true, true);

        Hero opp = new BasicHunter();
        opp.id = false;

        Board b = new Board(me, opp);
        b.curr = true;

        foreach(KeyValuePair<Card, int> kvp in me.deck)
        {
            kvp.Key.SetOwner(me);
        }

        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("What cards did you draw?");
            int j = 0;
            foreach (Card c in b.me.deck.Keys)
            {
                Console.WriteLine(j + ": " + c);
                j++;
            }
            int index = int.Parse(Console.ReadLine());
            Console.Clear();
            j = 0;
            foreach (Card c in b.me.deck.Keys)
            {
                if (j == index)
                {
                    b = b.me.DrawCard(b, c).board;
                    break;
                }
                j++;
            }
        }

        UnknownCard uc = new UnknownCard(b.opp.DeckList) { owner = opp };
        b.opp.deck.Add(uc, 30);

        for (int i = 0; i < 4; i++)
            b = b.opp.DrawCard(b, uc).board;
            
        b.opp.hand.Add(new Coin { owner = b.opp });

        MasterBoardContainer first = new MasterBoardContainer(b);

        while (true)
        {
            b = first.board;

            Console.WriteLine("What card did you draw?");
            int j = 0;
            foreach (Card c in b.me.deck.Keys)
            {
                Console.WriteLine(j + ": " + c);
                j++;
            }
            int index = int.Parse(Console.ReadLine());
            Console.Clear();
            j = 0;
            foreach (Card c in b.me.deck.Keys)
            {
                if (j == index)
                {
                    b = b.me.DrawCard(b, c).board;
                    break;
                }
                j++;
            }

            b.me.maxMana += 1;
            b.me.Mana = b.me.maxMana;
            foreach (Minion m in b.me.onBoard)
                m.AttacksLeft = m.maxAttacks;
            b.me.HeroPowerUsed = false;

            bool running = true;
            Timer tmr = new Timer(20000);
            tmr.Elapsed += (o, e) => { running = false; };
            int ownstatesvisited = 0;

            MinHeap mh = new MinHeap(1000000, false);
            List<MasterBoardContainer> turnEnded = new List<MasterBoardContainer>();
            first = new MasterBoardContainer(b);
            mh.Insert(first);

            tmr.Start();
            while (running && !mh.Empty())
            {
                MasterBoardContainer mbc = mh.MinimumExtract();
                if (mbc.board.curr != me.id)
                {
                    turnEnded.Add(mbc);
                    continue;
                }
                mbc.Expand();
                ownstatesvisited++;

                foreach (SubBoardContainer sbc in mbc.children)
                    foreach (MasterBoardContainer newMbc in sbc.children)
                        mh.Insert(newMbc);
            }

            turnEnded.Sort((a1, a2) => a1.value.CompareTo(a2.value));

            MinHeap mhopp = new MinHeap(1000000, true);
            for (int i = 0; i < turnEnded.Count; i++)
            {
                turnEnded[i].board.opp.maxMana += 1;
                turnEnded[i].board.opp.Mana = turnEnded[i].board.opp.maxMana;
                foreach (Minion m in turnEnded[i].board.opp.onBoard)
                    m.AttacksLeft = m.maxAttacks;
                foreach (Card c in turnEnded[i].board.opp.deck.Keys)
                    turnEnded[i].board = turnEnded[i].board.opp.DrawCard(turnEnded[i].board, c).board;
                turnEnded[i].board.opp.HeroPowerUsed = false;
                if (i < 10)
                    mhopp.Insert(turnEnded[i]);
            }

            Timer tmr2 = new Timer(20000);
            bool running2 = true;
            tmr2.Elapsed += (o, a) => running2 = false;

            tmr2.Start();
            while (running2 && !mhopp.Empty())
            {
                MasterBoardContainer mbc = mhopp.MinimumExtract();
                if (mbc.board.curr != opp.id)
                {
                    //turnEnded.Add(mbc);
                    continue;
                }
                mbc.Expand();

                foreach (SubBoardContainer sbc in mbc.children)
                    foreach (MasterBoardContainer newMbc in sbc.children)
                        mhopp.Insert(newMbc);
            }

            first.Sort();

            Console.WriteLine(ownstatesvisited);

            while (first.board.curr)
            {
                if (first.children.Count == 0)
                    first.Expand();
                SubBoardContainer sbc = first.children[0];
                Console.WriteLine(sbc.action);
                int resultingIndex = 0;
                if (sbc.GetType() == typeof(RandomSubBoardContainer) || sbc.GetType() == typeof(UnknownSubBoardContainer))
                {
                    //Console.WriteLine("Iets met een random effect! Hier ga ik opties geven en vragen wat er is gebeurd");
                    for (int i = 0; i < sbc.children.Count; i++)
                    {
                        Console.WriteLine(i + ": " + sbc.children[i].action);
                    }
                    resultingIndex = int.Parse(Console.ReadLine());
                }
                else if (sbc.GetType() == typeof(ChoiceSubBoardContainer))
                {
                    Console.WriteLine(sbc.children[0].action);
                    Console.ReadKey();
                }
                else if (sbc.GetType() == typeof(RandomChoiceSubBoardContainer))
                {
                    RandomChoiceSubBoardContainer RCSBC = (RandomChoiceSubBoardContainer)sbc;
                    for (int i = 0; i < RCSBC.options.Count; i++)
                    {
                        Console.WriteLine(i + ": " + RCSBC.options[i].Item3);
                    }
                    int thisIndex = int.Parse(Console.ReadLine());
                    first = RCSBC.options[thisIndex].Item1[0];
                    Console.Clear();
                    Console.WriteLine(RCSBC.options[thisIndex].Item1[0].action);
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else
                    Console.ReadKey();
                Console.Clear();

                first = sbc.children[resultingIndex];
            }
            while (!first.board.curr)
            {
                if (first.children.Count == 0)
                    first.Expand();
                Console.WriteLine("What did your opponent do?");
                for (int i = 0; i < first.children.Count; i++)
                {
                    Console.WriteLine(i + ": " + first.children[i].action);
                }
                int resIndex = int.Parse(Console.ReadLine());
                SubBoardContainer sbc = first.children[resIndex];
                Console.Clear();
                if (sbc.children.Count == 1)
                {
                    first = sbc.children[0];
                    continue;
                }
                Console.WriteLine("What was the result?");
                for (int i = 0; i < sbc.children.Count; i++)
                {
                    Console.WriteLine(i + ": " + sbc.children[i].action);
                }
                resIndex = int.Parse(Console.ReadLine());
                first = sbc.children[resIndex];
                Console.Clear();
            }
        }
     }
}