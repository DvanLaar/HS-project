using System;
using System.Collections.Generic;

class ArcaneExplosion : Spell
{
    public ArcaneExplosion() : base(2)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();
        foreach(Minion m in opponent.onBoard)
        {
            m.Damage(owner.spellDamage + 1);
        }
    }
}

class ArcaneIntellect : Spell
{
    public ArcaneIntellect() : base(3)
    {

    }

    public override void OnPlay()
    {
        base.OnPlay();
        owner.DrawCards(2);
    }
}

class ArcaneMissiles : Spell
{
    public ArcaneMissiles() : base(1)
    {

    }

    public override void OnPlay()
    {
        base.OnPlay();
        List<IDamagable> targets;
        Random random = new Random();

        targets = opponent.onBoard.ConvertAll<IDamagable>(minion => minion);
        targets.Add(this.opponent);

        int target;
        for (int i = 0; i < owner.spellDamage + 3; i++ )
        {
            if (targets.Count == 0)
                return;
            target = random.Next(0, targets.Count);
            if (targets[target].health <= 0)
            {
                targets.Remove(targets[target]);
                i--;
                continue;
            }
            targets[target].Damage(1);
        }
    }
}

class Fireball : Spell
{
    public Fireball() : base(4)
    {
    }

    public Fireball(Hero owner, Hero opponent) : this()
    {
        this.owner = owner;
        this.opponent = opponent;
    }

    public override void OnPlay()
    {
        base.OnPlay();
        IDamagable target = owner.getTarget();
        target.Damage(owner.spellDamage + 6);
    }
}

class Flamestrike : Spell
{
    public Flamestrike() : base(7)
    {

    }

    public override void OnPlay()
    {
        base.OnPlay();

        foreach (IDamagable m in opponent.onBoard)
            m.Damage(owner.spellDamage + 4);
    }
}

class Frostbolt : Spell
{
    public Frostbolt() : base(2)
    {
    }

    public override void OnPlay()
    {
        base.OnPlay();

        IDamagable minion = owner.getMinionTarget();
        minion.Damage(owner.spellDamage + 3);
        minion.isFrozen = true;
    }
}

class MirrorImage : Spell
{
    public MirrorImage() : base(1)
    {

    }

    public override void OnPlay()
    {
        base.OnPlay();

        List<Minion> minions = owner.onBoard;
        Effects.Summon(minions, new MirrorImageToken());
        Effects.Summon(minions, new MirrorImageToken());
    }
}

class Polymorph : Spell
{
    public Polymorph() : base(4)
    {

    }

    public override void OnPlay()
    {
        base.OnPlay();
        Minion target = owner.getMinionTarget();
        Effects.Transform(target);
    }
}