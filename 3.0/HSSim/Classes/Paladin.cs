using System.Collections.Generic;

class Paladin : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Paladin() : base()
    {
        SetHeroPower();
    }

    public Paladin(bool id, bool nw) : base()
    {
        SetHeroPower();
    }

    private void SetHeroPower()
    {
        HeroPower = ((b) =>
        {
            if (Mana < 2 || HeroPowerUsed || onBoard.Count >= 7)
                return null;

            Board clone = b.Clone();
            Hero me = id == clone.me.id ? clone.me : clone.opp;
            me.StartSummon(new SilverHandRecruit());
            me.Mana -= 2;
            return new SingleSubBoardContainer(clone, b, "Use Hero Power");
        });
    }
}