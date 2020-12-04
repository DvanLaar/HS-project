using System;
using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Classes
{
    internal class Druid : Hero
    {
        public override Dictionary<Card, int> DeckList => new Dictionary<Card, int>();

        public Druid()
        {
            SetHeroPower();
        }

        public Druid(bool id, bool nw) : base(id, nw)
        {
            SetHeroPower();
        }

        private void SetHeroPower()
        {
            HeroPower = (b =>
            {
                if (ManaProtected < 2 || HeroPowerUsed)
                    return null;

                var cln = b.Clone();
                var own = cln.Me.Id == Id ? cln.Me : cln.Opp;
                own.Armor++;
                own.Attack++;
                own.Mana -= 2;

                SubBoardContainer Function(Board brd)
                {
                    var clone = brd.Clone();
                    (clone.Me.Id == Id ? clone.Me : clone.Opp).Attack--;
                    return new SingleSubBoardContainer(clone, brd, this + " Hero Power attack buff wears off");
                }

                own.EndTurnFuncs.Add(Function);
                own.SingleEndTurnFuncs.Add(own.EndTurnFuncs.IndexOf(Function));
                return new SingleSubBoardContainer(cln, b, "Use Hero Power");
            });
        }
    }
}