using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Faction : WorldElement
{
      
    public List<Character> factionCharacters = new List<Character>();

    public Faction (string name)
    {
        elementType = ElementType.Faction;
        elementID = name;

        int nr = 1;
        elementID = name;
        while (WorldController.Instance.ElementIDExists(this.elementID, this.elementType) == true)
        {
            nr++;
            elementID = name + nr;
        }

    }

}
