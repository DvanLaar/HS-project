using System.Collections.Generic;

class Rogue : Hero
{


    private void SetHeroPower()
    {
        HeroPower = ((b) =>
        {
            if (Mana < 2 || HeroPowerUsed)
                return null;
        });
    }
}