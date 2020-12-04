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
    }
}
