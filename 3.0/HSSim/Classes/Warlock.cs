using System.Collections.Generic;

class Warlock : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Warlock() : base()
    {
        SetHeroPower();
    }

    public Warlock(bool id, bool nw) : base(id, nw)
    {
        SetHeroPower();
    }

    private void SetHeroPower()
    {
        HeroPower = ((b) =>
        {
            if (Mana < 2 || HeroPowerUsed)
                return null;

            Board clone = b.Clone();
            Hero me = id == clone.me.id ? clone.me : clone.opp;
            me.Mana -= 2;
            SubBoardContainer sbc = me.DrawOneCard(clone);
            foreach (MasterBoardContainer mbc in sbc.children)
                (mbc.board.me.id == id ? mbc.board.me : mbc.board.opp).Health -= 2;
            return sbc;
        });
    }
}