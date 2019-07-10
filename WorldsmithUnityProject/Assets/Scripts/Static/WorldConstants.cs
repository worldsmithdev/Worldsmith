using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldConstants  
{
    // Hosts static constant valuables for any (economic) variables and some methods for receving them
          

    // GENERATION
    public static float GENERATION_RANDOMIZER_LO = 0.95f;
    public static float GENERATION_RANDOMIZER_HI = 1.05f;
    public static float CITIZEN_GENERATION_ADVANTAGE= 0.65f; // Proportion of generated resources that go to citizen populations
    public static float NATURAL_RESOURCE_PERCENTAGE = 0.35f;

    // INDUSTRY
    public static float WARES_FOCUS_BONUS = 1.2f;  

    // LOCALPOLITICS
    public static float GENERATION_LEVY_RATE_HI = 0.1f;
    public static float GENERATION_LEVY_RATE_MID = 0.05f;
    public static float GENERATION_LEVY_RATE_LO = 0.02f;
    public static float INDUSTRY_LEVY_RATE_HI = 0.2f;
    public static float INDUSTRY_LEVY_RATE_MID = 0.1f;
    public static float INDUSTRY_LEVY_RATE_LO = 0.05f;

    // POPULATION
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
    public static float FOOD_CONSUMPTION_RATE = 2.0f; // HL wheat needed to survive
    public static float LUXURY_CONSUMPTION_RATE = 1.0f; // Wares piece

    // LOCALEXCHANGE
    public static float POP_FOOD_STORAGE_CYCLES = 1.5f; // Number of cycles-worth to set apart for safety
    public static float POP_COMFORT_STORAGE_CYCLES = 1.0f; // Number of cycles-worth to set apart for comfort
    public static float POP_FOOD_WANTED_CYCLES = 1.0f;  
    public static float POP_COMFORT_WANTED_CYCLES = 1.0f;  
    public static float RULER_FOOD_STORAGE_CYCLES = 1.0f;  
    public static float RULER_COMFORT_STORAGE_CYCLES = 1.0f;  

    // Dictionaries

    // All        
   

    // GENERATION
    public static Dictionary<int, float> GENERATION_RATES_MODIFIER = new Dictionary<int, float>();
    public static Dictionary<Resource.Type, float> DEFAULT_NATURAL_RESOURCETYPES = new Dictionary<Resource.Type, float>();
    public static Dictionary<Territory.Size, float> TERRITORYSIZE_MULTIPLIER = new Dictionary<Territory.Size, float>();
    public static Dictionary<Territory.SoilQuality, float> TERRITORYSOIL_MULTIPLIER = new Dictionary<Territory.SoilQuality, float>();
    public static Dictionary<Territory.Status, float> TERRITORYSTATUS_MULTIPLIER = new Dictionary<Territory.Status, float>();

    // INDUSTRY
    public static Dictionary<int, float> INDUSTRY_RATE_MODIFIER = new Dictionary<int, float>();



    // POPULATION
    public static Dictionary<int, float> INDUSTRY_RATE_POPULATION_ADDITION = new Dictionary<int, float>();



    public static void SetDictionaries()
    {
        // ALL

     

        // GENERATION
        GENERATION_RATES_MODIFIER.Add(1, 0.04f);
        GENERATION_RATES_MODIFIER.Add(2, 0.1f);
        GENERATION_RATES_MODIFIER.Add(3, 0.25f);
        GENERATION_RATES_MODIFIER.Add(4, 0.55f);
        GENERATION_RATES_MODIFIER.Add(5, 0.85f);
        DEFAULT_NATURAL_RESOURCETYPES.Add(Resource.Type.Timber, 0.75f);  // Must equal 1.0 total
        DEFAULT_NATURAL_RESOURCETYPES.Add(Resource.Type.Stone, 0.25f);
        TERRITORYSIZE_MULTIPLIER.Add(Territory.Size.Core, 0.6f);
        TERRITORYSIZE_MULTIPLIER.Add(Territory.Size.Cluster, 0.8f);
        TERRITORYSIZE_MULTIPLIER.Add(Territory.Size.Valley, 1.0f);
        TERRITORYSIZE_MULTIPLIER.Add(Territory.Size.Urban, 1.3f);
        TERRITORYSIZE_MULTIPLIER.Add(Territory.Size.Chora, 1.6f);
        TERRITORYSOIL_MULTIPLIER.Add(Territory.SoilQuality.Poor, 0.7f);
        TERRITORYSOIL_MULTIPLIER.Add(Territory.SoilQuality.Average, 1.0f);
        TERRITORYSOIL_MULTIPLIER.Add(Territory.SoilQuality.Rich, 1.3f);
        TERRITORYSOIL_MULTIPLIER.Add(Territory.SoilQuality.Volcanic, 1.6f);
        TERRITORYSOIL_MULTIPLIER.Add(Territory.SoilQuality.Splendid, 1.8f);
        TERRITORYSTATUS_MULTIPLIER.Add(Territory.Status.Destroyed, 0.25f);
        TERRITORYSTATUS_MULTIPLIER.Add(Territory.Status.Neglected, 0.5f);
        TERRITORYSTATUS_MULTIPLIER.Add(Territory.Status.Maintained, 1.0f);
        TERRITORYSTATUS_MULTIPLIER.Add(Territory.Status.Optimised, 1.4f);
        TERRITORYSTATUS_MULTIPLIER.Add(Territory.Status.Industrialized, 1.8f);

        // POPULATION
        INDUSTRY_RATE_POPULATION_ADDITION.Add(1, 0.0f);
        INDUSTRY_RATE_POPULATION_ADDITION.Add(2, 0.02f);
        INDUSTRY_RATE_POPULATION_ADDITION.Add(3, 0.04f);
        INDUSTRY_RATE_POPULATION_ADDITION.Add(4, 0.08f);
        INDUSTRY_RATE_POPULATION_ADDITION.Add(5, 0.16f);

        // INDUSTRY
        INDUSTRY_RATE_MODIFIER.Add(1 , 0.9f);
        INDUSTRY_RATE_MODIFIER.Add(2 , 1.0f);
        INDUSTRY_RATE_MODIFIER.Add(3 , 1.1f);
        INDUSTRY_RATE_MODIFIER.Add(4 , 1.2f);
        INDUSTRY_RATE_MODIFIER.Add(5 , 1.3f);

    }


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
