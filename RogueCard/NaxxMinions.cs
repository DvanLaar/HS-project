using System;
using System.Collections.Generic;

class AnubarAmbusher : Minion
{
    public AnubarAmbusher() : base(4, 5, 5)
    {
        this.Deathrattle += deathrattle;
    }

    void deathrattle()
    {
        //getfriendlyMinion
        Minion target;
        while ((target = owner.getFriendlyMinionTarget(true)) == this);

        //returntohand
        Effects.ReturnToHand(target);
    }
}