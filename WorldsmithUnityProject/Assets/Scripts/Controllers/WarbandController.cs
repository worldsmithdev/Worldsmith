using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandController : MonoBehaviour
{
    public static WarbandController Instance { get; protected set; }

    Warband selectedWarband;

    private void Awake()
    {
        Instance = this;
    }


    public List<Warband> GetWarbandsAtLocation(Location loc)
    {
        List<Warband> returnList = new List<Warband>();
        foreach (Warband warband in EconomyController.Instance.warbandDictionary.Keys)
            if (warband != null)
                if (warband.xLocation == loc.GetPositionVector().x && warband.yLocation == loc.GetPositionVector().y)
                    returnList.Add(warband);
        return returnList;
    }

    public void SetSelectedWarband(Warband warband)
    {
        selectedWarband = warband;
    }
    public Warband GetSelectedWarband()
    {
        return selectedWarband;
    }
}
