using System;
using System.Collections.Generic;

class Mage : Hero
{
    public Mage() : base()
    {
        HeroPower = (b) =>
        {
            if (mana < 2 || HeroPowerUsed)
                return null;


            List<Board> results = new List<Board>();
            Hero opponent = b.me.id == id ? b.opp : b.me;
            foreach(Minion m in opponent.onBoard)
            {
                Board clone = b.Clone();
                Hero I = clone.me.id == id ? clone.me : clone.opp;
                (clone.me.id == opponent.id ? clone.me : clone.opp).onBoard[opponent.onBoard.IndexOf(m)].Health--;
                I.Mana -= 2;
                I.HeroPowerUsed = true;
                results.Add(clone);
            }
            foreach(Minion m in onBoard)
            {
                Board clone = b.Clone();
                Hero I = clone.me.id == id ? clone.me : clone.opp;
                I.Mana -= 2;
                I.HeroPowerUsed = true;
                (clone.me.id == id ? clone.me : clone.opp).onBoard[onBoard.IndexOf(m)].Health--;
                results.Add(clone);
            }

            Board c = b.Clone();
            (c.me.id == opponent.id ? c.me : c.opp).Health--;
            Hero me = c.me.id == id ? c.me : c.opp;
            me.Mana -= 2;
            me.HeroPowerUsed = true;
            results.Add(c);

            c = b.Clone();
            (c.me.id == id ? c.me : c.opp).Health--;
            me = c.me.id == id ? c.me : c.opp;
            me.Mana -= 2;
            me.HeroPowerUsed = true;
            results.Add(c);

            return new MultipleChoiceBoardContainer(results, "Hero power used");
        };
    }
}