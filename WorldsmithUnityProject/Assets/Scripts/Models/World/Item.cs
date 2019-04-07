using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : WorldElement
{
      
    public Item (string name)
    {
        elementType = ElementType.Item;
        elementID = name;

      

        int nr = 1;
        elementID = name;
        while (WorldController.Instance.ElementIDExists(this.elementID, this.elementType) == true)
        {
            nr++;
            elementID = name + nr;
        }
    }


    public void SetLocation(string locstring)
    {
        if (locstring != "")
        {
            this.SetXLocation(LocationController.Instance.GetSpecificLocation(locstring).GetPositionVector().x);
            this.SetYLocation(LocationController.Instance.GetSpecificLocation(locstring).GetPositionVector().y);
        }
    }
}
