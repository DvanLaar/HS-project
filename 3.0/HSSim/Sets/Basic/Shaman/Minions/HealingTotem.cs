

class HealingTotem : EndOfTurnMinion
{
    public HealingTotem() : base(1, 0, 2)
    {
        Totem = true;
    }

    protected override SubBoardContainer EoTEffect(Board b)
    {
        Board cln = b.Clone();
        Hero own = owner.id == cln.me.id ? cln.me : cln.opp;
        foreach (Minion m in own.onBoard)
            m.Health += 1;
        return new SingleSubBoardContainer(cln, b, this + " heals");
    }
}