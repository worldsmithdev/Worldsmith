using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalExchangeStep : Step
{
     

    // Every location gets a new LocalMarket
    public override void PrepareStep()
    {         
        MarketController.Instance.cycleLocalMarkets = new List<LocalMarket>();
        ExchangeController.Instance.cycleLocalExchanges = new List<LocalExchange>();
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
        {
            loc.localMarket = null;
            loc.localMarket = new LocalMarket(loc);
        }
    }
    // Each potential Participant sets their haves and wants
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

    // Each Participant gets added to its LocalMarket
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
        // Make sure no Participant offers resources that they also want
        CheckForSurplusDesiredOverlap();

        foreach (LocalMarket locMarket in MarketController.Instance.cycleLocalMarkets)    
         locMarket.ResolveMarket();

        //foreach (LocalExchange localExchange in MarketController.Instance.cycleLocalExchanges)        
        //    localExchange.ResolveExchange();
        
    }

    // ----------------------------------------------------------------------

    // Reserve some amount of resources. Add the rest to localSurplusDict
    void DetermineSurplusResources (Population population)
    {
        Dictionary<Resource.Type, float> resourceReservationList = new Dictionary<Resource.Type, float>();
        float foodAmountToKeep = population.cycleDesiredFoodConsumption * WorldConstants.POP_FOOD_STORAGE_CYCLES;
        float waresAmountToKeep = population.cycleDesiredComfortConsumption * WorldConstants.POP_COMFORT_STORAGE_CYCLES;
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


    // Reserve some amount of resources. Add the rest to localSurplusDict
    void DetermineSurplusResources(Ruler ruler)
    {
        Dictionary<Resource.Type, float> resourceReservationList = new Dictionary<Resource.Type, float>();
        float foodAmountToKeep = 0f;
        float waresAmountToKeep = 0f;

        if (ruler.isLocalRuler == true)
        {
            foreach (Population population in ruler.GetControlledPopulations())
            {
                if (population != null)
                {
                    foodAmountToKeep += population.cycleDesiredFoodConsumption * WorldConstants.RULER_FOOD_STORAGE_CYCLES;
                    waresAmountToKeep += population.cycleDesiredComfortConsumption * WorldConstants.RULER_COMFORT_STORAGE_CYCLES;
                }
            }
            foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)            
                resourceReservationList.Add(restype, 20);            

            resourceReservationList[Resource.Type.Wheat] = foodAmountToKeep;
            resourceReservationList[Resource.Type.Wares] = waresAmountToKeep;
            resourceReservationList[Resource.Type.Timber] = (ruler.GetHomeLocation().populationSize * 50);
            resourceReservationList[Resource.Type.Stone] = (ruler.GetHomeLocation().populationSize * 50); 

            foreach (Resource.Type restype in ruler.resourcePortfolio.Keys)
                if (ruler.resourcePortfolio[restype].amount > 0 && restype != Resource.Type.Silver)
                {
                    if (resourceReservationList.ContainsKey(restype))
                    {
                        if (ruler.resourcePortfolio[restype].amount > resourceReservationList[restype])
                            ruler.cycleLocalSurplusResources.Add(restype, (ruler.resourcePortfolio[restype].amount - resourceReservationList[restype]));
                    }
                    else
                        ruler.cycleLocalSurplusResources.Add(restype, ruler.resourcePortfolio[restype].amount);
                }
        }
        else // non-local-ruler
        {

        } 
    }

    // Determine what each pop needs for both types of consumption
    void DetermineWantedResources(Population population)
    {
        float actualDesiredFoodAmount;
        float actualDesiredWaresAmount;
        float totalDesiredFoodAmount = population.cycleDesiredFoodConsumption * WorldConstants.POP_FOOD_WANTED_CYCLES;
        float totalDesiredWaresAmount = population.cycleDesiredComfortConsumption * WorldConstants.POP_COMFORT_WANTED_CYCLES;

        if (population.resourcePortfolio[Resource.Type.Wheat].amount < totalDesiredFoodAmount)
            actualDesiredFoodAmount = totalDesiredFoodAmount - population.resourcePortfolio[Resource.Type.Wheat].amount;
        else
            actualDesiredFoodAmount = 0f;

        if (actualDesiredFoodAmount > 0) 
            population.cycleLocalWantedResources.Add(Resource.Type.Wheat, actualDesiredFoodAmount); 

        if (population.resourcePortfolio[Resource.Type.Wares].amount < totalDesiredWaresAmount)
            actualDesiredWaresAmount = totalDesiredWaresAmount - population.resourcePortfolio[Resource.Type.Wares].amount;
        else
            actualDesiredWaresAmount = 0f;
        if (actualDesiredWaresAmount > 0)
            population.cycleLocalWantedResources.Add(Resource.Type.Wares, actualDesiredWaresAmount);

    }
    // Determine what to want stockpile for each Pop's consumptions
    void DetermineWantedResources(Ruler ruler)
    {
        float actualDesiredFoodAmount;
        float actualDesiredWaresAmount;
        float totalDesiredFoodAmount = 0f;
        float totalDesiredWaresAmount = 0f;

        if (ruler.isLocalRuler == true)
        {
            foreach (Population population in ruler.GetControlledPopulations())
            {
                if (population != null)
                {
                    totalDesiredFoodAmount += population.cycleDesiredFoodConsumption;
                    totalDesiredWaresAmount += population.cycleDesiredComfortConsumption;
                }
            }

            if (ruler.resourcePortfolio[Resource.Type.Wheat].amount < totalDesiredFoodAmount)
                actualDesiredFoodAmount = totalDesiredFoodAmount - ruler.resourcePortfolio[Resource.Type.Wheat].amount;
            else
                actualDesiredFoodAmount = 0f;

            if (actualDesiredFoodAmount > 0)
                ruler.cycleLocalWantedResources.Add(Resource.Type.Wheat, actualDesiredFoodAmount);

            if (ruler.resourcePortfolio[Resource.Type.Wares].amount < totalDesiredWaresAmount)
                actualDesiredWaresAmount = totalDesiredWaresAmount - ruler.resourcePortfolio[Resource.Type.Wares].amount;
            else
                actualDesiredWaresAmount = 0f;
            if (actualDesiredWaresAmount > 0)
                ruler.cycleLocalWantedResources.Add(Resource.Type.Wares, actualDesiredWaresAmount);
        }
        else // non-local-ruler
        {

        }


    }
    void RegisterToMarket(Population population)
    {
        population.GetHomeLocation().localMarket.RegisterAsPopulation(population);
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
        ruler.cycleLocalWantedResources = new Dictionary<Resource.Type, float>();
    }
    void CheckForSurplusDesiredOverlap()
    {
        foreach (Population pop in EconomyController.Instance.populationDictionary.Keys)
            foreach (Resource.Type restype in pop.cycleLocalSurplusResources.Keys)
                if (pop.cycleLocalWantedResources.ContainsKey(restype))
                    Debug.Log("Overlap in Surplus and Wanted!  for " + pop.blockID + " restype: " + restype);
    }
}
