

class MurlocTidehunter : BattlecryMinion
{
    public MurlocTidehunter() : base(2, 2, 1)
    {
        SetBattlecry((b) =>
        {
            Board cln = b.Clone();
            Hero me = cln.me.id == owner.id ? cln.me : cln.opp;
            me.StartSummon(new MurlocScout());
            return new SingleSubBoardContainer(cln, b, "Play " + this);
        });
    }
}