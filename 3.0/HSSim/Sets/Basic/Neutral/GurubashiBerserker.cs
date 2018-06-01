

class GurubashiBerserker : Minion
{
    public GurubashiBerserker() : base(5, 2, 7)
    {
        OnDamaged += () =>
        {
            AlterAttack(3);
        };
    }
}