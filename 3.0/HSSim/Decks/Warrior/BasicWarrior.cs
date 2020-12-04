using System.Collections.Generic;
using HSSim.Abstract_Cards;
using HSSim.Sets.Basic.Neutral.Minions;
using HSSim.Sets.Basic.Warrior.Minions;
using HSSim.Sets.Basic.Warrior.Spells;
using HSSim.Sets.Basic.Warrior.Weapons;

namespace HSSim.Decks.Warrior
{
    internal class BasicWarrior : Classes.Warrior
    {
        // AAECAQcADymdAb8BgQKhAtgCkQOLBPsEgAaRBtAH7wfxB5YNAA==
        public override Dictionary<Card, int> DeckList { get; } = new Dictionary<Card, int>
        {
            { new Charge(), 2 },
            { new MurlocRaider(), 2 },
            { new Execute(), 2 },
            { new FrostwolfGrunt(), 2 },
            { new HeroicStrike(), 2 },
            { new MurlocTidehunter(), 2 },
            { new FieryWarAxe(), 2 },
            { new RazorfenHunter(), 2 },
            { new WarsongCommander(), 2 },
            { new Wolfrider(), 2 },
            { new DragonlingMechanic(), 2 },
            { new SenjinShieldmasta(), 2 },
            { new GurubashiBerserker(), 2 },
            { new BoulderfistOgre(), 2 },
            { new LordOfTheArena(), 2 },
        };

        public BasicWarrior()
        {
        }

        public BasicWarrior(bool id, bool nw) : base(id, nw)
        {
        }
    }
}