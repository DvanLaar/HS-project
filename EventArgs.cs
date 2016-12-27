using System;

class AttackEventArgs : EventArgs
{
    public IDamagable attacker, defender;

    public AttackEventArgs(IDamagable attacker, IDamagable defender)
    {
        this.attacker = attacker;
        this.defender = defender;
    }
}