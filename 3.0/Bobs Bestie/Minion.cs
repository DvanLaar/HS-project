using System;
using System.Collections.Generic;

abstract class Minion
{
    public int Health, MaxHealth, Attack, Rank;
    public bool DivineShield, Windfury, Poisonous, Taunt;

    public Minion(int rank, int attack, int health)
    {
        Health = health;
        MaxHealth = health;
        Attack = attack;
        Rank = rank;
    }

    public abstract Minion Clone();
    public abstract (string, Args)[] OnDeath(int index);
}