using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalExchangeStep : Step
{
     

    public override void PrepareStep()
    {
        MarketController.Instance.cycleLocalMarkets = new List<LocalMarket>();
        MarketController.Instance.cycleLocalExchanges = new List<LocalExchange>();
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            loc.localMarket = new LocalMarket(loc); 
    }
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
            RegisterToMarket(population);

        }
        else if (ecoblock.blockType == EcoBlock.BlockType.Ruler)
        {
            Ruler ruler = (Ruler)ecoblock;
            RegisterToMarket(ruler);

        }
    }

    public override void ResolveStep()
    {
        CheckForSurplusDesiredOverlap();

        foreach (LocalMarket locMarket in MarketController.Instance.cycleLocalMarkets)  //WorldController.Instance.GetWorld().locationList)        
         locMarket.ResolveMarket();

        foreach (LocalExchange localExchange in MarketController.Instance.cycleLocalExchanges)        
            localExchange.ResolveExchange();
        
    }


    void DetermineSurplusResources (Population population)
    {
        Dictionary<Resource.Type, float> resourceReservationList = new Dictionary<Resource.Type, float>();
        float foodAmountToKeep = population.cycleDesiredFoodConsumption * WorldConstants.POPULATION_FOOD_STORAGE;
        float waresAmountToKeep = population.cycleDesiredLuxuryConsumption * WorldConstants.POPULATION_LUXURY_STORAGE;
        resourceReservationList.Add(Resource.Type.Wheat, foodAmountToKeep);
        resourceReservationList.Add(Resource.Type.Wares, waresAmountToKeep);
  
        foreach (Resource.Type restype in population.resourcePortfolio.Keys)   
            if (population.resourcePortfolio[restype].amount > 0 && restype != Resource.Type.Silver)
            {
                if (resourceReservationList.ContainsKey(restype))
                {
                    if (population.resourcePortfolio[restype].amount > resourceReservationList[restype])
                        population.cycleLocalSurplusResources.Add(restype, (population.resourcePortfolio[restype].amount - resourceReservationList[restype]));
                }
                else
                    population.cycleLocalSurplusResources.Add(restype, population.resourcePortfolio[restype].amount);
            }

    }
    void DetermineWantedResources(Population population)
    {
        float actualDesiredFoodAmount;
        float actualDesiredWaresAmount;
        float totalDesiredFoodAmount = population.cycleDesiredFoodConsumption * WorldConstants.POPULATION_FOOD_STORAGE;
        float totalDesiredWaresAmount = population.cycleDesiredLuxuryConsumption * WorldConstants.POPULATION_LUXURY_STORAGE;


        if (population.resourcePortfolio[Resource.Type.Wheat].amount < totalDesiredFoodAmount)
            actualDesiredFoodAmount = totalDesiredFoodAmount - population.resourcePortfolio[Resource.Type.Wheat].amount  ;
        else
            actualDesiredFoodAmount = 0f;
        if (actualDesiredFoodAmount > 0)
            population.cycleLocalWantedResources.Add(Resource.Type.Wheat, actualDesiredFoodAmount);


        if (population.resourcePortfolio[Resource.Type.Wares].amount < totalDesiredWaresAmount)
            actualDesiredWaresAmount = totalDesiredWaresAmount - population.resourcePortfolio[Resource.Type.Wares].amount  ;
        else
            actualDesiredWaresAmount = 0f;  
        if (actualDesiredWaresAmount > 0)
            population.cycleLocalWantedResources.Add(Resource.Type.Wares, actualDesiredWaresAmount);

    }
    void RegisterToMarket(Population population)
    {
        population.GetHomeLocation().localMarket.RegisterAsPopulation(population);
    }

    void DetermineSurplusResources(Ruler ruler)
    {

    }
 
    void DetermineWantedResources(Ruler ruler)
    {

    }
    void RegisterToMarket(Ruler ruler)
    {
        ruler.GetHomeLocation().localMarket.RegisterAsRuler(ruler);

    }

    void ClearStepVariables(Population population)
    {
        population.cycleLocalSurplusResources = new Dictionary<Resource.Type, float>();
        population.cycleLocalWantedResources = new Dictionary<Resource.Type, float>();
    }
    void ClearStepVariables(Ruler ruler)
    {
        ruler.cycleLocalSurplusResources = new Dictionary<Resource.Type, float>();
    }
    void CheckForSurplusDesiredOverlap()
    {
        foreach (Population pop in EconomyController.Instance.populationDictionary.Keys)
            foreach (Resource.Type restype in pop.cycleLocalSurplusResources.Keys)
                if (pop.cycleLocalWantedResources.ContainsKey(restype))
                    Debug.Log("Overlap!  for " + pop.blockID + " restype: " + restype);
    }
}
