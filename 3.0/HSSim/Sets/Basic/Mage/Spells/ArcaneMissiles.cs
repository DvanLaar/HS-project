using System.Collections.Generic;
using System.Linq;
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

        public override double DeltaBoardValue(Board b)
        {
            if (!CanPlay(b))
                return -100;

            int damage = 3 + Owner.SpellDamage, damageLeft = damage;
            var opp = Owner.Id == b.Me.Id ? b.Opp : b.Me;
            var targetN = opp.OnBoard.Count + 1;
            var targets = new IDamagable[targetN];
            for (var i = 0; i < targetN - 1; i++)
            {
                targets[i] = opp.OnBoard[i];
            }
            targets[targetN - 1] = opp;

            var maxHealth = new int[targetN];
            for (var i = 0; i < targetN; i++)
            {
                maxHealth[i] = targets[i].Health;
            }

            var perms = GenPerms(maxHealth, damage);
            var totalCombs = 0;
            double totalScore = 0;
            foreach (var perm in perms)
            {
                var comb = Combs(perm);
                totalCombs += comb;
                var score = 0;
                var allDead = true;
                for (var i = 1; i < perm.Length; i++)
                {
                    if (perm[i] == maxHealth[i])
                    {
                        score += targets[i].Health + targets[i].Attack;
                    }
                    else
                    {
                        score += perm[i];
                        allDead = false;
                    }
                }
                if (allDead)
                    score += 2 + opp.MaxMana;
                totalScore += Owner.CalcValue(cards: -1) + opp.CalcValue() - Owner.CalcValue() + opp.CalcValue(health: 0 - perm[0], minions: 0 - score);
            }
            return totalScore / totalCombs;
        }

        private IEnumerable<int[]> GenPerms(int[] maxHealths, int maxDmg)
        {
            var sum = maxHealths.Sum();
            if (sum >= maxDmg)
                return new List<int[]>() { maxHealths };

            if (maxHealths.Length == 1)
                return new List<int[]>() { new int[] { maxDmg } };

            var perms = new List<int[]>();
            for (var j = 0; j <= maxHealths[0] && j <= maxDmg; j++)
            {
                var maxHealthsAccent = new int[maxHealths.Length - 1];
                for (var k = 1; k < maxHealths.Length; k++)
                {
                    maxHealthsAccent[k - 1] = maxHealths[k];
                }
                var permsAccent = GenPerms(maxHealthsAccent, maxDmg - j);
                perms.AddRange(permsAccent.Select(perm => Combine(new[] {j}, perm)));
            }
            return perms;
        }

        private static int[] Combine(IReadOnlyList<int> a, IReadOnlyList<int> b)
        {
            var res = new int[a.Count + b.Count];

            for (var i = 0; i < a.Count; i++)
            {
                res[i] = a[i];
            }
            for (var i = 0; i < b.Count; i++)
            {
                res[i + a.Count] = b[i];
            }
            return res;
        }

        private int Combs(IReadOnlyList<int> damages)
        {
            var totalHits = damages.Sum();

            var combs = 1;
            foreach (var missile in damages)
            {
                combs *= NOverK(totalHits, missile);
                totalHits -= missile;
            }
            return combs;
        }

        private static int NOverK(int n, int k)
        {
            return Fac(n) / (Fac(k) * Fac(n - k));
        }

        private static int Fac(int f)
        {
            var res = 1;
            for (var i = f; i > 0; i--)
            {
                res *= i;
            }
            return res;
        }
    }
}