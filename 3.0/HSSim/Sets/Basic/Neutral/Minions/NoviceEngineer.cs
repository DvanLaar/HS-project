using System;

class NoviceEngineer : BattlecryMinion
{
    public NoviceEngineer() : base(2, 1, 1)
    {
        SetBattlecry((b) =>
        {
            return owner.DrawOneCard(b);
        });
    }
}