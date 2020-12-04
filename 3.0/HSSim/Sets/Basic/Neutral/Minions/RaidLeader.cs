using System.Linq;
using HSSim.Abstract_Cards.Minions;
using HSSim.Abstract_Cards.Minions.AuraMinions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class RaidLeader : FriendlyMinionAuraMinion
    {
        public RaidLeader() : base(3, 2, 2)
        {
        }

        protected override void AddAura()
        {
            base.AddAura();
            foreach (var m in Owner.OnBoard.Where(m => m != this))
            {
                m.AlterAttack(1);
            }
            Owner.Summon += Aura;
        }

        protected override void RemoveAura()
        {
            base.RemoveAura();
            foreach (var m in Owner.OnBoard.Where(m => m != this))
            {
                m.AlterAttack(-1);
            }
            Owner.Summon -= Aura;
        }

        protected override void Aura(Minion m)
        {
            if (m == this)
                return;
            m.AlterAttack(1);
        }

        protected override void AuraInvert(Minion m)
        {
            if (m == this)
                return;
            m.AlterAttack(-1);
        }
    }
}