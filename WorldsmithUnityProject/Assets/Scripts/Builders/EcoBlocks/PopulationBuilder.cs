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
    }




    void BuildSettledPopulations(Location loc)
    {
        Population pop = new Population(loc);
        EconomyController.Instance.populationDictionary.Add(pop, null);
    }
}
