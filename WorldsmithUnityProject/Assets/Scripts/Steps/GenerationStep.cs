using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStep : Step
{
    public override void ConfigureStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        ClearGeneratedResources(territory);

        DetermineFood(territory);
        DetermineSecondary(territory);
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        GenerateFood(territory);
        GenerateSecondary(territory); 

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
        manpower *= (1 - WorldConstants.SECONDARY_RATE[loc.rateResource]);
        terr.cycleStoredFood = ResourceConverter.GetResourceForManpower (Resource.Type.Wheat, manpower).amount;   
    }
    void DetermineSecondary(Territory terr)
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
        manpower *=  WorldConstants.SECONDARY_RATE[loc.rateResource];


        // add in primary and secondary
        terr.cycleStoredFood = ResourceConverter.GetResourceForManpower(Resource.Type.Wheat, manpower).amount;
    }
    void GenerateFood(Territory terr)
    {
        terr.GenerateFoodResources();

      
    }
    void GenerateSecondary(Territory terr)
    {  
    }

    void ClearGeneratedResources (Territory terr)
    { 
        terr.cycleGeneratedResources = new Dictionary<Resource.Type, float>();
    }



}
