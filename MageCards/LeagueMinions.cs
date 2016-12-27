using System;

class EtherealConjurer : Minion
{
    public EtherealConjurer() : base(5, 6, 3)
    {
        this.Battlecry += battlecry;
    }

    void battlecry()
    {
        //Discover a spell
    }
}