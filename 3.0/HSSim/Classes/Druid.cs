using System;
using System.Collections.Generic;

class Druid : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Druid() : base()
    {

    }

    public Druid(bool id, bool nw) : base(id, nw)
    {

    }

    private void SetHeroPower()
    {
        HeroPower = ((b) =>
        {
            if (mana < 2 || HeroPowerUsed)
                return null;

            Board cln = b.Clone();
            Hero own = cln.me.id == id ? cln.me : cln.opp;
            own.Armor++;
            own.Attack++;
            own.Mana -= 2;
            Func<Board, SubBoardContainer> function = (brd) => { Board clone = brd.Clone(); (clone.me.id == id ? clone.me : clone.opp).Attack--; return new SingleSubBoardContainer(clone, brd, this + " Hero Power attack buff wears off"); };
            own.EndTurnFuncs.Add(function);
            own.SingleEndTurnFuncs.Add(own.EndTurnFuncs.IndexOf(function));
            return new SingleSubBoardContainer(cln, b, "Use Hero Power");
        });
    }
}