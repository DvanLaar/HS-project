using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class Flamestrike : Spell
    {
        public Flamestrike() : base(7)
        {
            SetSpell(b =>
            {
                var cln = b.Clone();
                var opp = cln.Me.Id == Owner.Id ? cln.Opp : cln.Me;
                foreach (var m in opp.OnBoard)
                {
                    m.ReduceHealth(4 + Owner.SpellDamage);
                }
                return new SingleSubBoardContainer(cln, b, "Play Flamestrike");
            });
        }

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            var opp = b.Me.Id == Owner.Id ? b.Opp : b.Me;
            var damage = 4 + Owner.SpellDamage;
            var allDead = true;
            var damages = 0;
            foreach (var m in opp.OnBoard)
            {
                if (m.Health <= damage)
                {
                    damages += m.Health + m.Attack;
                }
                else
                {
                    damages += damage;
                    allDead = false;
                }
            }
            if (allDead)
                damages += 2 + opp.MaxMana;
            return opp.CalcValue() + Owner.CalcValue(cards: -1) - opp.CalcValue(minions: 0 - damages) - Owner.CalcValue();
        }
    }
}
