using System;
using System.Collections.Generic;
using System.Linq;

static class Effects
{
    static public T GetTarget<T>(List<T> list, bool random = false)
    {
        if (random)
        {
            Random rng = new Random();
            return list[rng.Next(0, list.Count)];
        }
        Console.WriteLine("Who would you like to target?");
        foreach (T item in list)
        {
            Console.WriteLine((list.IndexOf(item) + 1) + ": " + item);
        }
        int index = int.Parse(Console.ReadLine());
        return list[(index-1)];
    }
    static public T GetTarget<T>(List<T> list1, List<T> list2, bool random = false)
    {
        List<T> list = new List<T>();
        list.AddRange(list1);
        list.AddRange(list2);
        return GetTarget(list, random);
    }
    static public Minion GetMinionTarget(Board board, bool playerMinions = true, bool bossMinions = true, bool random = false)
    {
        return (Minion)getTargets(board, false, false, playerMinions, bossMinions, random);
    }
    static private IDamagable getTargets(Board board, bool player = true, bool boss = true, bool playerMinions = true, bool bossMinions = true, bool random = false)
    {
        List<IDamagable> targets = new List<IDamagable>();
        if (player)
            targets.Add(board.player);
        if (playerMinions)
            foreach(Minion m in board.player.onBoard)
            {
                targets.Add(m);
            }
        if (bossMinions)
            foreach(Minion m in board.boss.onBoard)
            {
                targets.Add(m);
            }
        if (boss)
            targets.Add(board.boss);
        if (!random)
        {
            Console.WriteLine("Who would you like to target?");
            foreach (IDamagable thing in targets)
            {
                Console.WriteLine((targets.IndexOf(thing) + 1) + ": " + thing);
            }

            int index = int.Parse(Console.ReadLine());
            return (targets[index - 1]);
        }
        Random rng = new Random();
        return targets[rng.Next(0, targets.Count)];
    }

    static public bool Summon(List<Minion> minions, Minion summonable)
    {
        if (minions.Count >= 7)
            return false;
        minions.Add(summonable);
        return true;
    }
    static public void Transform(Minion target)
    {
        List<Minion> source = target.owner.onBoard;
        target.Destroy();
        source[source.IndexOf(target)] = new Sheep();
    }
    static public void AddToHand(Card toAdd)
    {
        if (toAdd.owner.hand.Count < 10)
        {
            toAdd.owner.hand.Add(toAdd);
        }
    }
    static public void ReturnToHand(Minion minion)
    {
        if (minion.owner.hand.Count >= 10)
            minion.Destroy();
        minion.owner.hand.Add(minion);
        minion.owner.onBoard.Remove(minion);
    }

    static public bool checkList<T>(List<T> list1, List<T> list2)
    {
        if (list1 == null || list2 == null)
            return (list1 == null && list2 == null);
        foreach (T t1 in list1)
        {
            bool found = false;
            foreach (T t2 in list2)
            {
                if (t1.Equals(t2))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;
        }
        return true;
    }
}

interface IDamagable
{
    int attack { get; set; }
    int health { get; set; }
    bool isFrozen { get; set; }

    event AttackEventHandler Attack;
    event AttackEventHandler Attacked;

    void Damage(int damage);
    void Heal(int healFor);
    void Freeze();
}