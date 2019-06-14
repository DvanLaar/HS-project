using System;
using System.Collections.Generic;

class Board
{
    public Hero me, opp;
    public bool curr;
    public Stack<Func<Board, SubBoardContainer>> toPerform;

    public double value { get
        {
            //if (curr)
                return me.value - opp.value;
            //else
            //    return opp.value - me.value;
        } }

    public Board Clone()
    {
        Board b = new Board();
        b.me = me.Clone();
        b.opp = opp.Clone();
        b.curr = curr;
        b.toPerform = new Stack<Func<Board, SubBoardContainer>>(toPerform.ToArray());
        return b;
    }

    public void Attack(IDamagable attacker, IDamagable defender)
    {
        defender.TakeDamage(attacker.Attack);
        attacker.TakeDamage(defender.Attack);

        attacker.AttacksLeft--;
    }

    public Board(Hero me, Hero opp)
    {
        this.me = me;
        this.opp = opp;
        toPerform = new Stack<Func<Board, SubBoardContainer>>();
    }

    public Board()
    {
        toPerform = new Stack<Func<Board, SubBoardContainer>>();
    }
}