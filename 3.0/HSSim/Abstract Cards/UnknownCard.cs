using System.Collections.Generic;

class UnknownCard : Card
{
    List<Card> options;

    public UnknownCard() : base(0)
    {
        options = new List<Card>();
    }

    public UnknownCard(List<Card> options) : base(0)
    {
        options.Sort((a, b) => a.baseCost.CompareTo(b.baseCost));
        baseCost = options[0].baseCost;
        cost = baseCost;
        this.options = options;
    }

    public UnknownCard(Dictionary<Card, int> DeckList) : base(0)
    {
        Card[] arr = new Card[DeckList.Keys.Count];
        DeckList.Keys.CopyTo(arr, 0);
        options = new List<Card>(arr);
        options.Sort((a, b) => a.baseCost.CompareTo(b.baseCost));
        baseCost = options[0].baseCost;
        cost = baseCost;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;
        List<SubBoardContainer> result = new List<SubBoardContainer>();
        foreach (Card c in options)
        {
            c.SetOwner(owner);
            if (c.CanPlay(curBoard))
            {
                Card toPlay = c.Clone();
                Board cln = curBoard.Clone();
                (cln.curr == cln.me.id ? cln.me : cln.opp).hand[owner.hand.IndexOf(this)] = toPlay;
                toPlay.SetOwner(cln.curr == cln.me.id ? cln.me : cln.opp);
                //bool debug = c.owner.hand.Contains(c);
                result.Add(toPlay.Play(cln));
            }
        }
        result.Sort((a, b) => b.value.CompareTo(a.value));
        if (result.Count == 0)
            return null;
        return new UnknownSubBoardContainer(result, curBoard, "Play Unknown Card");
    }

    public override Card Clone()
    {
        UnknownCard uc = (UnknownCard)base.Clone();
        uc.options = new List<Card>(options);

        return uc;
    }

    public override void SetOwner(Hero owner)
    {
        base.SetOwner(owner);
        foreach (Card c in options)
            c.owner = owner;
    }
}