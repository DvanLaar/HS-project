using System.Collections.Generic;
using System.Linq;

namespace HSSim
{
    internal class MasterBoardContainer
    {
        private const double WinValue = 1000;
        private const double LoseValue = -1000;
        public string Action;
        public List<SubBoardContainer> Children;
        private bool _expanded;
        public double Value { get { if (Board.Opp.Health <= 0) return WinValue; if (Board.Me.Health <= 0) return LoseValue; return _expanded ? Children[0].Value : Board.Value; } }
        public readonly Board Board;

    

        public MasterBoardContainer(Board b)
        {
            Board = b;
            _expanded = false;
            Children = new List<SubBoardContainer>();
        }

        public void Expand()
        {
            if (_expanded)
                return;

            if (Value > 999 || Value < -999)
            {
                _expanded = true;
                return;
            }
            _expanded = true;
            Children = new List<SubBoardContainer>();

            if (Board.ToPerform.Count > 0)
            {
                var action = Board.ToPerform.Pop();
                var cln = Board.Clone();
                var sbc = action(cln);
                Children.Add(sbc);
                return;
            }

            var currentPlayer = Board.Me.Id == Board.Curr ? Board.Me : Board.Opp;

            foreach (var play in currentPlayer.Hand.Select(c => c.Play(Board)).Where(play => play != null))
            {
                Children.Add(play);
            }
            foreach (var attack in currentPlayer.OnBoard.Select(m => m.PerformAttack(Board)).Where(attack => attack != null))
            {
                Children.Add(attack);
            }
            var hAttack = currentPlayer.PerformAttack(Board);
            if (hAttack != null)
                Children.Add(hAttack);

            var hp = currentPlayer.UseHeroPower(Board);
            if (hp != null)
                Children.Add(hp);


            var clone = Board.Clone();
            (clone.Me.Id == clone.Curr ? clone.Me : clone.Opp).EndTurn(Board);
            clone.Curr = !clone.Curr;
            (clone.Me.Id == clone.Curr ? clone.Me : clone.Opp).StartTurn(clone);
            Children.Add(new SingleSubBoardContainer(clone, Board, "End turn"));
        }

        public void Sort()
        {
            if (!_expanded)
                return;
            foreach (var sbc in Children)
            {
                foreach (var mbc in sbc.Children)
                {
                    mbc.Sort();
                }
                sbc.Children.Sort((x, y) => Board.Curr ? y.Value.CompareTo(x.Value) : x.Value.CompareTo(y.Value));
            }
            Children.Sort((x, y) => Board.Curr ? y.Value.CompareTo(x.Value) : x.Value.CompareTo(y.Value));
        }
    }

    internal abstract class SubBoardContainer
    {
        public List<MasterBoardContainer> Children;
        public abstract double Value { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private Board Parent { get; }
        public readonly string Action;

        protected SubBoardContainer(Board parent, string action)
        {
            Action = action;
            Parent = parent;
        }
    }

    internal class SingleSubBoardContainer : SubBoardContainer
    {
        public override double Value => Children[0].Value;

        public SingleSubBoardContainer(MasterBoardContainer b, Board parent, string action) : base(parent, action)
        {
            Children = new List<MasterBoardContainer> { b };
        }

        public SingleSubBoardContainer(Board b, Board parent, string action) : this(new MasterBoardContainer(b), parent, action)
        {
        }
    }

    internal class ChoiceSubBoardContainer : SubBoardContainer
    {
        public override double Value { get
        {
            return Children.Select(mbc => mbc.Value).Prepend(double.MinValue).Max();
        } }

        // ReSharper disable once UnusedMember.Global
        public ChoiceSubBoardContainer(IEnumerable<Board> boards, Board parent, string action) : base(parent, action)
        {
            Children = new List<MasterBoardContainer>();

            foreach (var b in boards)
                Children.Add(new MasterBoardContainer(b));
        }

        public ChoiceSubBoardContainer(List<MasterBoardContainer> mbcs, Board parent, string action) : base(parent, action)
        {
            Children = mbcs;
        }
    }

    internal class RandomSubBoardContainer : SubBoardContainer
    {
        private readonly List<int> _occurences;

        public override double Value { get {
            var total = _occurences.Sum();
            return Children.Select((t, i) => t.Value * _occurences[i] / total).Sum(); } }

        // ReSharper disable once UnusedMember.Global
        public RandomSubBoardContainer(IEnumerable<Board> boards, List<int> occurences, Board parent, string action) : base(parent, action)
        {
            Children = new List<MasterBoardContainer>();
            foreach (var b in boards)
                Children.Add(new MasterBoardContainer(b));
            _occurences = occurences;
        }

        // ReSharper disable once UnusedMember.Global
        public RandomSubBoardContainer(IEnumerable<(Board, int)> input, Board parent, string action) : base(parent, action)
        {
            Children = new List<MasterBoardContainer>();
            _occurences = new List<int>();

            foreach (var (board, item2) in input)
            {
                Children.Add(new MasterBoardContainer(board));
                _occurences.Add(item2);
            }
        }

        public RandomSubBoardContainer(IEnumerable<(MasterBoardContainer, int)> input, Board parent, string action) : base(parent, action)
        {
            Children = new List<MasterBoardContainer>();
            _occurences = new List<int>();

            foreach (var (masterBoardContainer, i) in input)
            {
                Children.Add(masterBoardContainer);
                _occurences.Add(i);
            }
        }

    }

    internal class UnknownSubBoardContainer : SubBoardContainer
    {
        private readonly List<SubBoardContainer> _options;

        public UnknownSubBoardContainer(List<SubBoardContainer> options, Board parent, string action) : base(parent, action)
        {
            _options = options;
            Children = new List<MasterBoardContainer>();
            foreach (var sbc in options)
            foreach (var mbc in sbc.Children)
            {
                mbc.Action = sbc.Action + " " + mbc.Action;
                Children.Add(mbc);
            }
        }

        public override double Value { get
        {
            return _options.Select(sbc => sbc.Value).Prepend(double.MinValue).Max();
        } }
    }

    internal class RandomChoiceSubBoardContainer : SubBoardContainer
    {
        public readonly List<(List<MasterBoardContainer>, int, string)> Options;

        public RandomChoiceSubBoardContainer(List<(List<MasterBoardContainer>, int, string)> input, Board parent, string action) : base(parent, action)
        {
            Children = new List<MasterBoardContainer>();
            Options = input;
            foreach (var mbc in input.SelectMany(results => results.Item1))
                Children.Add(mbc);
        }

        public override double Value { get { double res = 0; foreach (var (masterBoardContainers, i, _) in Options) { var mbcValue = masterBoardContainers.Select(mbc => mbc.Value).Prepend(double.MinValue).Max();
            res += mbcValue * i; } return res; } }
    }
}