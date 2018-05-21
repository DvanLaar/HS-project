using System.Collections.Generic;

class Tracking : Spell
{
    public Tracking() : base(1)
    {
        SetSpell((b) =>
        {
            List<(List<MasterBoardContainer>, int, string)> result = new List<(List<MasterBoardContainer>, int, string)>();
            List<Card> seen = new List<Card>();
            foreach (KeyValuePair<Card, int> c in owner.deck)
            {
                List<Card> seen2 = new List<Card>();
                Board cln = b.Clone();
                Hero me = owner.id == cln.me.id ? cln.me : cln.opp;

                me.deck[c.Key]--;
                if (me.deck[c.Key] == 0)
                    me.deck.Remove(c.Key);

                foreach (KeyValuePair<Card, int> c2 in me.deck)
                {
                    if (seen.Contains(c2.Key))
                        continue;

                    Board cln2 = cln.Clone();
                    Hero me2 = owner.id == cln2.me.id ? cln2.me : cln2.opp;

                    me2.deck[c2.Key]--;
                    if (me2.deck[c2.Key] == 0)
                        me2.deck.Remove(c2.Key);

                    foreach (KeyValuePair<Card, int> c3 in me2.deck)
                    {
                        if (seen2.Contains(c3.Key) || seen.Contains(c3.Key))
                            continue;

                        Board cln3 = cln.Clone();
                        Hero me3 = owner.id == cln3.me.id ? cln3.me : cln3.opp;

                        me3.deck[c3.Key]--;
                        if (me3.deck[c3.Key] == 0)
                            me3.deck.Remove(c3.Key);

                        Board res1 = cln3.Clone();
                        (res1.me.id == owner.id ? res1.me : res1.opp).hand.Add(c.Key);
                        MasterBoardContainer mbc1 = new MasterBoardContainer(res1) { action = "Pick " + c.Key };

                        Board res2 = cln3.Clone();
                        (res2.me.id == owner.id ? res2.me : res2.opp).hand.Add(c2.Key);
                        MasterBoardContainer mbc2 = new MasterBoardContainer(res2) { action = "Pick " + c2.Key };

                        Board res3 = cln3.Clone();
                        (res3.me.id == owner.id ? res3.me : res3.opp).hand.Add(c3.Key);
                        MasterBoardContainer mbc3 = new MasterBoardContainer(res3) { action = "Pick " + c3.Key };

                        result.Add((new List<MasterBoardContainer> { mbc1, mbc2, mbc3 }, c.Value * c2.Value * c3.Value, c + " + " + c2 + " + " + c3));
                    }
                    seen2.Add(c2.Key);
                }
                seen.Add(c.Key);
            }

            return new RandomChoiceSubBoardContainer(result, b, "Play " + this);
        });
    }
}