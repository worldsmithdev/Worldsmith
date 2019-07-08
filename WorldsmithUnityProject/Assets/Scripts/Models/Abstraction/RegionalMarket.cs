using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalMarket : Market
{ 

    public List<RegionalExchange> regionalExchangesList = new List<RegionalExchange>();


    public Ruler seller;
    public List<Ruler> buyersList = new List<Ruler>();


    public RegionalMarket(Location loc)
    {
        marketName = "RegionalMarket" + WorldController.Instance.GetWorld().completedCycles;
        MarketController.Instance.cycleRegionalMarkets.Add(this);
        MarketController.Instance.archivedRegionalMarkets[loc].Add(this);
    }
     
     

    public void InitializeMarket(Ruler ruler)
    {
        // Determine type 



        // Determine Parties
         
 
    }

    public override void ResolveMarket()
    { 
        // Determine and pledge resources

        // Creat exchanges
    }

}
