using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Sets.Basic.Mage.Spells;
using HSSim.Sets.Basic.Neutral.Minions;

namespace HSSim.Decks.Mage
{
    internal class BasicMage : Classes.Mage
    {
        // AAECAY0WAA9NvwHYAZwCoQK7Ar8DqwS0BPsEngXZCtoK+QqWDQA=
        public override Dictionary<Card, int> DeckList { get; } = new Dictionary<Card, int>
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

        public BasicMage()
        {
        }

        public BasicMage(bool id, bool nw) : base(id, nw)
        {
        }
    }
}