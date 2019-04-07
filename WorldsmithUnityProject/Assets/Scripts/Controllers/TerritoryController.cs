using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryController : MonoBehaviour
{
    public static TerritoryController Instance { get; protected set; }

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
}
