using System;

abstract class Spell : Card
{
    protected Func<Board, SubBoardContainer> Cast;

    public Spell(int mana) : base(mana)
    {

    }

    public void SetSpell(Func<Board, SubBoardContainer> effect)
    {
        Cast = effect;
    }

    public override SubBoardContainer Play(Board curBoard)
    {
        if (!CanPlay(curBoard))
            return null;

        Board clone = curBoard.Clone();
        Hero own = owner.id == clone.me.id ? clone.me : clone.opp; //Move to card
        own.hand.RemoveAt(owner.hand.IndexOf(this));
        own.Mana -= cost;

        return Cast.Invoke(clone);
    }
}

abstract class Weapon : Card
{
    public int Attack { get; set; }
    public int Durability { get => durability; set { if (value == 0) owner.StartDestroyWeapon(this); durability = value; } }
    public bool Active
    {
        get => _active; set
        {
            if (_active == value) return;
            if (value)
                owner.Attack += Attack;
            else owner.Attack -= Attack; _active = value;
        }
    }
    private bool _active;
    int durability;

    public Weapon(int mana, int attack, int durability) : base(mana)
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

        Board clone = curBoard.Clone();
        Hero me = clone.me.id == owner.id ? clone.me : clone.opp;

        if (me.CurrentWeapon != null)
            me.StartDestroyWeapon(me.CurrentWeapon);

        Weapon w = (Weapon)me.hand[owner.hand.IndexOf(this)];
        me.CurrentWeapon = w;
        me.hand.Remove(w);
        me.Mana -= cost;
        w.Active = me.id == clone.curr;

        return new SingleSubBoardContainer(clone, curBoard, "Play " + this);
    }

    public override Card Clone()
    {
        Weapon w = (Weapon)base.Clone();
        w._active = _active;
        w.Attack = Attack;
        w.Durability = Durability;
        return w;
    }
}