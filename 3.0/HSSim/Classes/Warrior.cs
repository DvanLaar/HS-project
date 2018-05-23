using System.Collections.Generic;

class Warrior : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Warrior() : base()
    {

    }
    public Warrior(bool id, bool nw) : base(id, nw)
    {

    }

    private void SetHeropower()
    {
        HeroPower = ((b) =>
        {
            if (mana < 2 || HeroPowerUsed)
                return null;

            Board cln = b.Clone();
            Hero me = cln.me.id == id ? cln.me : cln.opp;
            me.Armor += 2;
            me.Mana -= 2;
            me.HeroPowerUsed = true;
            return new SingleSubBoardContainer(cln, b, "Use Hero Power");
        });
    }
}