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
            }  
    }

    public override void CycleStep(EcoBlock ecoblock)
    { 
            Ruler ruler = (Ruler)ecoblock;
        if (ruler.isLocalRuler)
        {
            ruler.GetHomeLocation().regionalMarket.CreateMarket(ruler);
        } 
    }
    public override void ResolveStep()
    {  
        foreach (RegionalMarket market in MarketController.Instance.cycleRegionalMarkets)
            market.ResolveMarket();  
    }



    // ----------------------------------------------------------------------
 


    // Keep some necessities for populations. Keep some reserved resources.
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
                            ruler.cycleRegionalSurplusResources[restype]+= (ruler.resourcePortfolio[restype].amount - resourceReservationList[restype]);
                    }
                    else
                        ruler.cycleRegionalSurplusResources[restype] += ruler.resourcePortfolio[restype].amount;
                }
        } 
    }
    void DetermineWantedResources(Ruler ruler)
    {
        ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Silver);

        if (ruler.GetHomeLocation().hubType == 1)
        {
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wares);
        }
        if (ruler.GetHomeLocation().hubType == 2)
        {
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wares);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wheat);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Timber);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Stone);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Salt);
        }
        if (ruler.GetHomeLocation().hubType == 3)
        {
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wheat);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Timber);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Stone);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Salt); 
        }
        if (ruler.GetHomeLocation().hubType == 4)
        {
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wheat);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Timber);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Stone);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Salt); 
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wares);
        }
        if (ruler.GetHomeLocation().hubType == 5)
        {
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Wheat);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Timber);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Stone);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Salt);
            ruler.cycleRegionalWantedResourceTypes.Add(Resource.Type.Marble);
        }
        foreach (Resource.Type restype in ruler.wantedResourceTypes)        
            if (ruler.cycleRegionalWantedResourceTypes.Contains(restype) == false)
                 ruler.cycleRegionalWantedResourceTypes.Add(restype);
        
    }


    void ClearStepVariables(Ruler ruler)
    {
        ruler.cycleRegionalSurplusResources = new Dictionary<Resource.Type, float>();
        ruler.cycleRegionalCommittedResources = new Dictionary<Resource.Type, float>();
        ruler.cycleRegionalWantedResourceTypes = new List<Resource.Type >();
        ResourceController.Instance.InitiateResourceDictionary(ruler.cycleRegionalSurplusResources); 
        ResourceController.Instance.InitiateResourceDictionary(ruler.cycleRegionalCommittedResources); 
    }
}
