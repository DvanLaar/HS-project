using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Abstract_Cards.Minions;
using HSSim.Sets.Basic.Neutral.Tokens;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class Polymorph : Spell
    {
        public Polymorph() : base(4)
        {
            SetSpell(b =>
            {
                var result = new List<MasterBoardContainer>();
                var opponent = b.Me.Id == Owner.Id ? b.Opp : b.Me;
                foreach (var m in opponent.OnBoard)
                {
                    var clone = b.Clone();
                    var opp = clone.Me.Id == opponent.Id ? clone.Me : clone.Opp;
                    Minion sheep = new Sheep();
                    sheep.SetOwner(opp);
                    opp.OnBoard[opponent.OnBoard.IndexOf(m)].StartTransform();
                    opp.OnBoard[opponent.OnBoard.IndexOf(m)] = sheep;
                    result.Add(new MasterBoardContainer(clone) { Action = "Transform " + m });
                }

                foreach (var m in Owner.OnBoard)
                {
                    var clone = b.Clone();
                    var me = clone.Me.Id == Owner.Id ? clone.Me : clone.Opp;
                    me.Mana -= Cost;
                    Minion sheep = new Sheep();
                    sheep.SetOwner(me);
                    me.OnBoard[Owner.OnBoard.IndexOf(m)] = sheep;
                    result.Add(new MasterBoardContainer(clone) { Action = "Transform " + m });
                }
                return new ChoiceSubBoardContainer(result, b, "Play Polymorph");
            });
        }
        public override bool CanPlay(Board b)
        {
            if (!base.CanPlay(b))
                return false;

            return b.Me.OnBoard.Count > 0 || b.Opp.OnBoard.Count > 0;
        }
    }
}