using System;

class Coin : Spell
{
    public Coin() : base(0)
    {
        SetSpell((b) =>
        {
            Board clone = b.Clone();
            Hero ownerClone = b.me.id == owner.id ? b.me : b.opp;
            ownerClone.Mana -= cost;
            ownerClone.Mana += 1;
            return new SingleBoardContainer(clone, "Play Coin");
        });
    }

}