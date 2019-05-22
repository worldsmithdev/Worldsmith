using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustryStep : Step
{

    public override void ConfigureStep(EcoBlock ecoblock)
    {
        Population population = (Population)ecoblock;
        ClearStepVariables(population);

        DetermineWares(population); 
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Population population = (Population)ecoblock;
        CreateWares(population);  
    }

    public override void ResolveStep()
    { 

    }

    void DetermineWares (Population population)
    {
        if (population.laborType == Population.LaborType.Artisan)
        {
            Location loc = population.GetHomeLocation();
            float manpower = population.amount;

            if (loc.hubType == 3 || loc.hubType == 5)
                manpower *= WorldConstants.WARES_FOCUS_BONUS;

            manpower *= WorldConstants.INDUSTRY_RATE_MODIFIER[loc.rateIndustry];
            population.cycleWaresManpower = manpower;
        }
               
    }

    void CreateWares(Population population)
    {
        Resource createdWares = Converter.GetResourceForManpower(Resource.Type.Wares, population.cycleWaresManpower);

        population.cycleCreatedResources.Add(createdWares.type, createdWares.amount);

        foreach (Resource.Type restype in population.cycleCreatedResources.Keys)
            if (ResourceController.Instance.resourceCompendium[restype] == Resource.Category.Manufactured)
                population.cycleAvailableResources[restype] += population.cycleCreatedResources[restype];
    }
    void ClearStepVariables(Population population)
    {
        population.cycleCreatedResources = new Dictionary<Resource.Type, float>(); 
    }
}
