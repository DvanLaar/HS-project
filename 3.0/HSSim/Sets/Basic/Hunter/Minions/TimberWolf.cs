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
    }
}