using System.Collections.Generic;

class Houndmaster : BattlecryMinion
{
    public Houndmaster() : base(4, 4, 3)
    {
        SetBattlecry((b) =>
        {
            List<Minion> beasts = new List<Minion>();
            foreach (Minion m in owner.onBoard)
                if (m.Beast)
                    beasts.Add(m);

            if (beasts.Count == 0)
                return new SingleSubBoardContainer(b, b, "Play " + this);

            List<MasterBoardContainer> result = new List<MasterBoardContainer>();

            foreach (Minion m in beasts)
            {
                Board cln = b.Clone();
                Hero me = cln.me.id == owner.id ? cln.me : cln.opp;
                Minion target = me.onBoard[owner.onBoard.IndexOf(m)];
                target.AlterAttack(2);
                target.AddHealth(2);
                target.Taunt = true;
                result.Add(new MasterBoardContainer(cln) { action = "Target " + target });
            }

            return new ChoiceSubBoardContainer(result, b, "Play " + this);
        });
    }

    private bool hasBeast(Hero h)
    {
        foreach (Minion m in h.onBoard)
        {
            if (m.Beast)
                return true;
        }
        return false;
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero h = owner.id == b.me.id ? b.me : b.opp;
        if (hasBeast(h))
            return h.CalcValue(minions: 11) - h.CalcValue();
        else if (h.onBoard.Count == 0)
            return h.CalcValue(minions: 9 + h.maxMana) - h.CalcValue();
        else
            return h.CalcValue(minions: 7) - h.CalcValue();
    }
}