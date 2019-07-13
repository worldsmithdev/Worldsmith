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
        GenerateNatural(territory); 
    }

    public override void ResolveStep()
    { 
    }

    void DetermineFood (Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        float manpower;

        int housekeeperPop = 0;
        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper) housekeeperPop += population.amount;
        manpower = housekeeperPop;
         
        manpower *= (1 - WorldConstants.GENERATION_RATES_MODIFIER[loc.rateBuild]);
        manpower *= (1 - WorldConstants.GENERATION_RATES_MODIFIER[loc.rateResource]);
        manpower *= WorldConstants.TERRITORYSIZE_MULTIPLIER[terr.territorySize];
        manpower *= WorldConstants.TERRITORYSOIL_MULTIPLIER[terr.territorySoilQuality];
        manpower *= WorldConstants.TERRITORYSTATUS_MULTIPLIER[terr.territoryStatus];
         
        terr.cycleFarmedManpower = manpower;   
    }
    void DetermineNatural(Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        float manpower;

        int housekeeperPop = 0;
        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper) housekeeperPop += population.amount;
        manpower = housekeeperPop;

        manpower *= (1 - WorldConstants.GENERATION_RATES_MODIFIER[loc.rateBuild]);
        manpower *= WorldConstants.GENERATION_RATES_MODIFIER[loc.rateResource];
        manpower *= WorldConstants.TERRITORYSIZE_MULTIPLIER[terr.territorySize];
        manpower *= WorldConstants.TERRITORYSOIL_MULTIPLIER[terr.territorySoilQuality];
        manpower *= WorldConstants.TERRITORYSTATUS_MULTIPLIER[terr.territoryStatus];
        terr.cycleNaturalManpower = manpower;
    }
    void GenerateFood(Territory terr)
    { 
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        Population habitantHousekeepers = null;
        Population citizenHousekeepers = null;
        bool hasCitizenHousekeepers = false;
        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper && population.classType != Population.ClassType.Slave)
            {
                if (population.classType == Population.ClassType.Citizen)
                {
                    hasCitizenHousekeepers = true;
                    citizenHousekeepers = population;
                }
                else if (population.classType == Population.ClassType.Habitant)
                {
                    habitantHousekeepers = population;
                }
            }

        Resource generatedFood = Converter.GetResourceForManpower(Resource.Type.Wheat, terr.cycleFarmedManpower);
        if (randomizeAmount == true)
            generatedFood.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);

        if (habitantHousekeepers == null)
            Debug.LogWarning("No Habitant Houseekepers found for Location " + loc.elementID);

        if (hasCitizenHousekeepers == false)
        {
            habitantHousekeepers.cycleGeneratedResources.Add(generatedFood.type, generatedFood.amount);
            habitantHousekeepers.cycleAvailableResources[generatedFood.type] += generatedFood.amount ;
        }
        else
        {
            habitantHousekeepers.cycleGeneratedResources.Add(generatedFood.type, generatedFood.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE) );
            habitantHousekeepers.cycleAvailableResources[generatedFood.type] += generatedFood.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE) ;
            citizenHousekeepers.cycleGeneratedResources.Add(generatedFood.type, generatedFood.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
            citizenHousekeepers.cycleAvailableResources[generatedFood.type] += generatedFood.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE;
        }  
    }
    void GenerateNatural(Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        Population habitantHousekeepers = null;
        Population citizenHousekeepers = null;
        bool hasCitizenHousekeepers = false;
        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper && population.classType != Population.ClassType.Slave)
            {
                if (population.classType == Population.ClassType.Citizen)
                {
                    hasCitizenHousekeepers = true;
                    citizenHousekeepers = population;
                }
                else if (population.classType == Population.ClassType.Habitant)
                {
                    habitantHousekeepers = population;
                }
            }
        if (habitantHousekeepers == null)
            Debug.LogWarning("No Habitant Houseekepers found for Location " + loc.elementID);

        if (loc.primaryResourceType == Resource.Type.Unassigned) // Location has no specified resource, so generate default resources
        {
            foreach (Resource.Type restype in WorldConstants.DEFAULT_NATURAL_RESOURCETYPES.Keys)
            { 
                Resource createdResource = Converter.GetResourceForManpower(restype, (terr.cycleNaturalManpower * WorldConstants.DEFAULT_NATURAL_RESOURCETYPES[restype]));
                if (randomizeAmount == true)
                    createdResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);

                if (hasCitizenHousekeepers == false)
                {
                    habitantHousekeepers.cycleGeneratedResources.Add(createdResource.type, createdResource.amount);
                    habitantHousekeepers.cycleAvailableResources[createdResource.type] += createdResource.amount;
                }
                else
                {
                    habitantHousekeepers.cycleGeneratedResources.Add(createdResource.type, createdResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE));
                    habitantHousekeepers.cycleAvailableResources[createdResource.type] += createdResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleGeneratedResources.Add(createdResource.type, createdResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleAvailableResources[createdResource.type] += createdResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE;
                } 
            } 
        }
        else if (loc.primaryResourceType != Resource.Type.Unassigned && loc.secondaryResourceType == Resource.Type.Unassigned) // Location has one specific resource
        {
            if (Converter.RESOURCE_PER_MANPOWER_INDEX.ContainsKey(loc.primaryResourceType))
            {
                Resource createdResource = Converter.GetResourceForManpower(loc.primaryResourceType, terr.cycleNaturalManpower);
                if (randomizeAmount == true)
                    createdResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);

                if (hasCitizenHousekeepers == false)
                {
                    habitantHousekeepers.cycleGeneratedResources.Add(createdResource.type, createdResource.amount);
                    habitantHousekeepers.cycleAvailableResources[createdResource.type] += createdResource.amount;
                }
                else
                {
                    habitantHousekeepers.cycleGeneratedResources.Add(createdResource.type, createdResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE));
                    habitantHousekeepers.cycleAvailableResources[createdResource.type] += createdResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleGeneratedResources.Add(createdResource.type, createdResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleAvailableResources[createdResource.type] += createdResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE;
                }
            } 
        }
        else // Location has primary and secondary resource specified
        {
            if (Converter.RESOURCE_PER_MANPOWER_INDEX.ContainsKey(loc.primaryResourceType) && Converter.RESOURCE_PER_MANPOWER_INDEX.ContainsKey(loc.secondaryResourceType))
            {
                Resource primaryCreatedResource = Converter.GetResourceForManpower(loc.primaryResourceType, (terr.cycleNaturalManpower * (1 - WorldConstants.NATURAL_RESOURCE_PERCENTAGE)));
                Resource secondaryCreatedResource = Converter.GetResourceForManpower(loc.secondaryResourceType, (terr.cycleNaturalManpower * WorldConstants.NATURAL_RESOURCE_PERCENTAGE));
                if (randomizeAmount == true)
                {
                    primaryCreatedResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
                    secondaryCreatedResource.amount *= Random.Range(WorldConstants.GENERATION_RANDOMIZER_LO, WorldConstants.GENERATION_RANDOMIZER_HI);
                }
                if (hasCitizenHousekeepers == false)
                {
                    habitantHousekeepers.cycleGeneratedResources.Add(primaryCreatedResource.type, primaryCreatedResource.amount);
                    habitantHousekeepers.cycleGeneratedResources.Add(secondaryCreatedResource.type, secondaryCreatedResource.amount);
                    habitantHousekeepers.cycleAvailableResources[primaryCreatedResource.type] += primaryCreatedResource.amount;
                    habitantHousekeepers.cycleAvailableResources[secondaryCreatedResource.type] += secondaryCreatedResource.amount;
                }
                else
                {
                    habitantHousekeepers.cycleGeneratedResources.Add(primaryCreatedResource.type, primaryCreatedResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE));
                    habitantHousekeepers.cycleAvailableResources[primaryCreatedResource.type] += primaryCreatedResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleGeneratedResources.Add(primaryCreatedResource.type, primaryCreatedResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleAvailableResources[primaryCreatedResource.type] += primaryCreatedResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE;
                    habitantHousekeepers.cycleGeneratedResources.Add(secondaryCreatedResource.type, secondaryCreatedResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE));
                    habitantHousekeepers.cycleAvailableResources[secondaryCreatedResource.type] += secondaryCreatedResource.amount * (1 - WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleGeneratedResources.Add(secondaryCreatedResource.type, secondaryCreatedResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE);
                    citizenHousekeepers.cycleAvailableResources[secondaryCreatedResource.type] += secondaryCreatedResource.amount * WorldConstants.CITIZEN_GENERATION_ADVANTAGE;
                } 
            }
        } 
    }
 
    void ClearStepVariables (Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        foreach (Population population in loc.populationList)
        {
            population.cycleGeneratedResources = new Dictionary<Resource.Type, float>();
            population.cycleAvailableResources = new Dictionary<Resource.Type, float>();
            population.cyclePaidLevyResources = new List<Resource>();
            foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)            
                population.cycleAvailableResources.Add(restype, 0); 
        } 
    }



}
