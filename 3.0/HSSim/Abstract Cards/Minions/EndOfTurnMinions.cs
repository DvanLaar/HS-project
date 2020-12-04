namespace HSSim.Abstract_Cards.Minions
{
    internal abstract class EndOfTurnMinion : Minion
    {
        protected EndOfTurnMinion(int mana, int attack, int health) : base(mana, attack, health)
        {
            Transform += RemoveEffect;
        }

        public override void SetOwner(Hero owner)
        {
            base.SetOwner(owner);
            owner.Summon += m => { if (m == this) AddEffect(); };
        }

        protected abstract SubBoardContainer EoTEffect(Board b);

        private void AddEffect()
        {
            Owner.EndTurnFuncs.Add(EoTEffect);
        }

        private void RemoveEffect()
        {
            Owner.EndTurnFuncs.Remove(EoTEffect);
        }
    }
}