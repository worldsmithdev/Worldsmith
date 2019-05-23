using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
{
    public enum BuildingType { Unassigned, Citadel, Dwelling, Market, Granary , Walls, Temple }

    public string buildingName;
    public BuildingType buildingType;
}
