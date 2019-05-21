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

        SetTerritoryDictionaryHierarchy();
    }
    void BuildSettledTerritory(Location loc)
    {
        Territory terr = new Territory(loc);
        EconomyController.Instance.territoryDictionary.Add(terr, null);

        loc.locationTerritory = terr;
        for (int i = 0; i < 10; i++)
        {
            if (loc.territorySize == i)
                terr.territorySize = (Territory.Size)i;
            if (loc.territorySoil == i)
                terr.territorySoilQuality = (Territory.SoilQuality)i;
            if (loc.territoryStatus == i)
                terr.territoryStatus = (Territory.Status)i;


        }
    }



    void SetTerritoryDictionaryHierarchy()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
                EconomyController.Instance.territoryDictionary[loc.locationTerritory] = loc.localRuler;
    }

}
