using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerController : MonoBehaviour
{
    public static RulerController Instance { get; protected set; }

    private void Awake()
    {
        Instance = this;
    }


    public List<Ruler> GetRulersAtLocation(Location loc)
    {
        List<Ruler> returnList = new List<Ruler>();
        foreach (Ruler ruler in EconomyController.Instance.rulerDictionary.Keys)
            if (ruler != null)
                if (ruler.xLocation == loc.GetPositionVector().x && ruler.yLocation == loc.GetPositionVector().y)
                    returnList.Add(ruler);
        return returnList;
    }


    public Ruler GetRulerFromLocation(string locname)
    {
        foreach (Ruler ruler in EconomyController.Instance.rulerDictionary.Keys)
            if (ruler.GetHomeLocation().elementID == locname && ruler.isLocalRuler == true)
                return ruler;

        Debug.Log("Did not find ruler for loc name: " + locname);
        return null;
    }


}
