using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Location
{



    public City(LocationType type, LocationSubType subtype, string name)
    {
        elementType = ElementType.Location;
        locType = type;
        elementID = name;
        locSubType = subtype;
    }


}
