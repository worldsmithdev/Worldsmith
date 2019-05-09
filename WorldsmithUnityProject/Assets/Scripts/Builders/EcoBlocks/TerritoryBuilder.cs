using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryBuilder : MonoBehaviour
{
    public void BuildTerritories()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled) 
                    BuildSettledTerritory(loc);
    }
    void BuildSettledTerritory(Location loc)
    {
        Territory terr = new Territory(loc);
        EconomyController.Instance.territoryDictionary.Add(terr, null);
    }

 
}
