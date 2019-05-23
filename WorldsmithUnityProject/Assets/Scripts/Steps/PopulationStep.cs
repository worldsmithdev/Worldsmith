using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationStep : Step
{
    public override void ConfigureStep (EcoBlock ecoblock)
    {
        Population population = (Population)ecoblock;
        ClearStepVariables(population);

        DetermineFoodConsumption(population);
        DetermineLuxuryConsumption(population);
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Population population = (Population)ecoblock;
        ConsumeFood(population);
        ConsumeLuxury(population);
    }

    public override void ResolveStep()
    {
        

    }

    void DetermineFoodConsumption(Population population)
    {
        population.cycleDesiredFoodConsumption = population.amount * WorldConstants.FOOD_CONSUMPTION_RATE;
    }
    void DetermineLuxuryConsumption(Population population)
    {
        population.cycleDesiredLuxuryConsumption = population.amount * WorldConstants.LUXURY_CONSUMPTION_RATE;

    }
    void ConsumeFood(Population population)
    {
        if (population.resourcePortfolio[Resource.Type.Wheat].amount >= population.cycleDesiredFoodConsumption) // Enough food available
        {
            population.cycleActualFoodConsumption = population.cycleDesiredFoodConsumption;
            population.successiveFoodConsumptionShortage = 0;
            population.foodConsumptionShortageList.Add(0);            
        }
        else if (population.resourcePortfolio[Resource.Type.Wheat].amount < population.cycleDesiredFoodConsumption) // Too little food available
        {
            population.cycleActualFoodConsumption = population.resourcePortfolio[Resource.Type.Wheat].amount;
            population.successiveFoodConsumptionShortage += 1;
            population.foodConsumptionShortageList.Add( (population.cycleDesiredFoodConsumption  - population.cycleActualFoodConsumption));
        }
        population.resourcePortfolio[Resource.Type.Wheat].amount -= population.cycleActualFoodConsumption;
    }
    void ConsumeLuxury(Population population)
    {
        if (population.resourcePortfolio[Resource.Type.Wares].amount >= population.cycleDesiredLuxuryConsumption) // Enough food available
        {
            population.cycleActualLuxuryConsumption = population.cycleDesiredLuxuryConsumption;
            population.successiveLuxuryConsumptionShortage = 0;
            population.luxuryConsumptionShortageList.Add(0);
        }
        else if (population.resourcePortfolio[Resource.Type.Wares].amount < population.cycleDesiredLuxuryConsumption) // Too little food available
        {
            population.cycleActualLuxuryConsumption = population.resourcePortfolio[Resource.Type.Wares].amount;
            population.successiveLuxuryConsumptionShortage += 1;
            population.luxuryConsumptionShortageList.Add((population.cycleDesiredLuxuryConsumption - population.cycleActualLuxuryConsumption));
        } 
        population.resourcePortfolio[Resource.Type.Wares].amount -= population.cycleActualLuxuryConsumption;
    }

    void ClearStepVariables(Population population)
    {
 

    }
}
