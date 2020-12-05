using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using HSSim.Abstract_Cards;
using HSSim.Decks.Hunter;
using HSSim.Sets.Basic.Neutral.Spells;

namespace HSSim
{
    internal static class Program
    {
        private static void Main()
        {
            Hero me = new BasicHunter(true, true);
            Hero opp = new BasicHunter();
            opp.Id = false;

            var b = new Board(me, opp) {Curr = true};

            foreach(var kvp in me.Deck)
            {
                kvp.Key.SetOwner(me);
            }

            for (var i = 0; i < 3; i++)
            {
                Console.WriteLine("What cards did you draw?");
                var j = 0;
                foreach (var c in b.Me.Deck.Keys)
                {
                    Console.WriteLine(j + ": " + c);
                    j++;
                }
                var index = int.Parse(Console.ReadLine() ?? string.Empty);
                Console.Clear();
                j = 0;
                foreach (var c in b.Me.Deck.Keys)
                {
                    if (j == index)
                    {
                        b = b.Me.DrawCard(b, c).Board;
                        break;
                    }
                    j++;
                }
            }

            var uc = new UnknownCard(b.Opp.DeckList) { Owner = opp };
            b.Opp.Deck.Add(uc, 30);

            for (var i = 0; i < 4; i++)
                b = b.Opp.DrawCard(b, uc).Board;
            
            b.Opp.Hand.Add(new Coin { Owner = b.Opp });

            b.Me.StartTurn(b);
            var first = new MasterBoardContainer(b);

            while (true)
            {
                b = first.Board;

                Console.WriteLine("What card did you draw?");
                var j = 0;
                foreach (var c in b.Me.Deck.Keys)
                {
                    Console.WriteLine(j + ": " + c);
                    j++;
                }
                var index = int.Parse(Console.ReadLine());
                Console.Clear();
                j = 0;
                foreach (var c in b.Me.Deck.Keys)
                {
                    if (j == index)
                    {
                        b = b.Me.DrawCard(b, c).Board;
                        break;
                    }
                    j++;
                }

                //b.me.maxMana += 1;
                //b.me.Mana = b.me.maxMana;
                //foreach (Minion m in b.me.onBoard)
                //    m.AttacksLeft = m.maxAttacks;
                //b.me.AttacksLeft = 1;
                //b.me.HeroPowerUsed = false;

                var running = true;
                var tmr = new Timer(20000);
                tmr.Elapsed += (o, e) => { running = false; };
                var ownStatesVisited = 0;

                var mh = new MinHeap(1000000, false);
                var turnEnded = new List<MasterBoardContainer>();
                first = new MasterBoardContainer(b);
                mh.Insert(first);

                tmr.Start();
                while (running && !mh.Empty())
                {
                    var mbc = mh.MinimumExtract();
                    if (mbc.Board.Curr != me.Id)
                    {
                        mbc.Board.Me.EndTurn(mbc.Board);
                        turnEnded.Add(mbc);
                        continue;
                    }
                    mbc.Expand();
                    ownStatesVisited++;

                    foreach (var newMbc in mbc.Children.SelectMany(sbc => sbc.Children))
                        mh.Insert(newMbc);
                }

                turnEnded.Sort((a1, a2) => a1.Value.CompareTo(a2.Value));

                var minHeapOpponent = new MinHeap(1000000, true);
                for (var i = 0; i < turnEnded.Count; i++)
                {
                    turnEnded[i].Board.Opp.EndTurn(turnEnded[i].Board);
                    if (i < 10)
                        minHeapOpponent.Insert(turnEnded[i]);
                }

                var tmr2 = new Timer(20000);
                var running2 = true;
                tmr2.Elapsed += (o, a) => running2 = false;

                tmr2.Start();
                while (running2 && !minHeapOpponent.Empty())
                {
                    var mbc = minHeapOpponent.MinimumExtract();
                    if (mbc.Board.Curr != opp.Id)
                    {
                        //turnEnded.Add(mbc);
                        mbc.Board.Opp.EndTurn(mbc.Board);
                        continue;
                    }
                    mbc.Expand();

                    foreach (var sbc in mbc.Children)
                    foreach (var newMbc in sbc.Children)
                        minHeapOpponent.Insert(newMbc);
                }

                //first.Sort();
                Console.WriteLine(ownStatesVisited);

                while (first.Board.Curr)
                {
                    if (first.Children.Count == 0)
                        first.Expand();
                    Console.WriteLine(first.Board.Opp.Health);
                    Console.WriteLine(first.Board.Opp.Armor);
                    var sbc = first.Children[0];
                    Console.WriteLine(sbc.Action);
                    var resultingIndex = 0;
                    if (sbc.GetType() == typeof(RandomSubBoardContainer) || sbc.GetType() == typeof(UnknownSubBoardContainer))
                    {
                        for (var i = 0; i < sbc.Children.Count; i++)
                        {
                            Console.WriteLine(i + ": " + sbc.Children[i].Action);
                        }
                        resultingIndex = int.Parse(Console.ReadLine());
                    }
                    else if (sbc.GetType() == typeof(ChoiceSubBoardContainer))
                    {
                        Console.WriteLine(sbc.Children[0].Action);
                        Console.ReadKey();
                    }
                    else if (sbc.GetType() == typeof(RandomChoiceSubBoardContainer))
                    {
                        var rcsbc = (RandomChoiceSubBoardContainer)sbc;
                        for (var i = 0; i < rcsbc.Options.Count; i++)
                        {
                            Console.WriteLine(i + ": " + rcsbc.Options[i].Item3);
                        }
                        var thisIndex = int.Parse(Console.ReadLine());
                        first = rcsbc.Options[thisIndex].Item1[0];
                        Console.Clear();
                        Console.WriteLine(rcsbc.Options[thisIndex].Item1[0].Action);
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    else
                        Console.ReadKey();
                    Console.Clear();

                    first = sbc.Children[resultingIndex];
                }
                while (!first.Board.Curr)
                {
                    if (first.Children.Count == 0)
                        first.Expand();
                    Console.WriteLine("What did your opponent do?");
                    for (var i = 0; i < first.Children.Count; i++)
                    {
                        Console.WriteLine(i + ": " + first.Children[i].Action);
                    }
                    var resIndex = int.Parse(Console.ReadLine());
                    var sbc = first.Children[resIndex];
                    Console.Clear();
                    if (sbc.Children.Count == 1)
                    {
                        first = sbc.Children[0];
                        continue;
                    }
                    Console.WriteLine("What was the result?");
                    for (var i = 0; i < sbc.Children.Count; i++)
                    {
                        Console.WriteLine(i + ": " + sbc.Children[i].Action);
                    }
                    resIndex = int.Parse(Console.ReadLine());
                    first = sbc.Children[resIndex];
                    Console.Clear();
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}