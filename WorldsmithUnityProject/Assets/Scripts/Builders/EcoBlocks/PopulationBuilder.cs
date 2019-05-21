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
        Population pop = new Population(loc);
        EconomyController.Instance.populationDictionary.Add(pop, null);
    }


    void SetPopulationDictionaryHierarchy()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            foreach (Population pop in loc.populationList)
                EconomyController.Instance.populationDictionary[pop] = loc.localRuler;
    }
}
