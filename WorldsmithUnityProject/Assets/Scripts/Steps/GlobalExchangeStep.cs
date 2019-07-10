using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalExchangeStep : Step
{
    public override void PrepareStep()
    {
        MarketController.Instance.cycleGlobalMarkets = new List<GlobalMarket>();
        ExchangeController.Instance.cycleGlobalExchanges = new List<GlobalExchange>();
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
        {
            if (loc.offloadOne == "Global")
            {
                loc.globalMarket = null;
                loc.globalMarket = new GlobalMarket(loc);
            }
          
        }
    }

    public override void ConfigureStep(EcoBlock ecoblock)
    { 
          if (ecoblock.blockType == EcoBlock.BlockType.Ruler)
        {
            Ruler ruler = (Ruler)ecoblock;

            if (ruler.GetHomeLocation().offloadOne == "Global")
            {
                if (ruler.isLocalRuler)
                {
                    ClearStepVariables(ruler);
                    DetermineSurplusResources(ruler); 
                }
            }
          
        } 
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Ruler ruler = (Ruler)ecoblock;
        if (ruler.GetHomeLocation().offloadOne == "Global")
        {
            ruler.GetHomeLocation().globalMarket.CreateMarket(ruler);
        }
           
    }
    public override void ResolveStep()
    {
        foreach (GlobalMarket market in MarketController.Instance.cycleGlobalMarkets)
        { 
            market.ResolveMarket();
        }
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
                    // Check that the goods are marked as exportable
                    if (ExchangeController.Instance.globalAcceptedTypes.Contains(restype))
                    {
                        if (resourceReservationList.ContainsKey(restype))
                        {
                            if (ruler.resourcePortfolio[restype].amount > resourceReservationList[restype])
                                ruler.cycleGlobalSurplusResources.Add(restype, (ruler.resourcePortfolio[restype].amount - resourceReservationList[restype]));
                        }
                        else
                            ruler.cycleGlobalSurplusResources.Add(restype, ruler.resourcePortfolio[restype].amount);
                    }
         
                }
        }
    }
 


    void ClearStepVariables(Ruler ruler)
    {
        ruler.cycleGlobalSurplusResources = new Dictionary<Resource.Type, float>(); 
         
    }
}
