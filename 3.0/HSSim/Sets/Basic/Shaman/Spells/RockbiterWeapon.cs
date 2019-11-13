using System.Collections.Generic;

class RockbiterWeapon : Spell
{
    public RockbiterWeapon() : base(2)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> results = new List<MasterBoardContainer>();
            Hero me = owner.id == b.me.id ? b.me : b.opp;
            foreach (Minion m in me.onBoard)
            {
                Board clone = b.Clone();
                Hero me2 = owner.id == clone.me.id ? clone.me : clone.opp;
                Minion target = me2.onBoard[me.onBoard.IndexOf(m)];
                target.AlterAttack(3);
                owner.EndTurnFuncs.Add((b2) =>
                {
                    target.AlterAttack(-3);

                });
            }
        });
    }
}