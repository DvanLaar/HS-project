using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Neutral.Spells
{
    internal class Coin : Spell
    {
        public Coin() : base(0)
        {
            SetSpell(b =>
            {
                var clone = b.Clone();
                var ownerClone = b.Me.Id == Owner.Id ? clone.Me : clone.Opp;
                ownerClone.Mana -= Cost;
                ownerClone.Mana += 1;
                return new SingleSubBoardContainer(clone, b, "Play The Coin");
            });
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            return Owner.CalcValue(cards: -1) - Owner.CalcValue();
        }
    }
}