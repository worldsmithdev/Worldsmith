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


    public List<Ruler> GetLocationRulers(Location loc)
    {
        List<Ruler> returnList = new List<Ruler>();

        foreach (Ruler ruler in loc.secondaryRulers)
            if (ruler != null)
                returnList.Add(ruler);

        returnList.Add(loc.localRuler);
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

    public Ruler GetRulerFromID(string id)
    {
        foreach (Ruler ruler in EconomyController.Instance.rulerDictionary.Keys)
            if (ruler.blockID == id)
                return ruler;
        Debug.Log("Tried to find Ruler but no id match for " + id);
        return null;
    }
}
