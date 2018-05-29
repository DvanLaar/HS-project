

abstract class EndOfTurnMinion : Minion
{
    public EndOfTurnMinion(int mana, int attack, int health) : base(mana, attack, health)
    {
        Transform += RemoveEffect;
    }

    public override void SetOwner(Hero owner)
    {
        base.SetOwner(owner);
        owner.Summon += (m) => { if (m == this) AddEffect(); };
    }

    protected abstract SubBoardContainer EoTEffect(Board b);

    public void AddEffect()
    {
        owner.EndTurnFuncs.Add(EoTEffect);
    }

    public void RemoveEffect()
    {
        owner.EndTurnFuncs.Remove(EoTEffect);
    }

}