using System;
using System.Collections.Generic;

abstract class Card
{
    public int baseCost { get; set; }
    protected int cost;
    public Hero owner;
    public abstract SubBoardContainer Play(Board curBoard);
    public virtual Card Clone()
    {
        Card c = (Card)GetType().InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
        c.baseCost = baseCost;
        c.cost = cost;
        return c;
    }

    public virtual void SetOwner(Hero owner)
    {
        this.owner = owner;
    }

    public virtual bool CanPlay(Board b)
    {
        return cost <= owner.Mana;
    }

    public Card(int mana)
    {
        baseCost = mana;
        cost = mana;
    }

    public override string ToString()
    {
        string bs = base.ToString();
        for (int i = 1; i < bs.Length; i++)
        {
            if (bs[i] >= 'A' && bs[i] <= 'Z')
            {
                bs = bs.Insert(i, " ");
                i++;
            }
        }
        return bs;
    }

    public SubBoardContainer DealDamage(int dmg, Board b)
    {
        List<MasterBoardContainer> result = new List<MasterBoardContainer>();
        Hero opponent = b.me.id == owner.id ? b.opp : b.me;

        Board clone;

        clone = b.Clone();
        clone.me.TakeDamage(dmg);
        (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
        result.Add(new MasterBoardContainer(clone) { action = "Hit Own Face" });

        clone = b.Clone();
        clone.opp.TakeDamage(dmg);
        (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
        result.Add(new MasterBoardContainer(clone) { action = "Hit Face" });

        foreach (Minion m in owner.onBoard)
        {
            clone = b.Clone();
            Hero me = clone.me.id == owner.id ? clone.me : clone.opp;
            me.Mana -= cost;
            Minion target = me.onBoard[owner.onBoard.IndexOf(m)];
            target.TakeDamage(dmg);
            result.Add(new MasterBoardContainer(clone) { action = "Hit " + target });
        }

        foreach (Minion m in opponent.onBoard)
        {
            clone = b.Clone();
            Hero Opponent = clone.me.id == opponent.id ? clone.me : clone.opp;
            (clone.me.id == owner.id ? clone.me : clone.opp).Mana -= cost;
            Minion target = Opponent.onBoard[opponent.onBoard.IndexOf(m)];
            target.TakeDamage(dmg);
            result.Add(new MasterBoardContainer(clone) { action = "Hit " + target });
        }

        return new ChoiceSubBoardContainer(result, b, "play " + this);
    }
}