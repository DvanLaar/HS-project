using System.Collections.Generic;

class Hunter : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Hunter() : base()
    {
        SetHeropower();
    }

    public Hunter(bool id, bool nw) : base(id, nw)
    {
        SetHeropower();
    }

    private void SetHeropower()
    {
        HeroPower = (b) =>
        {
            if (mana < 2 || HeroPowerUsed)
                return null;

            Board cln = b.Clone();
            (cln.me.id == id ? cln.opp : cln.me).TakeDamage(2);
            (cln.me.id == id ? cln.me : cln.opp).Mana -= 2;
            (cln.me.id == id ? cln.me : cln.opp).HeroPowerUsed = true;
            return new SingleSubBoardContainer(cln, b, "Use Hero Power");
        };
    }
}