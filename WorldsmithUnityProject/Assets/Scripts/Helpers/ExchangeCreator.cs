using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeCreator : MonoBehaviour
{



    // ------------------------ LOCAL

    void AttemptResourcePurchase(LocalExchange createdExchange, Resource shoppedResource, Resource.Type payType)
    {
        Participant activeParticipant = createdExchange.activeParticipant;
        Participant passiveParticipant = createdExchange.passiveParticipant; 
        bool forceSilver = ExchangeController.Instance.forceSilverOnLocalExchange;
        float silverPercentage = DetermineForcedSilverPercentage(activeParticipant, passiveParticipant);

        // Subtract the portion of this resource sthat is already paid for
        Resource wantedResource = new Resource(shoppedResource.type, shoppedResource.amount);
        wantedResource.amount -= createdExchange.passiveCommitted[shoppedResource.type];

        float exchangeRate = ExchangeController.Instance.GetLocalExchangeRate(createdExchange);

        float soldResourceValue = Converter.GetSilverEquivalent(wantedResource) * exchangeRate; 
        float paymentResourceValue = Converter.GetSilverEquivalent(new Resource(payType, activeParticipant.offeredResources[payType]));
        paymentResourceValue -= Converter.GetSilverEquivalent(new Resource(payType, createdExchange.activeCommitted[payType]));
        float paymentResourceWantValue = Converter.GetSilverEquivalent(new Resource(payType, passiveParticipant.wantedResources[payType])); 
        if (paymentResourceValue > paymentResourceWantValue)
            paymentResourceValue = paymentResourceWantValue;
        

        if (paymentResourceValue > soldResourceValue)
        {
            if (forceSilver == true)
            {
                createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, soldResourceValue * silverPercentage));
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, soldResourceValue * (1 - silverPercentage)));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, soldResourceValue / exchangeRate));
            }
            else
            {
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, soldResourceValue));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, soldResourceValue / exchangeRate));
            } 
        }
        else if (paymentResourceValue > 0)
        {
            if (forceSilver == true)
            {
                createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, paymentResourceValue * silverPercentage));
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, paymentResourceValue * (1 - silverPercentage)));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, paymentResourceValue / exchangeRate));
            }
            else
            {
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, paymentResourceValue));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, paymentResourceValue  / exchangeRate));
            }   
        } 
    } 

    void AttemptSilverPurchase(LocalExchange createdExchange, Resource shoppedResource )
    {
        Participant activeParticipant = createdExchange.activeParticipant;
        Participant passiveParticipant = createdExchange.passiveParticipant; 
        
        Resource wantedResource = new Resource(shoppedResource.type, shoppedResource.amount);
        wantedResource.amount -= createdExchange.passiveCommitted[shoppedResource.type];

        float exchangeRate = ExchangeController.Instance.GetLocalExchangeRate(createdExchange);

        float soldResourceValue = Converter.GetSilverEquivalent(wantedResource) * exchangeRate; 
        float reservedSilver = Determiner.DetermineSilverReservation(activeParticipant.linkedEcoBlock);
        float spendValue = activeParticipant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount - reservedSilver;
        spendValue -= createdExchange.activeCommitted[Resource.Type.Silver];

        if (spendValue > soldResourceValue)
        {
            createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, soldResourceValue));
            createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, soldResourceValue / exchangeRate));
       //     Debug.Log(" Buying ALL with silver!  for " + activeParticipant.participantName + "  shopping at " + passiveParticipant.participantName + "  resource " + wantedResource.type + wantedResource.amount + "  Actually buying: " + Converter.GetResourceEquivalent(wantedResource.type, sellValue).type + Converter.GetResourceEquivalent(wantedResource.type, sellValue).amount + "  for silver:" + sellValue);

        }
        else if (soldResourceValue > 0)
        {
            createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, spendValue));
            createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, spendValue / exchangeRate));
       //     Debug.Log(" Buying AFFORDABLE with silver!  for " + activeParticipant.participantName + "  shopping at " + passiveParticipant.participantName + "  resource " + wantedResource.type + wantedResource.amount + "  Actually buying: " + Converter.GetResourceEquivalent(wantedResource.type, spendValue).type + Converter.GetResourceEquivalent(wantedResource.type, spendValue).amount + "  for silver:" + spendValue);

        }

    } 
     
    public LocalExchange CreateLocalExchange(LocalExchange.Type type, LocalMarket market, Participant activeParticipant, Participant passiveParticipant, List<Resource> shoppingList)
    {
        LocalExchange createdExchange = new LocalExchange();

        createdExchange.exchangeName = "LocExch" +type + activeParticipant.participantName + WorldController.Instance.GetWorld().completedCycles;
        createdExchange.type = type;
        createdExchange.activeParticipant = activeParticipant;
        createdExchange.passiveParticipant = passiveParticipant;
        createdExchange.localMarket = market;
        createdExchange.shoppingList = shoppingList;


        OrderShoppingList(createdExchange, shoppingList);

   
        // Attempt Purchases for each item in ordered shopping list
        foreach (Resource shopRes in createdExchange.orderedShoppingList)
        {
            // Attempt Purchase through prioritized payment-types
            for (int secondcount = 1; activeParticipant.resourceSellPriority.ContainsValue(secondcount); secondcount++)
            {
                // set Pay type
                Resource.Type payType = Resource.Type.Unassigned;
                foreach (Resource.Type restype in activeParticipant.resourceSellPriority.Keys)
                    if (activeParticipant.resourceSellPriority[restype] == secondcount)
                        payType = restype;


                if (activeParticipant.offeredResources.ContainsKey(payType) && passiveParticipant.wantedResources.ContainsKey(payType))                
                    AttemptResourcePurchase(createdExchange, shopRes, payType);
                
            }
            // Attempt Purchase through non-prioritized resources
            if (shopRes.amount > createdExchange.passiveCommitted[shopRes.type])
            {
                Resource remainingShopRes = new Resource(shopRes.type, shopRes.amount - createdExchange.passiveCommitted[shopRes.type]);
                foreach (Resource.Type restype in activeParticipant.resourceSellPriority.Keys)
                    if (activeParticipant.resourceSellPriority[restype] == 0) // remaning types               
                        if (activeParticipant.offeredResources.ContainsKey(restype) && passiveParticipant.wantedResources.ContainsKey(restype))
                            AttemptResourcePurchase(createdExchange, remainingShopRes, restype);
            }
         

            // Pay with silver if there's more to be had
            if (shopRes.amount > createdExchange.passiveCommitted[shopRes.type])
            {
                Resource remainingShopRes = new Resource(shopRes.type, shopRes.amount - createdExchange.passiveCommitted[shopRes.type]);
                AttemptSilverPurchase(createdExchange, remainingShopRes);
            }

        } 


        float totalValue = 0f;
        foreach (Resource res in createdExchange.activeResources)        
            if (res != null)
                totalValue += Converter.GetSilverEquivalent(res);
        foreach (Resource res in createdExchange.passiveResources)
            if (res != null)
                totalValue += Converter.GetSilverEquivalent(res);

        if (totalValue > 0)
        {
            market.localExchangesList.Add(createdExchange);
            createdExchange.ResolveExchange();
        }

        return createdExchange;
    }


    float DetermineForcedSilverPercentage(Participant activeParticipant, Participant passiveParticipant)
    {
        float amount = 0.1f; 
        return amount;
    }

    void OrderShoppingList(LocalExchange createdExchange, List<Resource> shoppingList)
    {
        Participant activeParticipant = createdExchange.activeParticipant;
        Participant passiveParticipant = createdExchange.passiveParticipant;

        for (int firstcount = 1; activeParticipant.resourceBuyPriority.ContainsValue(firstcount); firstcount++)
        {
            // Check what type shopping for
            Resource.Type shopType = Resource.Type.Unassigned;
            foreach (Resource.Type restype in activeParticipant.resourceBuyPriority.Keys)
                if (activeParticipant.resourceBuyPriority[restype] == firstcount)
                    shopType = restype;
            // verify that this type is in shopping list 
            foreach (Resource res in activeParticipant.shoppingList)
                if (res.type == shopType)
                    createdExchange.orderedShoppingList.Add(res);
        }
        // shop for all remaning types in shoplist
        foreach (Resource.Type buytype in activeParticipant.resourceBuyPriority.Keys)
            if (activeParticipant.resourceBuyPriority[buytype] == 0) // remaning types            
                foreach (Resource res in activeParticipant.shoppingList)
                    if (res.type == buytype)
                        createdExchange.orderedShoppingList.Add(res);
    }


    // ------------------------ REGIONAL 
    
    public void CreateRegionalSeizedExchange(RegionalMarket market, EcoBlock activeParty, Ruler dominator)
    {
        RegionalExchange createdExchange = new RegionalExchange();
        createdExchange.type = RegionalExchange.Type.Seized;
        createdExchange.exchangeName = "" + createdExchange.type + "Exch" +  WorldController.Instance.GetWorld().completedCycles + "-" + market.regionalExchangesList.Count;
        createdExchange.activeParty = activeParty;
        createdExchange.passiveParty = dominator;
        createdExchange.regionalMarket = market;

        foreach (Resource.Type restype in dominator.cycleRegionalWantedResourceTypes)
            if (activeParty.resourcePortfolio[restype].amount > 0)
                createdExchange.CommitResource(true, new Resource(restype, activeParty.resourcePortfolio[restype].amount) ); 
        
        if (createdExchange.activeResources.Count > 0 || createdExchange.passiveResources.Count > 0)
               market.regionalExchangesList.Add(createdExchange);  
    }
    public void CreateRegionalForcedExchange(RegionalMarket market, Ruler activeParty, Ruler passiveParty)
    { 
        RegionalExchange createdExchange = new RegionalExchange();
        createdExchange.type = RegionalExchange.Type.Forced;
        createdExchange.exchangeName = "" + createdExchange.type + "Exch" + activeParty.GetHomeLocation().elementID + WorldController.Instance.GetWorld().completedCycles + "-" + market.regionalExchangesList.Count;
        createdExchange.activeParty = activeParty;
        createdExchange.passiveParty = passiveParty;
        createdExchange.regionalMarket = market;

        foreach (Resource.Type restype in passiveParty.cycleRegionalWantedResourceTypes)        
            if (activeParty.cycleRegionalSurplusResources.ContainsKey(restype))           
                AttemptTrade(createdExchange, restype, activeParty, passiveParty);

        if (createdExchange.activeResources.Count > 0 || createdExchange.passiveResources.Count > 0)
        market.regionalExchangesList.Add(createdExchange);
    }

    public void CreateRegionalFreeExchange(RegionalMarket market, Ruler activeParty, Ruler passiveParty)
    {
        
            RegionalExchange createdExchange = new RegionalExchange();
            createdExchange.type = RegionalExchange.Type.Free;
            createdExchange.exchangeName = "" + createdExchange.type + "Exch" + activeParty.GetHomeLocation().elementID + WorldController.Instance.GetWorld().completedCycles + "-" + market.regionalExchangesList.Count;
            createdExchange.activeParty = activeParty;
            createdExchange.passiveParty = passiveParty;
            createdExchange.regionalMarket = market;

        foreach (Resource.Type restype in passiveParty.cycleRegionalWantedResourceTypes)
                if (activeParty.cycleRegionalSurplusResources.ContainsKey(restype))
                    AttemptTrade(createdExchange, restype, activeParty, passiveParty);

            if (createdExchange.activeResources.Count > 0 || createdExchange.passiveResources.Count > 0)
                market.regionalExchangesList.Add(createdExchange); 
    } 
    void AttemptTrade(RegionalExchange createdExchange, Resource.Type restype, Ruler activeParty, Ruler passiveParty)
    {
        float offeredValue = Converter.GetSilverEquivalent(new Resource(restype, activeParty.cycleRegionalSurplusResources[restype] - activeParty.cycleRegionalCommittedResources[restype]));

        float exchangeRate = ExchangeController.Instance.GetExchangeRate(activeParty);

        float paidValue = 0f;

        if (offeredValue > 0)
        {
            List<Resource> paymentResources = DeterminePaymentResources(offeredValue * exchangeRate, activeParty, passiveParty);
            foreach (Resource res in paymentResources)
            {
                createdExchange.CommitResource(false, res);
                paidValue += Converter.GetSilverEquivalent(res) / exchangeRate;
            }
            createdExchange.CommitResource(true, Converter.GetResourceEquivalent(restype, paidValue));
        }
    }
    List<Resource> DeterminePaymentResources(float value, Ruler activeParty, Ruler passiveParty)
    {
        List<Resource> returnList = new List<Resource>();

        float remainingValue = value;
        float silverToPay;

        if (ExchangeController.Instance.forceSilverOnRegionalExchange == true && passiveParty.resourcePortfolio[Resource.Type.Silver].amount > 0)
        {
            silverToPay = remainingValue * ExchangeController.Instance.regionalForcedSilverPercentage;
            if (passiveParty.resourcePortfolio[Resource.Type.Silver].amount >= silverToPay)
            {
                returnList.Add(new Resource(Resource.Type.Silver, silverToPay));
                remainingValue -= silverToPay;
            }
            else if (passiveParty.resourcePortfolio[Resource.Type.Silver].amount < silverToPay)
            {
                returnList.Add(new Resource(Resource.Type.Silver, passiveParty.resourcePortfolio[Resource.Type.Silver].amount));
                remainingValue -= passiveParty.resourcePortfolio[Resource.Type.Silver].amount;
            }
        }

        foreach (Resource.Type restype in activeParty.cycleRegionalWantedResourceTypes)
        {
            if (passiveParty.resourcePortfolio[restype].amount > 0)
            {
                bool trading = true;
                
                    if (ExchangeController.Instance.globalImportPercentages.ContainsKey(restype))
                        trading = true;
                    else
                        trading = false; 
                
                 
                if (trading == true)
                {
                    // has more of resource to pay with than needed
                    if (Converter.GetSilverEquivalent(new Resource(restype, passiveParty.resourcePortfolio[restype].amount)) >= remainingValue)
                    {
                        returnList.Add(new Resource(restype, Converter.GetResourceEquivalent(restype, remainingValue).amount));
                        remainingValue = 0;
                    }
                    // has less of resource to pay with than needed
                    else if (Converter.GetSilverEquivalent(new Resource(restype, passiveParty.resourcePortfolio[restype].amount)) < remainingValue)
                    {
                        returnList.Add(new Resource(restype, Converter.GetResourceEquivalent(restype, passiveParty.resourcePortfolio[restype].amount).amount));
                        remainingValue -= Converter.GetResourceEquivalent(restype, passiveParty.resourcePortfolio[restype].amount).amount;
                    }
                } 
                
            }
        }

        if (remainingValue > 0 && passiveParty.resourcePortfolio[Resource.Type.Silver].amount > 0)
        {
            if (passiveParty.resourcePortfolio[Resource.Type.Silver].amount >= remainingValue)
            {
                returnList.Add(new Resource(Resource.Type.Silver, remainingValue));
            }
            else if (passiveParty.resourcePortfolio[Resource.Type.Silver].amount < remainingValue)
            {
                returnList.Add(new Resource(Resource.Type.Silver, passiveParty.resourcePortfolio[Resource.Type.Silver].amount));
            }
        }

        return returnList;
    }


    // ------------------------ GLOBAL 

    public void CreateGlobalExchange (GlobalMarket market, Ruler ruler)
    {
        GlobalExchange createdExchange = new GlobalExchange();
        createdExchange.exchangeName = "GlobalExch" + ruler.GetHomeLocation().elementID + WorldController.Instance.GetWorld().completedCycles;
        createdExchange.activeParty = ruler;
        createdExchange.globalMarket = market;

        float totalExportsValue = 0f;

        // Ship off resources and tally total value
        foreach (Resource.Type restype in ruler.cycleGlobalSurplusResources.Keys)
        { 
            createdExchange.CommitResource(new Resource(restype, ruler.cycleGlobalSurplusResources[restype]));
            totalExportsValue += Converter.GetSilverEquivalent(new Resource(restype, ruler.cycleGlobalSurplusResources[restype]));
        }

        // Return resources for total value according to preset proportions
        foreach (Resource.Type restype in ExchangeController.Instance.globalImportPercentages.Keys)
        {
            float percentage = ExchangeController.Instance.globalImportPercentages[restype];
            createdExchange.receivedResources.Add(new Resource (restype, Converter.GetResourceEquivalent(restype, totalExportsValue).amount * percentage     )    );
             
        }

        if (totalExportsValue > 0  )
            market.globalExchangesList.Add(createdExchange);
    }


}
