class RazorfenHunter : BattlecryMinion
{
    public RazorfenHunter() : base(3, 2, 3)
    {
        SetBattlecry((b) =>
        {
            Board clone = b.Clone();
            (clone.me.id == owner.id ? clone.me : clone.opp).StartSummon(new Boar() { owner = owner });
            return new SingleSubBoardContainer(clone, b, "Play " + this);
        });
    }
}