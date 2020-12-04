namespace HSSim.Abstract_Cards
{
    internal abstract class Weapon : Card
    {
        private int Attack { get; set; }
        private int Durability { get => _durability; set { if (value == 0) Owner.StartDestroyWeapon(this); _durability = value; } }
        public bool Active
        {
            get => _active; set
            {
                if (_active == value) return;
                if (value)
                    Owner.Attack += Attack;
                else Owner.Attack -= Attack; _active = value;
            }
        }
        private bool _active;
        private int _durability;

        protected Weapon(int mana, int attack, int durability) : base(mana)
        {
            Attack = attack;
            Durability = durability;
        }

        public override void SetOwner(Hero owner)
        {
            base.SetOwner(owner);
            Active = false;
        }

        public override SubBoardContainer Play(Board curBoard)
        {
            if (!CanPlay(curBoard))
                return null;

            var clone = curBoard.Clone();
            var me = clone.Me.Id == Owner.Id ? clone.Me : clone.Opp;

            if (me.CurrentWeapon != null)
                me.StartDestroyWeapon(me.CurrentWeapon);

            var w = (Weapon)me.Hand[Owner.Hand.IndexOf(this)];
            me.CurrentWeapon = w;
            me.Hand.Remove(w);
            me.Mana -= Cost;
            w.Active = me.Id == clone.Curr;

            return new SingleSubBoardContainer(clone, curBoard, "Play " + this);
        }

        public override Card Clone()
        {
            var w = (Weapon)base.Clone();
            w._active = _active;
            w.Attack = Attack;
            w.Durability = Durability;
            return w;
        }
    }
}