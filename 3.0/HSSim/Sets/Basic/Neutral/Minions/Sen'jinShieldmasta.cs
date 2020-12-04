using HSSim.Abstract_Cards.Minions;

namespace HSSim.Sets.Basic.Neutral.Minions
{
    internal class SenjinShieldmasta : Minion
    {
        public SenjinShieldmasta() : base(4, 3, 5)
        {
            Taunt = true;
        }

        public override string ToString()
        {
            return "Sen'jin Shieldmasta";
        }
    }
}