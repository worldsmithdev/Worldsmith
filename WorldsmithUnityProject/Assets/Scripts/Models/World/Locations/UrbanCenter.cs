using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrbanCenter : Location
{
    public UrbanCenter(LocationType type, LocationSubType subtype, string name)
    {
        elementType = ElementType.Location;
        locType = type;
        elementID = name;
        locSubType = subtype;
    }
}