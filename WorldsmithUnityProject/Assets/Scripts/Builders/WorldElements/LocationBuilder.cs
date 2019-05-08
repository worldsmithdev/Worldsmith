﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBuilder : MonoBehaviour
{
    public LocationImports locationImports;

    Location.LocationType locType;
    Location.LocationSubType locSubType;

    public void CreateLocations(World activeWorld)
    {
        // Loop through each data entry of the LocationImports file, then draw the Location's information from this entry called locdata. 
        foreach (LocationImportsData locdata in locationImports.dataArray)
        {
            if (locdata.Name != "")
            {
                if (locdata.Locationtype != "")
                    locType = (Location.LocationType)System.Enum.Parse(typeof(Location.LocationType), locdata.Locationtype);
                else
                    Debug.Log("Trying to create a location: " + locdata.Name + " that has no locationtype: " + locdata.Locationtype );

                if (locdata.Locationsubtype != "")
                    locSubType = (Location.LocationSubType)System.Enum.Parse(typeof(Location.LocationSubType), locdata.Locationsubtype);
                else
                    locSubType = Location.LocationSubType.Unassigned;
                
                if (locSubType == Location.LocationSubType.City)
                {
                    City createdLoc = new City(locType, locSubType, locdata.Name);
                    AssignLocationVariables(locdata, createdLoc);
                    activeWorld.locationList.Add(createdLoc);
                }

                else
                {
                    Location createdLoc = new Location(locType, locSubType, locdata.Name);
                    AssignLocationVariables(locdata, createdLoc);
                    activeWorld.locationList.Add(createdLoc);
                }
            }
        }
    }

    // Assign all other variables, that are not passed through a constructor
    void AssignLocationVariables(LocationImportsData locdata, Location loc)
    {
        loc.description = locdata.Description;
        loc.culturalType = locdata.Culturetype;
        loc.politicalType = locdata.Politicaltype;
        loc.economicType = locdata.Economictype;
        if (locdata.Dominates != "")
            loc.dominateStrings = locdata.Dominates.Split(',');

        loc.SetSpecialProperties();
    }
}
