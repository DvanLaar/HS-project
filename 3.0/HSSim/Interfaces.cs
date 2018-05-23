using System;
using System.Collections.Generic;

interface IDamagable
{
    int Health { get; set; }
    int Attack { get; set; }
    int AttacksLeft { get; set; }
    SubBoardContainer PerformAttack(Board curBoard);

    void TakeDamage(int amount);
}