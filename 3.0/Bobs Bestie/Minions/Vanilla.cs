using System;

class Vanilla : Minion
{
    public Vanilla(int rank, int attack, int health) : base(rank, attack, health)
    {

    }

    public static Minion MakeMinion()
    {
        Console.Clear();
        Console.WriteLine("What are the stats?");
        string[] s = Console.ReadLine().Split();

        Minion r = new Vanilla(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
        for (int i = 3; i < s.Length; i++)
        {
            switch (s[i])
            {
                case "taunt":
                    r.Taunt = true;
                    break;
                case "divine":
                    r.DivineShield = true;
                    break;
            }
        }
        return r;
    }

    public override Minion Clone()
    {
        return new Vanilla(Rank, Attack, MaxHealth)
        {
            Health = Health,
            DivineShield = DivineShield,
            Taunt = Taunt,
            Poisonous = Poisonous,
            Windfury = Windfury
        };
    }

    public override (string, Args)[] OnDeath(int index)
    {
        return new (string, Args)[0];
    }
}