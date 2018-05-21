

class IronforgeRifleman : BattlecryMinion
{
    public IronforgeRifleman() : base(3, 2, 2)
    {
        SetBattlecry((b) =>
        {
            return DealDamage(1, b);
        });
    }
}