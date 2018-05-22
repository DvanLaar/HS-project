using System;
using System.Collections.Generic;

class Board
{
    public Hero me, opp;
    public bool curr;
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

        return b;
    }

    public void Attack(IDamagable attacker, IDamagable defender)
    {
        defender.Health -= attacker.Attack;
        attacker.Health -= defender.Attack;

        attacker.AttacksLeft--;
    }

    public Board(Hero me, Hero opp)
    {
        this.me = me;
        this.opp = opp;
    }

    public Board()
    {

    }
}