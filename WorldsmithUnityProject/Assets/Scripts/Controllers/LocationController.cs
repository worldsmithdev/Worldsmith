using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    // Tracks the application's selected location for the Location Section and ExploreScreen. Same for Building - will probably have its own Controller later.

    public static LocationController Instance { get; protected set; }

    [HideInInspector]
    public Dictionary<Location.LocationSubType, Location.LocationType> locationTypePairings = new Dictionary<Location.LocationSubType, Location.LocationType>();


    public string preselectLocationName;

    Location selectedLocation; 
    Building.BuildingType selectedBuildingType;


    private void Awake()
    {
        Instance = this; 

        locationTypePairings.Add(Location.LocationSubType.City, Location.LocationType.Settled);
        locationTypePairings.Add(Location.LocationSubType.Town, Location.LocationType.Settled); 
        locationTypePairings.Add(Location.LocationSubType.Village, Location.LocationType.Settled);
        locationTypePairings.Add(Location.LocationSubType.Homestead, Location.LocationType.Settled);
        locationTypePairings.Add(Location.LocationSubType.Dwelling, Location.LocationType.Settled);
        locationTypePairings.Add(Location.LocationSubType.Mine, Location.LocationType.Productive);
        locationTypePairings.Add(Location.LocationSubType.Outpost, Location.LocationType.Defensive);
        locationTypePairings.Add(Location.LocationSubType.Sanctuary, Location.LocationType.Cultural); 
        locationTypePairings.Add(Location.LocationSubType.Hazard , Location.LocationType.Natural);

    }

  
    // Setting a location selected on start to save a bit of exception-coding
    public void SetPreSelectedLocation()
    {        
        if (ContainerController.Instance.locationContainerList.Count > 0)
        {
            if (preselectLocationName == "")
                Debug.Log("No preselect location set - please do so in LocationController - Preselect Location Name");
            else
             selectedLocation = GetSpecificLocation(preselectLocationName);
        }

    } 
    public void SetSelectedLocation(Location loc)
    {
        selectedLocation = loc;
        selectedBuildingType = Building.BuildingType.Unassigned;
        UIController.Instance.RefreshUI();
    }
    public void SetSelectedBuildingType(Building.BuildingType type)
    {
        selectedBuildingType = type; 
        UIController.Instance.RefreshUI();
    } 
    public Location GetSelectedLocation()
    {
        return selectedLocation;
    }
    public Building.BuildingType GetSelectedBuildingType()
    {
        return selectedBuildingType;
    }

    public Location GetSpecificLocation(string locname)
    {
        foreach (Location location in WorldController.Instance.GetWorld().locationList) 
            if (location.elementID == locname)
                return location; 
          
        Debug.Log("Did not find location for name: " + locname);
        return null;
    }




}
