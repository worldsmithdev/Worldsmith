using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandBuilder : MonoBehaviour
{
    public void BuildWarbands()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled)
                if (loc.GetLocationSubType() != Location.LocationSubType.Homestead && loc.GetLocationSubType() != Location.LocationSubType.Dwelling)
                    BuildSettledWarbands(loc);
    }

    void BuildSettledWarbands(Location loc)
    {
        Warband warband = new Warband(loc);
        EconomyController.Instance.warbandDictionary.Add(warband, null);
    }

}
