namespace HSSim.Abstract_Cards.Minions.AuraMinions
{
    internal abstract class MinionAuraMinion : AuraMinion
    {
        protected MinionAuraMinion(int mana, int attack, int health) : base(mana, attack, health)
        {
        }

        protected abstract void Aura(Minion m);
        protected abstract void AuraInvert(Minion m);
        public override void SetOwner(Hero newOwner)
        {
            if (AuraActive)
            {
                newOwner.Summon += Aura;
            }
            base.SetOwner(newOwner);
        }
    }
}