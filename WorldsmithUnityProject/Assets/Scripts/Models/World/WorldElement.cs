using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldElement  
{

    public enum ElementType {Unassigned, Location, God, Item, Character, Creature, Faction, Law, Story  }

     float xLocation;
     float yLocation;

    public ElementType elementType;
    public string elementID;
    public string description;

    public Vector3 GetPositionVector()
    {
        return new Vector3(xLocation, yLocation, 0);
    }

    public void SetXLocation (float x)
    {
        xLocation = x;
    }
    public void SetYLocation(float y)
    {
        yLocation = y;
    }
}
