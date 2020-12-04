namespace HSSim.Abstract_Cards.Minions.AuraMinions
{
    internal abstract class AuraMinion : Minion
    {
        protected bool AuraActive;

        protected AuraMinion(int mana, int attack, int health) : base(mana, attack, health)
        {
            AuraActive = false;
            Transform += RemoveAura;
        }

        protected virtual void AddAura()
        {
            AuraActive = true;
        }

        protected virtual void RemoveAura()
        {
            AuraActive = false;
        }
        public override Card Clone()
        {
            var am = (AuraMinion)base.Clone();
            am.AuraActive = AuraActive;
            return am;
        }

        public override int Health
        {
            get => CurHealth; set
            {
                CurHealth = value;
                if (CurHealth > 0) return;
                Owner.OnBoard.Remove(this);
                RemoveAura();
            }
        }

        public override void SetOwner(Hero owner)
        {
            base.SetOwner(owner);
            owner.Summon += m => { if (m == this) AddAura(); };
        }
    }
}