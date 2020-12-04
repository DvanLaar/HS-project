using System;
using System.Collections.Generic;

namespace HSSim
{
    internal class Board
    {
        public Hero Me, Opp;
        public bool Curr;
        public Stack<Func<Board, SubBoardContainer>> ToPerform;

        public double Value =>
            Me.Value - Opp.Value;

        public Board Clone()
        {
            var b = new Board
            {
                Me = Me.Clone(),
                Opp = Opp.Clone(),
                Curr = Curr,
                ToPerform = new Stack<Func<Board, SubBoardContainer>>(ToPerform.ToArray())
            };
            return b;
        }

        public static void Attack(IDamagable attacker, IDamagable defender)
        {
            defender.TakeDamage(attacker.Attack);
            attacker.TakeDamage(defender.Attack);

            attacker.AttacksLeft--;
        }

        public Board(Hero me, Hero opp)
        {
            Me = me;
            Opp = opp;
            ToPerform = new Stack<Func<Board, SubBoardContainer>>();
        }

        private Board()
        {
            ToPerform = new Stack<Func<Board, SubBoardContainer>>();
        }
    }
}