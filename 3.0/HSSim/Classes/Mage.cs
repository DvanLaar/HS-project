using System;
using System.Collections.Generic;

class Mage : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Mage() : base()
    {
        SetHeropower();
    }

    public Mage(bool id, bool nw) : base(id, nw)
    {
        SetHeropower();
    }

    private void SetHeropower()
    {
        HeroPower = (b) =>
        {
            if (mana < 2 || HeroPowerUsed)
                return null;

            List<MasterBoardContainer> results = new List<MasterBoardContainer>();
            Hero opponent = b.me.id == id ? b.opp : b.me;
            foreach (Minion m in opponent.onBoard)
            {
                Board clone = b.Clone();
                Hero I = clone.me.id == id ? clone.me : clone.opp;
                (clone.me.id == opponent.id ? clone.me : clone.opp).onBoard[opponent.onBoard.IndexOf(m)].Health--;
                I.Mana -= 2;
                I.HeroPowerUsed = true;
                results.Add(new MasterBoardContainer(clone) { action = "Ping " + m });
            }
            foreach (Minion m in onBoard)
            {
                Board clone = b.Clone();
                Hero I = clone.me.id == id ? clone.me : clone.opp;
                I.Mana -= 2;
                I.HeroPowerUsed = true;
                (clone.me.id == id ? clone.me : clone.opp).onBoard[onBoard.IndexOf(m)].Health--;
                results.Add(new MasterBoardContainer(clone) { action = "Ping " + m });
            }

            Board c = b.Clone();
            (c.me.id == opponent.id ? c.me : c.opp).Health--;
            Hero me = c.me.id == id ? c.me : c.opp;
            me.Mana -= 2;
            me.HeroPowerUsed = true;
            results.Add(new MasterBoardContainer(c) { action = "Ping Face" });

            c = b.Clone();
            (c.me.id == id ? c.me : c.opp).Health--;
            me = c.me.id == id ? c.me : c.opp;
            me.Mana -= 2;
            me.HeroPowerUsed = true;
            results.Add(new MasterBoardContainer(c) { action = "Ping Own Face" });

            return new ChoiceSubBoardContainer(results, b, "Use Hero Power");
        };
    }
}