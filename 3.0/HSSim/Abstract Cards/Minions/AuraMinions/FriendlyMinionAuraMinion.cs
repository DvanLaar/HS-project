namespace HSSim.Abstract_Cards.Minions.AuraMinions
{
    internal abstract class FriendlyMinionAuraMinion : MinionAuraMinion
    {
        protected FriendlyMinionAuraMinion(int mana, int attack, int health) : base(mana, attack, health)
        {
        }

        protected override void AddAura()
        {
            base.AddAura();
            foreach (var m in Owner.OnBoard)
                Aura(m);
            Owner.Summon += Aura;
        }

        protected override void RemoveAura()
        {
            base.RemoveAura();
            foreach (var m in Owner.OnBoard)
                AuraInvert(m);
            Owner.Summon -= Aura;
        }
    }
}