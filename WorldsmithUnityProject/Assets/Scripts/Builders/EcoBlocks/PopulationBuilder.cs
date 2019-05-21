using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationBuilder : MonoBehaviour
{
    public void BuildPopulations()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled) 
                    BuildSettledPopulations(loc);


        SetPopulationDictionaryHierarchy();
    }

    void BuildSettledPopulations(Location loc)
    {

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

        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Citizen, Population.LaborType.Housekeeper, 0.4f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Citizen, Population.LaborType.Builder, 0.02f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Citizen, Population.LaborType.Artisan, 0.05f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.35f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Habitant, Population.LaborType.Builder, 0.05f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.03f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Slave, Population.LaborType.Housekeeper, 0.04f, loc), null);
        EconomyController.Instance.populationDictionary.Add  ( new Population(Population.ClassType.Slave, Population.LaborType.Builder, 0.05f, loc), null);
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
            EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Housekeeper, 0.39f, loc), null);
            EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Citizen, Population.LaborType.Artisan, 0.05f, loc), null);
            EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.5f, loc), null);
            EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Builder, 0.06f, loc), null);
        }
        else
        {

            if (loc.hubType >= 3 || loc.rateIndustry > 1)
            {
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.88f, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Builder, 0.04f, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Artisan, 0.08f, loc), null);
            }
            else
            {
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Housekeeper, 0.92f, loc), null);
                EconomyController.Instance.populationDictionary.Add(new Population(Population.ClassType.Habitant, Population.LaborType.Builder, 0.08f, loc), null);
            }
        }
    }
}
