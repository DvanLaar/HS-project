using System;
using System.Collections.Generic;

class Board
{
    public delegate void AttackEventHandler(AttackEventArgs aea);
    public event AttackEventHandler Attack;

    public Hero player, boss;
    public double value;
    private bool gameEnded;
    private List<Board> possibilities;

    public Board()
    {
    }
    public Board(Hero player, Hero boss)
    {
        this.player = player;
        this.boss = boss;
        this.gameEnded = false;
        this.possibilities = new List<Board>();
    }

    public void simulate()
    {
        player.turn = true;
        Console.WriteLine("Do you go first?");

        TheCoin coin = new TheCoin();
        string s = Console.ReadLine();
        if (s.ToLower()[0] == 'y')
        {
            player.DrawCards(3);
            boss.DrawCards(4);
            coin.owner = boss;
            coin.opponent = player;
            boss.hand.Add(coin);
        }
        else if (s.ToLower()[0] == 'n')
        {
            boss.DrawCards(3);
            player.DrawCards(4);
            coin.owner = player;
            coin.opponent = boss;
            player.hand.Add(coin);
        }
        else
        {//TODO: teruggaan voor de plebs
            Console.WriteLine("Fucking moron...");
        }

        while(!gameEnded)
        {
            if (player.turn)
                playerTurn();
            else
                bossTurn();
        }
    }
    private void playerTurn()
    {
        //StartTurnEvent
        player.DrawCards(1);

        //SimulateTurn();

        switchTurn();
    }

    public HashSet<Board> SimulateTurn(Hero hero)
    {
        if (!hero.turn)
            return new HashSet<Board>(new Board[]{this});
        Board copy = (Board)this.MemberwiseClone();
        HashSet<Board> result = new HashSet<Board>(), temp = new HashSet<Board>();

        copy.switchTurn();
        result.Add(copy);

        copy = (Board)this.MemberwiseClone();
        copy.player.HeroPower(); //TODO
        result.Add(copy);

        foreach (Card c in hero.hand)
        {
            if (c.mana > hero.availableMana)
                continue;
            copy = (Board)this.MemberwiseClone();
            copy.player.hand.Remove(c);
            c.OnPlay();
            temp.Add(copy);
        }

        bool taunt = boss.onBoard.Count != 0 && !boss.onBoard.TrueForAll(minion => !minion.hasTaunt);
        foreach (Minion m in boss.onBoard)
        {
            if (taunt && !m.hasTaunt)
            {
                continue;
            }
            if (hero.attack > 0 && !hero.isFrozen)
            {
                copy = (Board)this.MemberwiseClone();
                copy.attack(hero, m);
                temp.Add(copy);
            }

            foreach(Minion min in boss.onBoard)
            {
                if (min.isFrozen)
                {
                    min.isFrozen = false;
                    continue;
                }
                if (min.attack == 0 || min.cantAttack)
                {
                    continue;
                }
                copy = (Board)this.MemberwiseClone();
                copy.attack(min, m);
                temp.Add(copy);
            }
        }

        for (int i = 0; i < possibilities.Count; i++)
        {
            //result.UnionWith(possibilities)
        }
        return result;
    }

    private void bossTurn()
    {


        switchTurn();
    }

    private void switchTurn()
    {
        if (player.turn)
        {
            player.turn = false;
            boss.turn = true;
        }
        else
        {
            player.turn = true;
            boss.turn = false;
        }
    }

    public Board Clone()
    {
        Board newBoard = new Board();
        newBoard.player = this.player.Clone();
        newBoard.boss = this.boss.Clone();
        newBoard.player.opponent = newBoard.boss;
        newBoard.boss.opponent = newBoard.player;
        newBoard.possibilities = new List<Board>();
        return newBoard;
    }
    public override bool Equals(object obj)
    {
        if (obj.GetType() != this.GetType())
            return false;
        Board compareTo = (Board)obj;
        return (this.player.Equals(compareTo.player) && this.boss.Equals(compareTo.boss));
    }

    private void attack(IDamagable attacker, IDamagable defender)
    {
        if (Attack != null)
            Attack(new AttackEventArgs(attacker, defender));
        attacker.health -= defender.attack;
        defender.health -= attacker.attack;
    }
}