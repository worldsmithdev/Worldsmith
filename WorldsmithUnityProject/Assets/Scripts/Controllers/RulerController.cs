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

    Ruler selectedRuler;

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


    public void SetSelectedRuler (Ruler ruler)
    {
        selectedRuler = ruler;
    }
    public Ruler GetSelectedRuler()
    {
        return selectedRuler;
    }


    public List<Ruler> GetShuffledList()
    {
        List<Ruler> shuffledList = new List<Ruler>();

        foreach (Ruler ruler in EconomyController.Instance.rulerDictionary.Keys)
            shuffledList.Add(ruler);

        //    int shuffledAmount = Random.Range(0, shuffledList.Count);

        for (int i = 0; i < shuffledList.Count - 2; i++)
        {
            int finalSpot = shuffledList.Count - 1;
            Ruler movedBlock = shuffledList[finalSpot];
            shuffledList.Remove(movedBlock);
            shuffledList.Insert(Random.Range(0, shuffledList.Count - 1), movedBlock);
        }
        for (int i = shuffledList.Count - 1; i > 1; i--)
        {
            Ruler movedBlock = shuffledList[0];
            shuffledList.Remove(movedBlock);
            shuffledList.Insert(Random.Range(1, shuffledList.Count - 1), movedBlock);
        } 
        return shuffledList; 
    }
}
