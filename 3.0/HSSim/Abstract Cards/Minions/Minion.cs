using System.Collections.Generic;

namespace HSSim.Abstract_Cards.Minions
{
    internal abstract class Minion : Card, IDamagable
    {
        private int _baseAttack, _baseHealth, _maxHealth;
        protected int CurHealth;
        public bool Taunt = false;
        private const bool Windfury = false;
        private const bool MegaWindfury = false;
        public bool CantAttackHeroes = false;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        public static int MaxAttacks { get { if (MegaWindfury) return 4; return Windfury ? 2 : 1;
        } }

        public bool Beast = false;
        protected bool Totem = false;
        protected bool Mech { get; set; } = false;
        protected bool Murloc = false;
        private bool _charge;

        public delegate void EmptyHandler();
        public event EmptyHandler Transform;
        public event EmptyHandler Destroy;
        public event EmptyHandler OnDamaged;

        public virtual int Health
        {
            get => CurHealth; set
            {
                if (value < CurHealth)
                    OnDamaged?.Invoke();
                CurHealth = value;
                if (CurHealth <= 0)
                {
                    StartDestroy();
                }
            }
        }
        public int Attack { get; set; }
        public int AttacksLeft { get; set; }
        public bool Charge { get => _charge; set { if (value) if (_charge) _charge = true; else { _charge = true; AttacksLeft = MaxAttacks; } else _charge = false; } }
        public bool Damaged => Health != _maxHealth;

        protected Minion(int mana, int attack, int health) : base(mana)
        {
            _baseAttack = attack;
            Attack = attack;
            _baseHealth = health;
            CurHealth = health;
            _maxHealth = health;
            Cost = mana;
            AttacksLeft = 0;
        }

        public override Card Clone()
        {
            var m = (Minion)base.Clone();
            m._baseAttack = _baseAttack;
            m._baseHealth = _baseHealth;
            m.Cost = Cost;
            m._maxHealth = _maxHealth;
            m.CurHealth = CurHealth;
            m.Attack = Attack;
            m.AttacksLeft = AttacksLeft;

            return m;
        }

        public void TakeDamage(int dmg)
        {
            Health -= dmg;
        }

        public override bool CanPlay(Board b)
        {
            return Owner.OnBoard.Count < 7 && base.CanPlay(b);
        }

        public void StartTransform() //Maybe include target, perform transform too
        {
            Transform?.Invoke();
        }

        public void StartDestroy()
        {
            Destroy?.Invoke();
            Owner.OnBoard.Remove(this);
        }

        public void AddHealth(int increase)
        {
            if (increase <= 0)
                return;
            _maxHealth += increase;
            CurHealth += increase;
        }

        public void ReduceHealth(int decrease)
        {
            if (decrease <= 0)
                return;
            _maxHealth -= decrease;
            if (CurHealth > _maxHealth)
                CurHealth = _maxHealth;
        }

        public void AlterAttack(int alteration)
        {
            if (Attack + alteration <= 0)
            {
                Attack = 0;
                return;
            }
            Attack += alteration;
        }

        public override SubBoardContainer Play(Board curBoard)
        {
            if (!CanPlay(curBoard))
                return null;

            var b = curBoard.Clone();

            var ownerClone = Owner.Id == b.Me.Id ? b.Me : b.Opp;
            var m = (Minion)ownerClone.Hand[Owner.Hand.IndexOf(this)];

            ownerClone.Hand.Remove(m);
            ownerClone.Mana -= Cost;
            ownerClone.StartSummon(m);

            return new SingleSubBoardContainer(new MasterBoardContainer(b), curBoard, "Play " + this);
        }

        public virtual SubBoardContainer PerformAttack(Board curBoard)
        {
            if (AttacksLeft <= 0)
                return null;

            var results = new List<MasterBoardContainer>();
            var opponent = curBoard.Me.Id == Owner.Id ? curBoard.Opp : curBoard.Me;
            var myIndex = Owner.OnBoard.IndexOf(this);
            if (opponent.OnBoard.TrueForAll(m => !m.Taunt)) //All minions don't have taunt => any minion and hero are valid targets)
            {
                foreach (var m in opponent.OnBoard)
                {
                    var b = curBoard.Clone();
                    var theirIndex = opponent.OnBoard.IndexOf(m);
                    var attacker = b.Me.Id == Owner.Id ? b.Me.OnBoard[myIndex] : b.Opp.OnBoard[myIndex];
                    var defender = b.Me.Id == m.Owner.Id ? b.Me.OnBoard[theirIndex] : b.Opp.OnBoard[theirIndex];
                    Board.Attack(attacker, defender);
                    results.Add(new MasterBoardContainer(b) { Action = "Attacks " + defender });
                }

                if (CantAttackHeroes) return new ChoiceSubBoardContainer(results, curBoard, this + " attacks");
                var clone = curBoard.Clone();
                var opp = clone.Me.Id == opponent.Id ? clone.Me : clone.Opp;
                var att = clone.Me.Id == Owner.Id ? clone.Me.OnBoard[myIndex] : clone.Opp.OnBoard[myIndex];
                Board.Attack(att, opp);
                results.Add(new MasterBoardContainer(clone) { Action = "Attacks Face" });

                return new ChoiceSubBoardContainer(results, curBoard, this + " attacks");
            }

            foreach (var m in opponent.OnBoard)
            {
                if (!m.Taunt)
                    continue;

                var b = curBoard.Clone();
                var theirIndex = opponent.OnBoard.IndexOf(m);
                var attacker = b.Me.Id == Owner.Id ? b.Me.OnBoard[myIndex] : b.Opp.OnBoard[myIndex];
                var defender = b.Me.Id == m.Owner.Id ? b.Me.OnBoard[theirIndex] : b.Opp.OnBoard[theirIndex];
                Board.Attack(attacker, defender);
                results.Add(new MasterBoardContainer(b) { Action = "Attacks " + defender });
            }

            return new ChoiceSubBoardContainer(results, curBoard, this + " attacks");
        }
    }
}