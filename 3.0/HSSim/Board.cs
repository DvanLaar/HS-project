using System;
using System.Collections.Generic;

class Board
{
    public Hero me, opp;
    public Hero curr;
    public double value { get
        {
            return me.value - opp.value;
        } }

    public Board Clone()
    {
        Board b = new Board();
        b.me = me.Clone();
        b.opp = opp.Clone();
        b.curr = me.id == curr.id ? b.me : b.opp;

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

    public void EndTurn()
    {
        if (curr == me)
            curr = opp;
        else
            curr = me;

        curr.maxMana += 1;
        curr.Mana = curr.maxMana;
    }

}