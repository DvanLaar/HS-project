using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSSim
{
    abstract class Enchantment
    {
        abstract public void Apply();
        abstract public void Resolve();
    }

    class AttackEnchantment : Enchantment
    {
        int attack;

        public AttackEnchantment(int attack)
        {
            this.attack = attack;
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
