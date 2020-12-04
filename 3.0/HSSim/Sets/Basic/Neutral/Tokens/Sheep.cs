﻿using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Tokens
{
    internal class Sheep : Minion
    {
        public Sheep() : base(1, 1, 1)
        {
            Beast = true;
        }
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        int min = 2;
        if (owner.onBoard.Count == 0)
            min += 2 + owner.maxMana;
        return owner.CalcValue(cards: -1, minions: min);
    }
}