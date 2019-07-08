using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalExchangeStep : Step
{ 
    public override void PrepareStep()
    {
        MarketController.Instance.cycleRegionalMarkets = new List<RegionalMarket>();
        ExchangeController.Instance.cycleRegionalExchanges = new List<RegionalExchange>();
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
        {
            loc.regionalMarket = null;
            loc.regionalMarket = new RegionalMarket(loc);
        }
    }

    public override void ConfigureStep(EcoBlock ecoblock)
    {  
            Ruler ruler = (Ruler)ecoblock; 
            if (ruler.isLocalRuler)
            {
                ClearStepVariables(ruler);
                DetermineSurplusResources(ruler);
                DetermineWantedResources(ruler);
                DetermineProfitResources(ruler);
            }  
    }

    public override void CycleStep(EcoBlock ecoblock)
    { 
            Ruler ruler = (Ruler)ecoblock;
            ruler.GetHomeLocation().regionalMarket.InitializeMarket(ruler); 
            ruler.GetHomeLocation().regionalMarket.ResolveMarket(); 
    }


  

    // ----------------------------------------------------------------------

   

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
                            ruler.cycleRegionalSurplusResources.Add(restype, (ruler.resourcePortfolio[restype].amount - resourceReservationList[restype]));
                    }
                    else
                        ruler.cycleRegionalSurplusResources.Add(restype, ruler.resourcePortfolio[restype].amount);
                }
        } 
    }
    void DetermineWantedResources(Ruler ruler)
    {
        float actualDesiredFoodAmount;
        float actualDesiredWaresAmount;
        float totalDesiredFoodAmount = 0f;
        float totalDesiredWaresAmount = 0f;

        
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
                ruler.cycleRegionalWantedResources.Add(Resource.Type.Wheat, actualDesiredFoodAmount);

            if (ruler.resourcePortfolio[Resource.Type.Wares].amount < totalDesiredWaresAmount)
                actualDesiredWaresAmount = totalDesiredWaresAmount - ruler.resourcePortfolio[Resource.Type.Wares].amount;
            else
                actualDesiredWaresAmount = 0f;

            if (actualDesiredWaresAmount > 0)
                ruler.cycleRegionalWantedResources.Add(Resource.Type.Wares, actualDesiredWaresAmount); 
    }
    void DetermineProfitResources(Ruler ruler)
    {

    }



    void ClearStepVariables(Ruler ruler)
    {
        ruler.cycleRegionalSurplusResources = new Dictionary<Resource.Type, float>();
        ruler.cycleRegionalWantedResources = new Dictionary<Resource.Type, float>();
    }
}
