using System.Collections;
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
                
                if (locSubType == Location.LocationSubType.Polis)
                {
                    Polis createdLoc = new Polis(locType, locSubType, locdata.Name);
                    AssignLocationVariables(locdata, createdLoc);
                    activeWorld.locationList.Add(createdLoc);
                }
                else if (locSubType == Location.LocationSubType.UrbanCenter)
                {
                    UrbanCenter createdLoc = new UrbanCenter(locType, locSubType, locdata.Name);
                    AssignLocationVariables(locdata, createdLoc);
                    activeWorld.locationList.Add(createdLoc);
                }
                else if (locSubType == Location.LocationSubType.Stronghold)
                {
                    Stronghold createdLoc = new Stronghold(locType, locSubType, locdata.Name);
                    AssignLocationVariables(locdata, createdLoc);
                    activeWorld.locationList.Add(createdLoc);
                }
                else if (locSubType == Location.LocationSubType.Village)
                {
                    Village createdLoc = new Village(locType, locSubType, locdata.Name);
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


        loc.populationSize = locdata.Populationsize;
        loc.founded = locdata.Founded;
        loc.originalFaction = locdata.Originalfaction;
        loc.currentFaction = locdata.Currentfaction;
        loc.greekType = locdata.Greektype;
        loc.hellenized = locdata.Hellenized;
        loc.territorySoil = locdata.Territorysoil;
        loc.territoryStatus = locdata.Territorystatus;
        loc.focusFood = locdata.Foodfocus;
        loc.focusBuild = locdata.Buildfocus;
        loc.rateBuild = locdata.Buildrate;
        loc.rateResource = locdata.Resourcerate;
        loc.rateIndustry = locdata.Industryrate;
        loc.rateMinting = locdata.Mintingrate;
        loc.resourcePrimary = locdata.Primaryresource;
        loc.recourceSecondary = locdata.Secondaryresource;
        loc.industryPrimary = locdata.Primaryindustry;
        loc.industrySecondary = locdata.Secondaryindustry;
        loc.harborQuality = locdata.Harborquality;
        loc.hubType = locdata.Hubtype;
        loc.offloadPrimary = locdata.Primaryoffload;
        loc.offloadSecondary = locdata.Secondaryoffload;
        loc.offloadResource = locdata.Resourceoffload;
        loc.offloadDifficultyPrimary = locdata.Primarydifficulty;
        loc.offloadDifficultySecondary = locdata.Secondarydifficulty;
        loc.summary = locdata.Summary;
        loc.politicalHierarchy = locdata.Politicalhierarchy;
        loc.authorityPolitical = locdata.Politicalauthority;
        loc.attitudePolitical = locdata.Politicalattitude;
        loc.challengePolitical = locdata.Politicalchallenge;
        loc.rival = locdata.Rival;
        loc.friend = locdata.Friend;
        loc.religiousImportance = locdata.Religiousimportance;
        loc.cultPrimary = locdata.Primarycult;
        loc.cultSecondary = locdata.Secondarycult;
        loc.coinageEmblem = locdata.Coinageemblem;
        loc.height = locdata.Height;
        loc.naturalDefense = locdata.Naturaldefense;
        loc.fortification = locdata.Fortification;
        loc.fightersPrimary = locdata.Primaryfighters;
        loc.fightersSecondary = locdata.Secondaryfighters;
        loc.strategicPrimary = locdata.Primarystrategic;
        loc.strategicSecondary = locdata.Secondarystrategic;
        loc.terrainString = locdata.Terrainstring;
        loc.mythString = locdata.Mythstring;
        loc.constructionString = locdata.Constructionstring;
        loc.surety = locdata.Surety;

        if (locdata.Dominates != "")
            loc.dominateStrings = locdata.Dominates.Split(',');

        loc.SetSpecialProperties();
    }
}
