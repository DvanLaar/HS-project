using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Warrior.Spells
{
    internal class HeroicStrike : Spell
    {
        public HeroicStrike() : base(2)
        {
            SetSpell(b =>
            {
                var clone = b.Clone();
                var me = Owner.Id == clone.Me.Id ? clone.Me : clone.Opp;
                me.Attack += 4;

                SubBoardContainer Function(Board brd)
                {
                    var cln = brd.Clone();
                    var hero = Owner.Id == cln.Me.Id ? cln.Me : cln.Opp;
                    hero.Attack -= 4;
                    return new SingleSubBoardContainer(cln, brd, "End of " + this + " effect");
                }

                me.EndTurnFuncs.Add(Function);
                me.SingleEndTurnFuncs.Add(me.EndTurnFuncs.IndexOf(Function));
                return new SingleSubBoardContainer(clone, b, "Play " + this);
            });
        }
    }

    public override double DeltaBoardValue(Board b)
    {
        Hero me = owner.id == b.me.id ? b.me : b.opp;
        return me.CalcValue(cards: -1) - me.CalcValue();
    }
}