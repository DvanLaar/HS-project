using System;

namespace HSSim.Abstract_Cards.Minions
{
    internal abstract class BattlecryMinion : Minion
    {
        private Func<Board, SubBoardContainer> _battlecry;

        protected BattlecryMinion(int mana, int attack, int health) : base(mana, attack, health)
        {
        }

        protected void SetBattlecry(Func<Board, SubBoardContainer> bc)
        {
            _battlecry = bc;
        }

        public override SubBoardContainer Play(Board curBoard)
        {
            if (!CanPlay(curBoard))
                return null;

            var b = base.Play(curBoard);
            return _battlecry.Invoke(b.Children[0].Board);        
        }
    }
}