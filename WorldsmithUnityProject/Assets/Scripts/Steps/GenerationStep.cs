using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStep : Step
{
    public override void ConfigureStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        ClearGeneratedResources(territory);

        DetermineFoodGeneration(territory);
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        GenerateResources(territory);

    }

    public override void ResolveStep(EcoBlock ecoblock)
    {
        Territory territory = (Territory)ecoblock;
        

    }

    void DetermineFoodGeneration (Territory terr)
    {
        Location loc = TerritoryController.Instance.GetLocationForTerritory(terr);
        int housekeeperPop = 0;
        float baseAmount = 0; 
        float totalnaturaloutput = 0;

        foreach (Population population in loc.populationList)
            if (population.laborType == Population.LaborType.Housekeeper) housekeeperPop += population.peopleAmount;

        baseAmount += housekeeperPop;

        baseAmount *= WorldConstants.TERRITORYSIZE_MULTIPLIER[terr.territorySize];
        baseAmount *= WorldConstants.TERRITORYSOIL_MULTIPLIER[terr.territorySoilQuality];
        baseAmount *= WorldConstants.TERRITORYSTATUS_MULTIPLIER[terr.territoryStatus];
        totalnaturaloutput += (WorldConstants.RATE_MULTIPLIER[loc.rateResource] * baseAmount);
        baseAmount *= (1 - WorldConstants.RATE_MULTIPLIER[loc.rateResource]);

        terr.cycleFoodGeneration = baseAmount; 
    //    terr.CycleSpecifiedGeneration = totalnaturaloutput;




    }

    void GenerateResources(Territory terr)
    {
        terr.GenerateFoodResources();

      
    }

    void ClearGeneratedResources (Territory terr)
    { 
        terr.cycleGeneratedResources = new Dictionary<Resource.Type, float>();
    }



}
