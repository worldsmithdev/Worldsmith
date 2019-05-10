using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryController : MonoBehaviour
{
    public static TerritoryController Instance { get; protected set; }


    Territory selectedTerritory;

    private void Awake()
    {
        Instance = this;
    }


    public Territory GetTerritoryAtLocation(Location loc)
    {
        foreach (Territory terr in EconomyController.Instance.territoryDictionary.Keys)
            if (terr != null)
                if (terr.xLocation == loc.GetPositionVector().x && terr.yLocation == loc.GetPositionVector().y)
                    return terr;
        return null;

    }

    public Location GetLocationForTerritory(Territory terr)
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.locationTerritory == terr)
                return loc;

        Debug.Log("Returning null Location for: " + terr.blockID);
        return null;

    }

    public void SetSelectedTerritory(Territory territory)
    {
        selectedTerritory = territory;
    }
    public Territory GetSelectedTerritory()
    {
        return selectedTerritory;
    }
}
