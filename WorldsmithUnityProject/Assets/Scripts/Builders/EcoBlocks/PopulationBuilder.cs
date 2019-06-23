using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationBuilder : MonoBehaviour
{

    float artisanAddition = 0f;

    public void BuildPopulations()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled) 
                    BuildSettledPopulations(loc);


        SetPopulationDictionaryHierarchy();
    }

    void BuildSettledPopulations(Location loc)
    {

        artisanAddition = WorldConstants.INDUSTRY_RATE_POPULATION_ADDITION[loc.rateIndustry];

        if (loc.GetLocationSubType() == Location.LocationSubType.Polis)
            BuildPolisPopulations(loc);
        else if (loc.GetLocationSubType() == Location.LocationSubType.UrbanCenter)
            BuildUrbanCenterPopulations(loc);
        else if (loc.GetLocationSubType() == Location.LocationSubType.Stronghold)
            BuildStrongholdPopulations(loc);
        else if (loc.GetLocationSubType() == Location.LocationSubType.Village)
            BuildVillagePopulations(loc);
         
    }
    void SetPopulationDictionaryHierarchy()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            foreach (Population pop in loc.populationList)
                EconomyController.Instance.populationDictionary[pop] = loc.localRuler;
    }

    void BuildPolisPopulations (Location loc)
    {
        artisanAddition /= 2;

        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Citizen, Population.LaborType.Leisure, 0.08f, loc), null); 
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Citizen, Population.LaborType.Housekeeper, 0.38f - artisanAddition, loc), null); 
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Citizen, Population.LaborType.Artisan, 0.05f + artisanAddition, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.4f - artisanAddition, loc), null); 
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.03f + artisanAddition, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Slave, Population.LaborType.Housekeeper, 0.09f, loc), null); 
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Slave, Population.LaborType.Artisan, 0.01f, loc), null);

    }
    void BuildUrbanCenterPopulations(Location loc)
    {
        BuildDefaultPopulationTemplate(loc);
    }
    void BuildStrongholdPopulations(Location loc)
    {
        BuildDefaultPopulationTemplate(loc);
    }
    void BuildVillagePopulations(Location loc)
    {
        BuildDefaultPopulationTemplate(loc);
    }

    void BuildDefaultPopulationTemplate(Location loc)
    {
        if (loc.currentFaction >= 4) // Greek and Phoenician cities have citizens
        {
            if (loc.hubType == 3 || loc.hubType == 5)
            {
                artisanAddition /= 2;

                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Leisure, 0.06f, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Housekeeper, 0.34f - artisanAddition, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Artisan, 0.08f + artisanAddition, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.5f - artisanAddition, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.06f + artisanAddition, loc), null); 
            }
            else
            {
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Leisure, 0.04f, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Housekeeper, 0.38f - artisanAddition, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Artisan, 0.04f + artisanAddition, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.54f - artisanAddition, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.02f + artisanAddition, loc), null); 
            }
        
        }
        else
        {

            if (loc.hubType == 3 || loc.hubType == 5) // crafts supplier or major hub
            {
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Leisure, 0.04f, loc), null); 
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.9f - artisanAddition, loc), null); 
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.08f + artisanAddition, loc), null);
            }
            else
            {
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Leisure, 0.03f, loc), null); 
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.96f - artisanAddition, loc), null); 
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.02f + artisanAddition, loc), null);
            }
        }
    }
}
