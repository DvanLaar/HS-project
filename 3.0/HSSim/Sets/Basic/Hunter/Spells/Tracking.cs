using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Hunter.Spells
{
    internal class Tracking : Spell
    {
        public Tracking() : base(1)
        {
            SetSpell(b =>
            {
                var result = new List<(List<MasterBoardContainer>, int, string)>();
                var seen = new List<Card>();
                foreach (var c in Owner.Deck)
                {
                    var seen2 = new List<Card>();
                    var cln = b.Clone();
                    var me = Owner.Id == cln.Me.Id ? cln.Me : cln.Opp;

                    me.Deck[c.Key]--;
                    if (me.Deck[c.Key] == 0)
                        me.Deck.Remove(c.Key);

                    foreach (var c2 in me.Deck)
                    {
                        if (seen.Contains(c2.Key))
                            continue;

                        var cln2 = cln.Clone();
                        var me2 = Owner.Id == cln2.Me.Id ? cln2.Me : cln2.Opp;

                        me2.Deck[c2.Key]--;
                        if (me2.Deck[c2.Key] == 0)
                            me2.Deck.Remove(c2.Key);

                        foreach (var c3 in me2.Deck)
                        {
                            if (seen2.Contains(c3.Key) || seen.Contains(c3.Key))
                                continue;

                            var cln3 = cln.Clone();
                            var me3 = Owner.Id == cln3.Me.Id ? cln3.Me : cln3.Opp;

                            me3.Deck[c3.Key]--;
                            if (me3.Deck[c3.Key] == 0)
                                me3.Deck.Remove(c3.Key);

                            var res1 = cln3.Clone();
                            (res1.Me.Id == Owner.Id ? res1.Me : res1.Opp).Hand.Add(c.Key);
                            var mbc1 = new MasterBoardContainer(res1) { Action = "Pick " + c.Key };

                            var res2 = cln3.Clone();
                            (res2.Me.Id == Owner.Id ? res2.Me : res2.Opp).Hand.Add(c2.Key);
                            var mbc2 = new MasterBoardContainer(res2) { Action = "Pick " + c2.Key };

                            var res3 = cln3.Clone();
                            (res3.Me.Id == Owner.Id ? res3.Me : res3.Opp).Hand.Add(c3.Key);
                            var mbc3 = new MasterBoardContainer(res3) { Action = "Pick " + c3.Key };

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
}