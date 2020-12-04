using System.Collections.Generic;
using System.Linq;
using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Hunter.Minions
{
    internal class Houndmaster : BattlecryMinion
    {
        public Houndmaster() : base(4, 4, 3)
        {
            SetBattlecry(b =>
            {
                var beasts = Owner.OnBoard.Where(m => m.Beast).ToList();

                if (beasts.Count == 0)
                    return new SingleSubBoardContainer(b, b, "Play " + this);

                var result = new List<MasterBoardContainer>();

                foreach (var m in beasts)
                {
                    var cln = b.Clone();
                    var me = cln.Me.Id == Owner.Id ? cln.Me : cln.Opp;
                    var target = me.OnBoard[Owner.OnBoard.IndexOf(m)];
                    target.AlterAttack(2);
                    target.AddHealth(2);
                    target.Taunt = true;
                    result.Add(new MasterBoardContainer(cln) { Action = "Target " + target });
                }

                return new ChoiceSubBoardContainer(result, b, "Play " + this);
            });
        }
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