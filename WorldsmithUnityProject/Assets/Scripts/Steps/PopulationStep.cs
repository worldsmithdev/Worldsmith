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

   public void DetermineFoodConsumption(Population population)
    {
        population.cycleDesiredFoodConsumption = population.amount * WorldConstants.FOOD_CONSUMPTION_RATE;
    }
   public  void DetermineLuxuryConsumption(Population population)
    {
        population.cycleDesiredComfortConsumption = population.amount * WorldConstants.LUXURY_CONSUMPTION_RATE;

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
        if (population.resourcePortfolio[Resource.Type.Wares].amount >= population.cycleDesiredComfortConsumption) // Enough food available
        {
            population.cycleActualComfortConsumption = population.cycleDesiredComfortConsumption;
            population.successiveComfortConsumptionShortage = 0;
            population.comfortConsumptionShortageList.Add(0);
        }
        else if (population.resourcePortfolio[Resource.Type.Wares].amount < population.cycleDesiredComfortConsumption) // Too little food available
        {
            population.cycleActualComfortConsumption = population.resourcePortfolio[Resource.Type.Wares].amount;
            population.successiveComfortConsumptionShortage += 1;
            population.comfortConsumptionShortageList.Add((population.cycleDesiredComfortConsumption - population.cycleActualComfortConsumption));
        } 
        population.resourcePortfolio[Resource.Type.Wares].amount -= population.cycleActualComfortConsumption;
    }

    void ClearStepVariables(Population population)
    {
 

    }
}
