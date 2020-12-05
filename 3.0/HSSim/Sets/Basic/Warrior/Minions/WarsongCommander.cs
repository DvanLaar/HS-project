using System.Linq;
using HSSim.Abstract_Cards.Minions;
using HSSim.Abstract_Cards.Minions.AuraMinions;

namespace HSSim.Sets.Basic.Warrior.Minions
{
    internal class WarsongCommander : MinionAuraMinion
    {
        public WarsongCommander() : base(3, 2, 3)
        {
        }

        protected override void AuraInvert(Minion m)
        {
            if (!m.Charge)
                return;

            m.Attack -= 1;
        }

        protected override void Aura(Minion m)
        {
            if (!m.Charge)
                return;

            m.Attack += 1;
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            var buffs = 5;
            var h = b.Me.Id == Owner.Id ? b.Me : b.Opp;
            if (h.OnBoard.Count == 0)
                buffs += 2 + h.MaxMana;
            buffs += h.OnBoard.Count(m => m.Charge);
            return h.CalcValue(cards: -1, minions: buffs) - h.CalcValue();
        }
    }
}