using System;

namespace HSSim.Abstract_Cards
{
    internal abstract class Spell : Card
    {
        private Func<Board, SubBoardContainer> _cast;

        protected Spell(int mana) : base(mana)
        {
        }

        protected void SetSpell(Func<Board, SubBoardContainer> effect)
        {
            _cast = effect;
        }

        public override SubBoardContainer Play(Board curBoard)
        {
            if (!CanPlay(curBoard))
                return null;

            var clone = curBoard.Clone();
            var own = Owner.Id == clone.Me.Id ? clone.Me : clone.Opp; //Move to card
            own.Hand.RemoveAt(Owner.Hand.IndexOf(this));
            own.Mana -= Cost;

            return _cast.Invoke(clone);
        }
    }
}