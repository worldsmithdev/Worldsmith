using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location : WorldElement
{
    
    public enum LocationType {  Settled, Productive, Defensive, Cultural, Natural}
    public enum LocationSubType {  Unassigned, Polis, UrbanCenter, Stronghold, Village,      Sanctuary, Hazard }
    
    // EcoBlock variables
    public Ruler localRuler;
    public Ruler dominatingRuler;
    public List<Ruler> secondaryRulers = new List<Ruler>();
    public Territory locationTerritory;
    public List<Population> populationList = new List<Population>();

    // Column Variables
    protected LocationType locType;
    protected LocationSubType locSubType;
    public int populationSize;
    public int founded;
    public int originalFaction;
    public int currentFaction;
    public int greekType;
    public int hellenized;
    public int territorySize;
    public int territorySoil;
    public int territoryStatus;
    public int focusFood;
    public int focusBuild;
    public int rateBuild;
    public int rateResource;
    public int rateIndustry;
    public int rateMinting;
    public string resourcePrimary;
    public string resourceSecondary;
    public string industryPrimary;
    public string industrySecondary;
    public int harborQuality;
    public int hubType;
    public string offloadOne;
    public string offloadTwo;
    public string offloadThree;
    public string offloadResource;
    public int offloadDifficultyOne;
    public int offloadDifficultyTwo;
    public int offloadDifficultyThree;
    public string summary;
    public int politicalHierarchy;
    public string[] dominateStrings;
    public int authorityPolitical;
    public int attitudePolitical;
    public int challengePolitical;
    public string rival;
    public string friend;
    public int religiousImportance;
    public string cultPrimary;
    public string cultSecondary;
    public string coinageEmblem;
    public int height;
    public int naturalDefense;
    public int fortification;
    public int fightersPrimary;
    public int fightersSecondary;
    public int strategicPrimary;
    public int strategicSecondary;
    public string terrainString;
    public string mythString;
    public string constructionString;
    public int surety;

    // Steps
    public LocalMarket localMarket;
    public RegionalMarket regionalMarket;
    public GlobalMarket globalMarket;

    public Resource.Type primaryResourceType = Resource.Type.Unassigned;
    public Resource.Type secondaryResourceType = Resource.Type.Unassigned;
    public Location()
    {

    }
    public Location(LocationType type, LocationSubType subtype, string name)
    {
        elementType = ElementType.Location;
        locType = type;
        elementID = name;
        locSubType = subtype;    
    }

    public void SetLocationType (LocationType type)
    {
        locType = type;
    }
    public void SetLocationSubType(LocationSubType type)
    {
        locSubType = type;
    }
    public LocationType GetLocationType()
    {
        return locType;
    }

    public LocationSubType GetLocationSubType()
    {
        return locSubType;
    }

    public virtual void SetSpecialProperties()
    {
        if (resourcePrimary != "")
            primaryResourceType = (Resource.Type)System.Enum.Parse(typeof(Resource.Type), resourcePrimary);
        if (resourceSecondary != "")
            secondaryResourceType = (Resource.Type)System.Enum.Parse(typeof(Resource.Type), resourceSecondary); 
    }

    public int GetTotalPopulation()
    {
        int total = 0;
        foreach (Population pop in populationList)
            total += pop.amount;

        return total;
    }
}
