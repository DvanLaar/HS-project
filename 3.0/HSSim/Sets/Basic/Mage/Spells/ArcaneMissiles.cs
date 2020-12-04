using System.Collections.Generic;
using HSSim.Abstract_Cards;

namespace HSSim.Sets.Basic.Mage.Spells
{
    internal class ArcaneMissiles : Spell
    {
        public ArcaneMissiles() : base(1)
        {
            SetSpell(b =>
            {
                var temp = new List<(MasterBoardContainer, int)>();
                var result = new[] { (new MasterBoardContainer(b), 1) };
                for (var i = 0; i < (3 + Owner.SpellDamage); i++)
                {
                    foreach(var (mbc, _) in result)
                    {
                        temp.AddRange(RandomDamage(mbc.Board, mbc.Action));
                    }
                    result = temp.ToArray();
                    temp.Clear();
                }
                return new RandomSubBoardContainer(new List<(MasterBoardContainer, int)>(result), b, "Play Arcane Missiles");
            });
        }

        private IEnumerable<(MasterBoardContainer, int)> RandomDamage(Board b, string prevAction)
        {
            var result = new List<(MasterBoardContainer, int)>();
            var opponent = b.Me.Id == Owner.Id ? b.Opp : b.Me;

            foreach (var m in opponent.OnBoard)
            {
                var clone = b.Clone();
                var opp = clone.Me.Id == opponent.Id ? clone.Me : clone.Opp;
                opp.OnBoard[opponent.OnBoard.IndexOf(m)].TakeDamage(1);
                result.Add((new MasterBoardContainer(clone) {Action = prevAction + " Hit " + m}, 1));
            }

            var cln = b.Clone();
            var guy = cln.Me.Id == opponent.Id ? cln.Me : cln.Opp;
            guy.TakeDamage(1);
            result.Add((new MasterBoardContainer(cln) { Action = prevAction + " Hit Face" }, 1));

            return result.ToArray();
        }
    }
}