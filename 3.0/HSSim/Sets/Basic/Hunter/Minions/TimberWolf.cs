using System.Linq;
using HSSim.Abstract_Cards.Minions;
using HSSim.Abstract_Cards.Minions.AuraMinions;

namespace HSSim.Sets.Basic.Hunter.Minions
{
    internal class TimberWolf : FriendlyMinionAuraMinion
    {
        public TimberWolf() : base(1, 1, 1)
        {
            Beast = true;
        }

        protected override void Aura(Minion m)
        {
            if (m.Beast && m != this)
                m.AlterAttack(1);
        }

        protected override void AuraInvert(Minion m)
        {
            if (m.Beast && m != this)
                m.AlterAttack(-1);
        }

        public override double DeltaBoardValue(Board b)
        {
            var me = Owner.Id == b.Me.Id ? b.Me : b.Opp;
            var buffs = 2 + me.OnBoard.Count(m => m.Beast);
            if (me.OnBoard.Count == 0)
            {
                buffs += 2 + me.MaxMana;
            }
            return me.CalcValue() - me.CalcValue(minions: buffs);
        }
    }
}