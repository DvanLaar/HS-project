

class WrathOfAirTotem : Minion
{
    public WrathOfAirTotem() : base(1, 0, 2)
    {
        Totem = true;
        owner.Summon += (m) => { if (m == this) { owner.SpellDamage++; m.Transform += () => owner.SpellDamage--; m.Destroy += () => owner.SpellDamage--; } };
    }

    public override string ToString()
    {
        return "Wrath of Air Totem";
    }
}