using System;

class EmptyDeck : Deck
{
    public EmptyDeck(double prob)
    {
    }

    public int Count()
    {
        return 0;
    }

    public int NoDoubleCount()
    {
        return 0;
    }

    public double Collect(int cardsLeft)
    {
        return 0.0;
    }

    public double CollectZero(int cardsLeft)
    {
        return 0.0;
    }
}

class DeckWithDoubles : Deck
{
    int total, doubles;
    Deck doubleDrawn, singleDrawn;
    double chance { get { return ((double)doubles * 2.0) / ((double)total); } }
    double prob;

    public DeckWithDoubles(int total, int doubles, double prob)
    {
        this.total = total;
        this.doubles = doubles;
        this.prob = prob;

        if (doubles * 2 == total)
        {
            this.singleDrawn = null;
            if (doubles == 1)
                this.doubleDrawn = new DeckWithoutDoubles(1, 1.0);
            else
                this.doubleDrawn = new DeckWithDoubles(total - 1, doubles - 1, 1.0);
            return;
        }
        this.singleDrawn = new DeckWithDoubles(total - 1, doubles, (1 - this.chance) * this.prob);
        if (doubles == 1)
            this.doubleDrawn = new DeckWithoutDoubles(total - 1, this.prob * this.chance);
        else
            this.doubleDrawn = new DeckWithDoubles(total - 1, doubles - 1, this.prob * this.chance);
    }

    public int Count()
    {
        int doubleCount = 0, singleCount = 0;
        if (this.doubleDrawn != null)
            doubleCount = doubleDrawn.Count();
        if (this.singleDrawn != null)
            singleCount = singleDrawn.Count();
        return doubleCount + singleCount + 1;
    }

    public int NoDoubleCount()
    {
        int result = 0;
        if (this.doubleDrawn != null)
            result += doubleDrawn.NoDoubleCount();
        if (this.singleDrawn != null)
            result += singleDrawn.NoDoubleCount();
        return result;
    }

    public double Collect(int cardsLeft)
    {
        if (total == cardsLeft)
            return this.prob;
        double result = 0.0;
        result += this.doubleDrawn.Collect(cardsLeft);
        if (this.singleDrawn != null)
            result += this.singleDrawn.Collect(cardsLeft);
        return result;
    }

    public double CollectZero(int cardsLeft)
    {
        if (total == cardsLeft)
            return 0.0;
        double result = 0.0;
        result += this.doubleDrawn.CollectZero(cardsLeft);
        if (this.singleDrawn != null)
            result += this.singleDrawn.CollectZero(cardsLeft);
        return result;
    } 
}

class DeckWithoutDoubles : Deck
{
    int total;
    Deck cardDrawn;
    double prob;

    public DeckWithoutDoubles(int total, double prob)
    {
        this.total = total;
        this.prob = prob;
        if (total == 1)
            cardDrawn = new EmptyDeck(this.prob);
        else
            cardDrawn = new DeckWithoutDoubles(total - 1, this.prob);
    }

    public int Count()
    {
        return 1 + cardDrawn.Count();
    }

    public int NoDoubleCount()
    {
        return 1 + cardDrawn.Count();
    }

    public double Collect(int cardsLeft)
    {
        if (total == cardsLeft)
            return this.prob;
        return cardDrawn.Collect(cardsLeft);
    }

    public double CollectZero(int cardsLeft)
    {
        if (total == cardsLeft)
            return this.prob;
        return cardDrawn.CollectZero(cardsLeft);
    }
}

interface Deck
{
    int Count();
    int NoDoubleCount();
    double Collect(int cardsLeft);
    double CollectZero(int cardsLeft);
}

class Program
{
    static void Main()
    {
        DeckWithDoubles deck = new DeckWithDoubles(29, 7, 1.0);
        Console.WriteLine(deck.Count());
        Console.WriteLine(deck.NoDoubleCount());
        for (int t = 0; t < 29; t++)
        {
            Console.WriteLine("Chance at " + t + " cards left:");
            Console.WriteLine(ChanceAt(t, deck));
        }
        Console.ReadLine();
    }

    static double ChanceAt(int cardsLeft, Deck startDeck)
    {
        double zeroes = startDeck.CollectZero(cardsLeft);
        double all = startDeck.Collect(cardsLeft);
        return zeroes / all;
    }
}