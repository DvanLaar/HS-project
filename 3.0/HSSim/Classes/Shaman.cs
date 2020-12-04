using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Abstract_Cards.Minions;
using HSSim.Sets.Basic.Shaman.Tokens;

namespace HSSim.Classes
{
    internal class Shaman : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        public Shaman()
        {
            SetHeroPower();
        }

        public Shaman(bool id, bool nw) : base(id, nw)
        {
            SetHeroPower();
        }

        private void SetHeroPower()
        {
            HeroPower = (b =>
            {
                if (HeroPowerUsed || Mana < 2 || OnBoard.Count >= 7)
                    return null;

                var totems = new Minion[] {new HealingTotem(), new SearingTotem(), new StoneclawTotem(), new WrathOfAirTotem() };
                var result = new List<(MasterBoardContainer, int)>();
                foreach (var m in totems)
                {
                    var clone = b.Clone();
                    var own = Id == clone.Me.Id ? clone.Me : clone.Opp;
                    m.SetOwner(own);
                    if (own.OnBoard.TrueForAll(comp => comp.GetType() != m.GetType()))
                    {
                        own.Mana -= 2;
                        own.StartSummon(m);
                        result.Add((new MasterBoardContainer(clone) { Action = "Summon " + m }, 1));
                    }
                }
                return result.Count == 0 ? null : new RandomSubBoardContainer(result, b, "Use Hero Power");
            });
        }
    }
}