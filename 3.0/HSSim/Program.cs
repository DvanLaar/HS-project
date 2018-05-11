using System;
using System.Collections.Generic;
using System.Timers;

class Program
{
    static void Main()
    {
        Hero me = new BasicMage();
        me.deck.Add((new ArcaneMissiles(), 2));
        me.deck.Add((new MurlocRaider(), 1));
        me.deck.Add((new ArcaneExplosion(), 2));
        me.deck.Add((new BloodfenRaptor(), 2));
        me.deck.Add((new NoviceEngineer(), 1));
        me.deck.Add((new RiverCrocolisk(), 2));
        me.deck.Add((new ArcaneIntellect(), 2));
        me.deck.Add((new RaidLeader(), 1));
        me.deck.Add((new Wolfrider(), 2));
        me.deck.Add((new Fireball(), 2));
        me.deck.Add((new OasisSnapjaw(), 2));
        me.deck.Add((new Polymorph(), 2));
        me.deck.Add((new SenjinShieldmasta(), 2));
        me.deck.Add((new Nightblade(), 1));
        me.deck.Add((new BoulderfistOgre(), 1));
        me.id = true;
        me.HeroPowerUsed = false;
        me.Health = 30;
        me.hand.Add(new NoviceEngineer() { owner = me });
        me.hand.Add(new Nightblade() { owner = me });
        me.hand.Add(new RaidLeader() { owner = me });
        me.hand.Add(new Coin() { owner = me });
        me.hand.Add(new BoulderfistOgre() { owner = me });
        me.hand.Add(new MurlocRaider() { owner = me });

        Hero opp = new BasicMage();
        opp.id = false;
        for (int i = 0; i < 3; i++)
            opp.hand.Add(new Sheep());
        opp.deck.Add((new Sheep(), 26));
        opp.Health = 30;
        opp.Mana = 0;
        opp.maxMana = 0;
        opp.onBoard.Add(new MurlocRaider() { owner = opp });

        Board b = new Board(me, opp);
        b.curr = me;
        me.Mana = 1;
        me.maxMana = 1;

        foreach((Card, int) kvp in me.deck)
        {
            kvp.Item1.SetOwner(me);
        }
        foreach ((Card, int) ci in me.deck.FindAll((a) => me.hand.Contains(a.Item1)))
        {
            me.deck[me.deck.IndexOf(ci)] = (ci.Item1, ci.Item2 - 1);
        }

        Queue<BoardContainer> boards = new Queue<BoardContainer>();
        List<BoardContainer> seen = new List<BoardContainer>();
        boards.Enqueue(new SingleBoardContainer(b.Clone(), ""));

        Timer tmr = new Timer(10000);
        bool running = true;
        tmr.Elapsed += (o, e) => { running = false; };

        while (running)
        {
            BoardContainer current = boards.Dequeue();
            seen.Add(current);

            List<BoardContainer> succ = Algorithm.getSuc(current);
            succ.Sort((a, c) => c.value.CompareTo(a.value));

            foreach (BoardContainer bc in succ)
                boards.Enqueue(bc);


        }
     }
}