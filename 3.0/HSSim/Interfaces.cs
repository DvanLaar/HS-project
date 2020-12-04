namespace HSSim
{
    internal interface IDamagable
    {
        int Health { get; set; }
        int Attack { get; set; }
        int AttacksLeft { get; set; }
        SubBoardContainer PerformAttack(Board curBoard);

        void TakeDamage(int amount);
    }
}