﻿using System;
using System.Collections.Generic;

class Charge : Spell
{
    public Charge() : base(1)
    {
        SetSpell((b) =>
        {
            List<MasterBoardContainer> result = new List<MasterBoardContainer>();
            for (int i = 0; i < owner.onBoard.Count; i++)
            {
                Board clone = b.Clone();
                Hero me = owner.id == clone.me.id ? clone.me : clone.opp;
                Minion m = me.onBoard[i];

                m.Charge = true;
                m.cantAttackHeroes = true;
                Func<Board, SubBoardContainer> func = (brd) =>
                {
                    m.cantAttackHeroes = false;
                    return new SingleSubBoardContainer(brd, brd, m + "loses can't attack heroes");
                };
                me.EndTurnFuncs.Add(func);
                me.SingleEndTurnFuncs.Add(me.EndTurnFuncs.IndexOf(func));
                result.Add(new MasterBoardContainer(clone) { action = "Target " + m });
            }
            return new ChoiceSubBoardContainer(result, b, "Play " + this);
        });
    }
}