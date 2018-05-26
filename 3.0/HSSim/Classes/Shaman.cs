using System.Collections.Generic;

class Shaman : Hero
{
    public override Dictionary<Card, int> DeckList { get => new Dictionary<Card, int>(); set { } }

    public Shaman() : base()
    {

    }

    public Shaman(bool id, bool nw) : base(id, nw)
    {

    }

    private void SetHeroPower()
    {
        HeroPower = ((b) =>
        {
            if (HeroPowerUsed || Mana < 2 || onBoard.Count >= 7)
                return null;

            Minion[] totems = new Minion[] {new HealingTotem(), new SearingTotem(), new StoneclawTotem(), new WrathOfAirTotem() };
            List<(MasterBoardContainer, int)> result = new List<(MasterBoardContainer, int)>();
            foreach (Minion m in totems)
            {
                Board clone = b.Clone();
                Hero own = id == clone.me.id ? clone.me : clone.opp;
                m.SetOwner(own);
                if (own.onBoard.TrueForAll((comp) => comp.GetType() != m.GetType()))
                {
                    own.StartSummon(m);
                    result.Add((new MasterBoardContainer(clone) { action = "Summon " + m }, 1));
                }
            }
            if (result.Count == 0)
                return null;
            return new RandomSubBoardContainer(result, b, "Use Hero Power");
        });
    }
}