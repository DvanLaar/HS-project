using System.Collections.Generic;

class AncestralHealing : Spell
{
    public AncestralHealing() : base(0)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> results = new List<MasterBoardContainer>();
            foreach (Minion m in b.me.onBoard)
            {
                Board clone = b.Clone();
                Hero hero = clone.me;
                Minion min = hero.onBoard[b.me.onBoard.IndexOf(m)];
                min.Heal(min.maxHealth);
                min.Taunt = true;
                results.Add(new MasterBoardContainer(clone) { action = "Target " + min });
            }
            foreach (Minion m in b.opp.onBoard)
            {
                Board clone = b.Clone();
                Hero hero = clone.opp;
                Minion min = hero.onBoard[b.opp.onBoard.IndexOf(m)];
                min.Heal(min.maxHealth);
                min.Taunt = true;
                results.Add(new MasterBoardContainer(clone) { action = "Target " + min });
            }

            return new ChoiceSubBoardContainer(results, b, "Play " + this);
        });
    }

    public override bool CanPlay(Board b)
    {
        return base.CanPlay(b) && (b.me.onBoard.Count + b.opp.onBoard.Count != 0);
    }
}