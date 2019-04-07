using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Law  : WorldElement 
{
      
    public Ruler lawRuler;
    public Location lawLocation;

    public Law(string name)
    {
        elementType = ElementType.Law;  
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
