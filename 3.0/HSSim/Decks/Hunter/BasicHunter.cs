using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Sets.Basic.Hunter.Minions;
using HSSim.Sets.Basic.Hunter.Spells;
using HSSim.Sets.Basic.Neutral.Minions;

namespace HSSim.Decks.Hunter
{
    internal class BasicHunter : Classes.Hunter
    {
        // AAECAR8AD0PYAYECpALTAp0D3gSIBe0G6weXCNkK2gr5CpcNAA==
        public override Dictionary<Card, int> DeckList { get; } = new Dictionary<Card, int>
        {
            {new ArcaneShot() , 2 },
            {new StonetuskBoar(), 2},
            {new TimberWolf(), 2 },
            {new Tracking(), 2 },
            {new BloodfenRaptor(), 2 },
            {new RiverCrocolisk(), 2 },
            {new IronforgeRifleman(), 2 },
            {new RaidLeader(), 2 },
            {new RazorfenHunter(), 2 },
            {new SilverbackPatriarch(), 2 },
            {new Houndmaster(), 2 },
            {new MultiShot(), 2 },
            {new OasisSnapjaw(), 2 },
            {new StormpikeCommando(), 2 },
            {new CoreHound(), 2 }
        };

        public BasicHunter()
        {
        }

        public BasicHunter(bool id, bool nw) : base(id, nw)
        {
        }
    }
}