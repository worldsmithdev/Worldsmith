using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalExchangeStep : Step
{
    public override void ConfigureStep(EcoBlock ecoblock)
    {
        if (ecoblock.blockType == EcoBlock.BlockType.Population)
        {
            Population population = (Population)ecoblock;
            ClearStepVariables(population);
            DetermineSurplusResources(population);
            DetermineWantedResources(population);

        }
        else if (ecoblock.blockType == EcoBlock.BlockType.Ruler)
        {
            Ruler ruler = (Ruler)ecoblock;
            ClearStepVariables(ruler);
            DetermineSurplusResources(ruler);
            DetermineWantedResources(ruler);

        }

    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        if (ecoblock.blockType == EcoBlock.BlockType.Population)
        {
            Population population = (Population)ecoblock; 

        }
        else if (ecoblock.blockType == EcoBlock.BlockType.Ruler)
        {
            Ruler ruler = (Ruler)ecoblock; 

        }
    }

    public override void ResolveStep()
    {


    }

    void DetermineSurplusResources (Population population)
    {
        Dictionary<Resource.Type, float> reservationList = new Dictionary<Resource.Type, float>();
        float foodAmountToKeep = population.cycleDesiredFoodConsumption * WorldConstants.POPULATION_FOOD_STORAGE;
        float waresAmountToKeep = population.cycleDesiredLuxuryConsumption * WorldConstants.POPULATION_WARES_STORAGE;
        // add reserved content

 
        // resolve:
        foreach (Resource.Type restype in population.resourcePortfolio.Keys)
        {
            if (reservationList.ContainsKey(restype))
            {
                if (population.resourcePortfolio[restype].amount > reservationList[restype])
                {
                    population.cycleLocalSurplusResources.Add(restype, (population.resourcePortfolio[restype].amount - reservationList[restype]));
                }
            }
            else
            {
                population.cycleLocalSurplusResources.Add(restype, population.resourcePortfolio[restype].amount);
            } 
        } 
    }
    void DetermineSurplusResources(Ruler ruler)
    {

    }
    void DetermineWantedResources(Population population)
    {
        
    }
    void DetermineWantedResources(Ruler ruler)
    {

    }
    void ClearStepVariables(Population population)
    {
        population.cycleLocalSurplusResources = new Dictionary<Resource.Type, float>();
    }
    void ClearStepVariables(Ruler ruler)
    {
        ruler.cycleLocalSurplusResources = new Dictionary<Resource.Type, float>();
    }
}
