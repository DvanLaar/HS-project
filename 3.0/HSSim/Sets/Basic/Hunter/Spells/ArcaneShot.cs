

class ArcaneShot : Spell
{
    public ArcaneShot() : base(1)
    {
        SetSpell((b) =>
        {
            return DealDamage(2, b);
        });
    }
}