using System;
using System.Collections.Generic;

class BasicMage : Mage
{
    // AAECAY0WAA9NvwHYAZwCoQK7Ar8DqwS0BPsEngXZCtoK+QqWDQA=
    public override Dictionary<Card, int> DeckList { get => deckList; set => deckList = value; }
    private Dictionary<Card, int> deckList = new Dictionary<Card, int>()
    {
        { new ArcaneMissiles(), 2 },
        { new MurlocRaider(), 2 },
        { new ArcaneExplosion(), 2 },
        { new BloodfenRaptor(), 2 },
        { new NoviceEngineer(), 2 },
        { new RiverCrocolisk(), 2 },
        { new ArcaneIntellect(), 2 },
        { new RaidLeader(), 2 },
        { new Wolfrider(), 2 },
        { new Fireball(), 2 },
        { new OasisSnapjaw(), 2 },
        { new Polymorph(), 2 },
        { new SenjinShieldmasta(), 2 },
        { new Nightblade(), 2 },
        { new BoulderfistOgre(), 2 },
    };

    public BasicMage() : base()
    {
    }

    public BasicMage(bool id, bool nw) : base(id, nw)
    {
    }
}