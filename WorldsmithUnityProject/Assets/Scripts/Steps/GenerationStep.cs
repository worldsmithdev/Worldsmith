using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStep : Step
{

    bool randomizeAmount = true;

    public override void ConfigureStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        ClearStepVariables(territory);

        DetermineFood(territory);
        DetermineNatural(territory);
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        GenerateFood(territory);
        StoreFood(territory);
        GenerateNatural(territory);
        StoreNatural(territory);
    }

    public override void ResolveStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock; 
    }

    void DetermineFood (Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        float manpower;

        int housekeeperPop = 0;
        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper) housekeeperPop += population.peopleAmount;
        manpower = housekeeperPop;

        manpower *= WorldConstants.TERRITORYSIZE_MULTIPLIER[terr.territorySize];
        manpower *= WorldConstants.TERRITORYSOIL_MULTIPLIER[terr.territorySoilQuality];
        manpower *= WorldConstants.TERRITORYSTATUS_MULTIPLIER[terr.territoryStatus];         
        manpower *= (1 - WorldConstants.NATURAL_RESOURCES_RATE[loc.rateResource]);
        terr.cycleFoodManpower = manpower;   
    }
    void DetermineNatural(Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        float manpower;

        int housekeeperPop = 0;
        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper) housekeeperPop += population.peopleAmount;
        manpower = housekeeperPop;

        manpower *= WorldConstants.TERRITORYSIZE_MULTIPLIER[terr.territorySize];
        manpower *= WorldConstants.TERRITORYSOIL_MULTIPLIER[terr.territorySoilQuality];
        manpower *= WorldConstants.TERRITORYSTATUS_MULTIPLIER[terr.territoryStatus];
        manpower *= WorldConstants.NATURAL_RESOURCES_RATE[loc.rateResource];
        terr.cycleSecondaryManpower = manpower;
    }
    void GenerateFood(Territory terr)
    { 
        Resource generatedFood = Converter.GetResourceForManpower(Resource.Type.Wheat, terr.cycleFoodManpower);
        if (randomizeAmount == true)
            generatedFood.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
        terr.cycleGeneratedResources.Add(generatedFood.type, generatedFood.amount);
    }
    void GenerateNatural(Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr); 

        if (loc.primaryResourceType == Resource.Type.Unassigned) // Location has no specified resource, so generate default resources
        {
            foreach (Resource.Type restype in WorldConstants.DEFAULT_NATURAL_RESOURCETYPES.Keys)
            { 
                Resource createdResource = Converter.GetResourceForManpower(restype, (terr.cycleSecondaryManpower * WorldConstants.DEFAULT_NATURAL_RESOURCETYPES[restype]));
                if (randomizeAmount == true)
                    createdResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
                terr.cycleGeneratedResources.Add(createdResource.type, createdResource.amount);
            } 
        }
        else if (loc.primaryResourceType != Resource.Type.Unassigned && loc.secondaryResourceType == Resource.Type.Unassigned) // Location has one specific resource
        {
            if (Converter.RESOURCE_PER_MANPOWER_INDEX.ContainsKey(loc.primaryResourceType))
            {
                Resource createdResource = Converter.GetResourceForManpower(loc.primaryResourceType, terr.cycleSecondaryManpower);
                if (randomizeAmount == true)
                    createdResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
                terr.cycleGeneratedResources.Add(createdResource.type, createdResource.amount);
            } 
        }
        else // Location has primary and secondary resource specified
        {
            if (Converter.RESOURCE_PER_MANPOWER_INDEX.ContainsKey(loc.primaryResourceType) && Converter.RESOURCE_PER_MANPOWER_INDEX.ContainsKey(loc.secondaryResourceType))
            {
                Resource primaryCreatedResource = Converter.GetResourceForManpower(loc.primaryResourceType, (terr.cycleSecondaryManpower * (1 - WorldConstants.NATURALRESOURCEPERCENTAGE)));
                Resource secondaryCreatedResource = Converter.GetResourceForManpower(loc.secondaryResourceType, (terr.cycleSecondaryManpower * WorldConstants.NATURALRESOURCEPERCENTAGE));
                if (randomizeAmount == true)
                {
                    primaryCreatedResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
                    secondaryCreatedResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
                }
                terr.cycleGeneratedResources.Add(primaryCreatedResource.type, primaryCreatedResource.amount);
                terr.cycleGeneratedResources.Add(secondaryCreatedResource.type, secondaryCreatedResource.amount); 
            }
        } 
    }
    void StoreFood(Territory terr)
    {
        foreach (Resource.Type restype in terr.cycleGeneratedResources.Keys)
            if (ResourceController.Instance.resourceCompendium[restype] == Resource.Category.Farmed)
                 terr.storedResources[restype] += terr.cycleGeneratedResources[restype];
        
    }
    void StoreNatural(Territory terr)
    {
        foreach (Resource.Type restype in terr.cycleGeneratedResources.Keys)
            if (ResourceController.Instance.resourceCompendium[restype] == Resource.Category.Natural)
                terr.storedResources[restype] += terr.cycleGeneratedResources[restype];
    }
    void ClearStepVariables (Territory terr)
    { 
        terr.cycleGeneratedResources = new Dictionary<Resource.Type, float>();
    }



}
