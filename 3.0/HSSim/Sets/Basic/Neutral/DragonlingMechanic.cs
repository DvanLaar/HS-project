

class DragonlingMechanic : BattlecryMinion
{
    public DragonlingMechanic() : base(4, 2, 4)
    {
        SetBattlecry((b) =>
        {
            Board clone = b.Clone();
            Hero me = owner.id == clone.me.id ? clone.me : clone.opp;
            me.StartSummon(new MechanicalDragonling());
            return new SingleSubBoardContainer(clone, b, "Play " + this);
        });
    }
}