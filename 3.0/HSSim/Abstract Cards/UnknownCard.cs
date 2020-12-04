using System.Collections.Generic;

namespace HSSim.Abstract_Cards
{
    internal class UnknownCard : Card
    {
        private List<Card> _options;

        public UnknownCard() : base(0)
        {
            _options = new List<Card>();
        }

        public UnknownCard(List<Card> options) : base(0)
        {
            options.Sort((a, b) => a.BaseCost.CompareTo(b.BaseCost));
            BaseCost = options[0].BaseCost;
            Cost = BaseCost;
            _options = options;
        }

        public UnknownCard(Dictionary<Card, int> deckList) : base(0)
        {
            var arr = new Card[deckList.Keys.Count];
            deckList.Keys.CopyTo(arr, 0);
            _options = new List<Card>(arr);
            _options.Sort((a, b) => a.BaseCost.CompareTo(b.BaseCost));
            BaseCost = _options[0].BaseCost;
            Cost = BaseCost;
        }

        public override SubBoardContainer Play(Board curBoard)
        {
            if (!CanPlay(curBoard))
                return null;
            var result = new List<SubBoardContainer>();
            foreach (var c in _options)
            {
                c.SetOwner(Owner);
                if (!c.CanPlay(curBoard)) continue;
                var toPlay = c.Clone();
                var cln = curBoard.Clone();
                (cln.Curr == cln.Me.Id ? cln.Me : cln.Opp).Hand[Owner.Hand.IndexOf(this)] = toPlay;
                toPlay.SetOwner(cln.Curr == cln.Me.Id ? cln.Me : cln.Opp);
                //bool debug = c.owner.hand.Contains(c);
                result.Add(toPlay.Play(cln));
            }
            result.Sort((a, b) => b.Value.CompareTo(a.Value));
            return result.Count == 0 ? null : new UnknownSubBoardContainer(result, curBoard, "Play Unknown Card");
        }

        public override Card Clone()
        {
            var uc = (UnknownCard)base.Clone();
            uc._options = new List<Card>(_options);

            return uc;
        }

        public override void SetOwner(Hero owner)
        {
            base.SetOwner(owner);
            foreach (var c in _options)
                c.Owner = owner;
        }
    }

    public override double DeltaBoardValue(Board b)
    {
        throw new System.NotImplementedException();
    }
}