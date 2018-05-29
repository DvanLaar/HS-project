using System.Collections.Generic;

class Rogue : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Rogue() : base()
    {
        SetHeroPower();
    }

    public Rogue(bool id, bool nw) : base(id, nw)
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
            me.EquipWeapon(new WickedKnife());
            return new SingleSubBoardContainer(clone, b, "Use Hero Power");
        });
    }
}