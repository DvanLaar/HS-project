using System;

class Nightblade : BattlecryMinion 
{
    public Nightblade() : base(5, 4, 4)
    {
        SetBattlecry((b) =>
        {
            Board clone = b.Clone();
            Hero OppClone = owner.id != clone.me.id ? clone.me : clone.opp;
            OppClone.TakeDamage(3);
            return new SingleSubBoardContainer(clone, b, "Play Nightblade");
        });
    }
}