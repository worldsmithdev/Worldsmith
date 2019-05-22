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
        population.cycleDesiredLuxuryConsumption = population.amount * WorldConstants.FOOD_CONSUMPTION_RATE;

    }
    void ConsumeFood(Population population)
    {

    }
    void ConsumeLuxury(Population population)
    {

    }

    void ClearStepVariables(Population population)
    {
 
    }
}
