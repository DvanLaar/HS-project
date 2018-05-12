using System;
using System.Collections.Generic;
using System.Timers;

class Program
{
    static void Main()
    {
        Hero me = new BasicMage();
        me.deck.Add(new ArcaneMissiles(), 2);
        me.deck.Add(new MurlocRaider(), 1);
        //me.deck.Add(new ArcaneExplosion(), 2);
        me.deck.Add(new BloodfenRaptor(), 2);
        //me.deck.Add(new NoviceEngineer(), 2);
        me.deck.Add(new RiverCrocolisk(), 1);
        me.deck.Add(new ArcaneIntellect(), 2);
        me.deck.Add(new RaidLeader(), 2);
        me.deck.Add(new Wolfrider(), 1);
        me.deck.Add(new Fireball(), 2);
        //me.deck.Add(new OasisSnapjaw(), 2);
        me.deck.Add(new Polymorph(), 1);
        me.deck.Add(new SenjinShieldmasta(), 1);
        //me.deck.Add(new Nightblade(), 2);
        me.deck.Add(new BoulderfistOgre(), 2);
        me.id = true;
        me.HeroPowerUsed = false;
        me.Health = 25;
        me.hand.Add(new Wolfrider() { owner = me });
        me.hand.Add(new ArcaneExplosion() { owner = me });
        me.hand.Add(new NoviceEngineer() { owner = me });
        me.hand.Add(new Nightblade() { owner = me });

        me.onBoard.Add(new OasisSnapjaw() { owner = me, AttacksLeft = 1, Health = 3 });
        me.onBoard.Add(new SenjinShieldmasta() { owner = me, AttacksLeft = 1, Health = 2 });
        me.onBoard.Add(new OasisSnapjaw() { owner = me, AttacksLeft = 1, Health = 7 });
        me.onBoard.Add(new RiverCrocolisk() { owner = me, AttacksLeft = 1, Health = 2 });

        Hero opp = new BasicMage();
        opp.id = false;
        for (int i = 0; i < 5; i++)
            opp.hand.Add(new Sheep());
        opp.deck.Add(new Sheep(), 14);
        opp.Health = 10;
        opp.Mana = 8;
        opp.maxMana = 8;

        opp.onBoard.Add(new BoulderfistOgre() { owner = opp });

        Board b = new Board(me, opp);
        b.curr = me;
        me.Mana = 9;
        me.maxMana = 9;

        foreach(KeyValuePair<Card, int> kvp in me.deck)
        {
            kvp.Key.SetOwner(me);
        }

        bool running = true;
        Timer tmr = new Timer(60000);
        tmr.Elapsed += (o, e) => { running = false; };
        tmr.Start();

        MinHeap mh = new MinHeap(1000000);
        MasterBoardContainer first = new MasterBoardContainer(b);
        mh.Insert(first);

        while (running && !mh.Empty())
        {
            MasterBoardContainer mbc = mh.MinimumExtract();
            if (mbc.board.curr.id != me.id)
                continue;
            mbc.Expand();

            foreach (SubBoardContainer sbc in mbc.children)
                foreach (MasterBoardContainer newMbc in sbc.children)
                    mh.Insert(newMbc);
        }

        first.Sort();
     }
}