using System;
using System.Collections.Generic;

class ArcaneMissiles : Spell
{
    public ArcaneMissiles() : base(1)
    {
        SetSpell((b) =>
        {
            List<(MasterBoardContainer, int)> temp = new List<(MasterBoardContainer, int)>();
            (MasterBoardContainer, int)[] result = new(MasterBoardContainer, int)[1] { (new MasterBoardContainer(b), 1) };
            for (int i = 0; i < (3 + owner.SpellDamage); i++)
            {
                foreach((MasterBoardContainer, int) brd in result)
                {
                    temp.AddRange(RandomDamage(brd.Item1.board, brd.Item1.action));
                }
                result = temp.ToArray();
                temp.Clear();
            }
            return new RandomSubBoardContainer(new List<(MasterBoardContainer, int)>(result), b, "Play Arcane Missiles");
        });
    }

    private (MasterBoardContainer, int)[] RandomDamage(Board b, string prevAction)
    {
        List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
        Hero opponent = b.me.id == owner.id ? b.opp : b.me;

        foreach (Minion m in opponent.onBoard)
        {
            Board clone = b.Clone();
            Hero Opp = clone.me.id == opponent.id ? clone.me : clone.opp;
            Opp.onBoard[opponent.onBoard.IndexOf(m)].TakeDamage(1);
            result.Add((new MasterBoardContainer(clone) {action = prevAction + " Hit " + m}, 1));
        }

        Board cln = b.Clone();
        Hero guy = cln.me.id == opponent.id ? cln.me : cln.opp;
        guy.TakeDamage(1);
        result.Add((new MasterBoardContainer(cln) { action = prevAction + " Hit Face" }, 1));

        return result.ToArray();
    }

    public override double DeltaBoardValue(Board b)
    {
        if (!CanPlay(b))
            return -100;

        int damage = 3 + owner.SpellDamage, damageLeft = damage;
        Hero opp = owner.id == b.me.id ? b.opp : b.me;
        int targetN = opp.onBoard.Count + 1;
        IDamagable[] targets = new IDamagable[targetN];
        for (int i = 0; i < targetN - 1; i++)
        {
            targets[i] = opp.onBoard[i];
        }
        targets[targetN - 1] = opp;

        int[] maxHealth = new int[targetN];
        for (int i = 0; i < targetN; i++)
        {
            maxHealth[i] = targets[i].Health;
        }

        List<int[]> perms = GenPerms(maxHealth, damage);
        int totalCombs = 0;
        double totalScore = 0;
        foreach (int[] perm in perms)
        {
            int comb = combs(perm);
            totalCombs += comb;
            int score = 0;
            bool allDead = true;
            for (int i = 1; i < perm.Length; i++)
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
                score += 2 + opp.maxMana;
            totalScore += owner.CalcValue(cards: -1) + opp.CalcValue() - owner.CalcValue() + opp.CalcValue(health: 0 - perm[0], minions: 0 - score);
        }
        return totalScore / totalCombs;
    }

    private List<int[]> GenPerms(int[] maxHealths, int maxDmg)
    {
        int sum = 0;
        for (int i = 0; i < maxHealths.Length; i++)
        {
            sum += maxHealths[i];
        }
        if (sum >= maxDmg)
            return new List<int[]>() { maxHealths };

        if (maxHealths.Length == 1)
            return new List<int[]>() { new int[] { maxDmg } };

        List<int[]> perms = new List<int[]>();
        for (int j = 0; j <= maxHealths[0] && j <= maxDmg; j++)
        {
            int[] maxHealthsAccent = new int[maxHealths.Length - 1];
            for (int k = 1; k < maxHealths.Length; k++)
            {
                maxHealthsAccent[k - 1] = maxHealths[k];
            }
            List<int[]> permsAccent = GenPerms(maxHealthsAccent, maxDmg - j);
            foreach (int[] perm in permsAccent)
            {
                perms.Add(combine(new int[] { j }, perm));
            }
        }
        return perms;
    }

    private int[] combine(int[] a, int[] b)
    {
        int[] res = new int[a.Length + b.Length];

        for (int i = 0; i < a.Length; i++)
        {
            res[i] = a[i];
        }
        for (int i = 0; i < b.Length; i++)
        {
            res[i + a.Length] = b[i];
        }
        return res;
    }

    private int combs(int[] damages)
    {
        int totalHits = 0;
        for (int i = 0; i < damages.Length; i++)
        {
            totalHits += damages[i];
        }

        int combs = 1;
        for (int i = 0; i < damages.Length; i++)
        {
            combs *= NoverK(totalHits, damages[i]);
            totalHits -= damages[i];
        }
        return combs;
    }

    private int NoverK(int n, int k)
    {
        return fac(n) / (fac(k) * fac(n - k));
    }

    private int fac(int f)
    {
        int res = 1;
        for (int i = f; i > 0; i--)
        {
            res *= i;
        }
        return res;
    }
}