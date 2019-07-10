using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMarket : Market
{
    public List<GlobalExchange> globalExchangesList = new List<GlobalExchange>();

    public Ruler seller;

    public GlobalMarket(Location loc)
    {
        marketName = "GlobalMarket" + loc.elementID + WorldController.Instance.GetWorld().completedCycles;
        MarketController.Instance.cycleGlobalMarkets.Add(this);
        MarketController.Instance.archivedGlobalMarkets[loc].Add(this);
    }

    public void CreateMarket(Ruler ruler)
    {
        seller = ruler;
        ExchangeController.Instance.exchangeCreator.CreateGlobalExchange(this, ruler); 
    }

    public override void ResolveMarket()
    {
        foreach (GlobalExchange exchange in globalExchangesList)
            exchange.ResolveExchange();
    }
}
