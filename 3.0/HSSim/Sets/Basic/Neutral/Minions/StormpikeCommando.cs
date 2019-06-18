class StormpikeCommando : BattlecryMinion
{
    public StormpikeCommando() : base(5, 4, 2)
    {
        SetBattlecry((b) =>
        {
            return DealDamage(2, b);
        });
    }
}