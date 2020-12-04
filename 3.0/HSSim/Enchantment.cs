using System;

namespace HSSim
{
    internal abstract class Enchantment
    {
        public abstract void Apply();
        public abstract void Resolve();
    }

    internal class AttackEnchantment : Enchantment
    {
        private int Attack { get; }

        public AttackEnchantment(int attack)
        {
            Attack = attack;
        }
        
        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override void Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
