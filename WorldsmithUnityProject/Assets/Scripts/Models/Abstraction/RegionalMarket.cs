using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalMarket : Market
{ 

    public enum Type {  Unassigned, Free, Forced, Seized}
    public List<RegionalExchange> regionalExchangesList = new List<RegionalExchange>();


    public Type type;
    public Ruler seller;
    public List<Ruler> buyersList = new List<Ruler>();


    public RegionalMarket(Location loc)
    {
        marketName = "RegionalMarket" + loc.elementID + WorldController.Instance.GetWorld().completedCycles;
        MarketController.Instance.cycleRegionalMarkets.Add(this);
        MarketController.Instance.archivedRegionalMarkets[loc].Add(this);
    }
     
     

    public void CreateMarket(Ruler ruler)
    {
        seller = ruler;
        // Determine type 
        if (ruler.rulerHierarchy == Ruler.Hierarchy.Subjugated)
            type = Type.Seized;
        else if (ruler.rulerHierarchy == Ruler.Hierarchy.Dominated)
            type = Type.Forced;
        else
            type = Type.Free;

        // Determine Parties
        if (type != Type.Free)
            buyersList.Add(ruler.GetController());
        else if (type == Type.Free)
        {
            Location loc = ruler.GetHomeLocation();
            if (loc.offloadOne != "Global" && loc.offloadOne != "")
                if (LocationController.Instance.GetSpecificLocation(loc.offloadOne) != null)
                    buyersList.Add(LocationController.Instance.GetSpecificLocation(loc.offloadOne).localRuler);
            if ( loc.offloadTwo != "")
                if (LocationController.Instance.GetSpecificLocation(loc.offloadTwo) != null)
                    buyersList.Add(LocationController.Instance.GetSpecificLocation(loc.offloadTwo).localRuler);
            if (loc.offloadThree != "")
                if (LocationController.Instance.GetSpecificLocation(loc.offloadThree) != null)
                    buyersList.Add(LocationController.Instance.GetSpecificLocation(loc.offloadThree).localRuler);

            if (loc.offloadOne != "Global" && buyersList.Count < 1)
                Debug.Log(ruler.blockID + " has no buyers for its free trade!");
        }

        if (buyersList.Count > 1)
             OrderBuyersList();

        if (type == Type.Seized)
        {
            ExchangeController.Instance.exchangeCreator.CreateRegionalSeizedExchange(this, ruler, buyersList[0]);
            foreach (Ruler secruler in ruler.GetHomeLocation().secondaryRulers)
                ExchangeController.Instance.exchangeCreator.CreateRegionalSeizedExchange(this, secruler, buyersList[0]);
            foreach (Population  population in ruler.GetControlledPopulations()) 
                ExchangeController.Instance.exchangeCreator.CreateRegionalSeizedExchange(this, population, buyersList[0]); 
        }
        else if (type == Type.Forced)
            ExchangeController.Instance.exchangeCreator.CreateRegionalForcedExchange(this, ruler, buyersList[0]);
        else if (type == Type.Free)
            foreach (Ruler buyer in buyersList)   
                if (ruler.GetHomeLocation().offloadOne != "Global") // exclude other regional exchanges for global for now
                     ExchangeController.Instance.exchangeCreator.CreateRegionalFreeExchange(this, ruler, buyer); 
    }

    public override void ResolveMarket()
    {
        foreach (RegionalExchange exchange in regionalExchangesList)        
            exchange.ResolveExchange();
    }


    void OrderBuyersList()
    { 
        // Go with preset-given order for now
    }
}
