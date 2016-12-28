using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static List<Boss> bosses;
    private static List<Hero> decks;

    static void Main()
    {
        initialize();
        Console.WriteLine("Welcome to the HearthstoneHelper. I will be your guide.");
        Console.WriteLine("What would you like to do today? Adventure, arena, play or collection?");
        //pickGameMode();
        Board board1 = new Board(new Mage(), new AnubRekhan());
        SorcerersApprentice sa = new SorcerersApprentice();
        sa.owner = board1.player;
        sa.opponent = board1.boss;
        board1.player.hand.Add(sa);
        sa.OnPlay();
        Fireball fb = new Fireball();
        fb.owner = board1.player;
        fb.opponent = board1.boss;
        Fireball fb2 = (Fireball)fb.Clone();
        board1.player.deck.Add(fb);
        board1.player.deck.Add(fb2);
        Board board2 = board1.Clone();
        //board1.player.drawKnownCards(1);
        //board2.player.drawKnownCards(1);
        //board1.player.onBoard[0].Destroy();
        //board1.player.drawKnownCards(1);
        //board2.player.drawKnownCards(1);
        Console.WriteLine(board1.Equals(board2));
        Console.ReadLine();
    }

    static private void initialize()
    {
        bosses = Boss.createBosses();

        StreamReader reader = new StreamReader("Decks.txt");
        decks = new List<Hero>();

        string s = reader.ReadLine();
        while (s == "Deck")
        {
            initializeDeck(reader);
            s = reader.ReadLine();
        }
    }

    static private void initializeDeck(StreamReader reader)
    {
        string heroName = reader.ReadLine();
        string deckName = reader.ReadLine();
        Hero hero = Hero.GetHero(heroName);
        Card cardy;

        hero.name = deckName;
        string s = reader.ReadLine();
        List<Card> deck = new List<Card>();
        while (s != "" && s != null)
        {
            string[] card = s.Split();
            int amount = int.Parse(card[1]);
            cardy = Card.GetCard(card[0]);
            cardy.owner = hero;
            for (int i = 0; i < amount; i++ )
                deck.Add(cardy);
            s = reader.ReadLine();
        }
        hero.deck = deck;
        decks.Add(hero);
        return;
    }

    static private void pickGameMode()
    {
        string s = Console.ReadLine();
        s = s.ToLower();
        switch(s)
        {
            case "adventure":
                adventure();
                break;
            case "arena":
                break;
            case "play":
                break;
            case "collection":
                break;
        }
    }

    static private void adventure()
    {
        Hero boss = pickBoss();
        Hero player = pickDeck();
        Board board = new Board(player, boss);
        board.simulate();
    }

    static private Hero pickBoss()
    {
        Console.WriteLine("Which of the following bosses would you like to fight?");
        for(int i = 1; i < bosses.Count + 1; i++)
        {
            Console.WriteLine(i + ": " + bosses[i-1]);
        }

        string input = Console.ReadLine();

        int j;
        Boss result = null;
        if (int.TryParse(input, out j))
            result = bosses[j - 1];
        else
        { }//Todo
        return result;
    }

    static private Hero pickDeck()
    {
        Console.WriteLine("Which of the following decks would you like to play with?");
        for(int i = 0; i < decks.Count; i++)
        {
            Console.WriteLine(i + 1 + ": " + decks[i]);
        }

        string input = Console.ReadLine();
        //???
        //int j;
        //if (int.TryParse(input, out j))
        //    input = decks[j - 1].ToString();
        //switch (input)
        //{
        //    case "Classic Mage":
        //        return decks[0];
        //    default:
        //        return null;
        //}
        int j;
        if (!int.TryParse(input, out j))
            return null;
        return decks[j - 1];
    }
}