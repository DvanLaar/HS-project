

class LordOfTheArena : Minion
{
    public LordOfTheArena() : base(6, 6, 5)
    {
        Taunt = true;
    }

    public override string ToString()
    {
        return "Lord of the Arena";
    }
}