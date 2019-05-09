using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldConstants : MonoBehaviour
{
    // Hosts static constant valuables for any (economic) variables and some methods for receving them

    public static int POPSIZE1LO = 80;
    public static int POPSIZE1HI = 100;
    public static int POPSIZE2LO = 220;
    public static int POPSIZE2HI = 250;
    public static int POPSIZE3LO = 450;
    public static int POPSIZE3HI = 500;
    public static int POPSIZE4LO = 700;
    public static int POPSIZE4HI = 800;
    public static int POPSIZE5LO = 1200;
    public static int POPSIZE5HI = 1400;
    public static int POPSIZE6LO = 2000;
    public static int POPSIZE6HI = 2200;
    public static int POPSIZE7LO = 2800;
    public static int POPSIZE7HI = 3000;
    public static int POPSIZE8LO = 4100;
    public static int POPSIZE8HI = 4500;
    public static int POPSIZE9LO = 5500;
    public static int POPSIZE9HI = 6000;
    public static int POPSIZE10LO = 7500;
    public static int POPSIZE10HI = 8000;




    public static int GetPopulationAmount(Location loc)
    {
        if (loc.populationSize == 1)
            return Random.Range(POPSIZE1LO, POPSIZE1HI);
        else if (loc.populationSize == 2)
            return Random.Range(POPSIZE2LO, POPSIZE2HI);
        else if (loc.populationSize == 3)
            return Random.Range(POPSIZE3LO, POPSIZE3HI);
        else if (loc.populationSize == 4)
            return Random.Range(POPSIZE4LO, POPSIZE4HI);
        else if (loc.populationSize == 5)
            return Random.Range(POPSIZE5LO, POPSIZE5HI);
        else if (loc.populationSize == 6)
            return Random.Range(POPSIZE6LO, POPSIZE6HI);
        else if (loc.populationSize == 7)
            return Random.Range(POPSIZE7LO, POPSIZE7HI);
        else if (loc.populationSize == 8)
            return Random.Range(POPSIZE8LO, POPSIZE8HI);
        else if (loc.populationSize == 9)
            return Random.Range(POPSIZE9LO, POPSIZE9HI);
        else if (loc.populationSize == 10)
            return Random.Range(POPSIZE10LO, POPSIZE10HI);

        return 0;
    }
}
