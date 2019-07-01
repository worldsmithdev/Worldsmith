﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeCreator : MonoBehaviour
{

    void AttemptResourcePurchase(LocalExchange createdExchange, Resource shoppedResource, Resource.Type payType)
    {
        Participant activeParticipant = createdExchange.activeParticipant;
        Participant passiveParticipant = createdExchange.passiveParticipant;

        
        bool forceSilver = ExchangeController.Instance.forceSilverOnLocalExchange;
        float silverPercentage = DetermineForcedSilverPercentage(activeParticipant, passiveParticipant);

        // Subtract the portion of this resource sthat is already paid for
        Resource wantedResource = new Resource(shoppedResource.type, shoppedResource.amount);
        wantedResource.amount -= createdExchange.passiveCommitted[shoppedResource.type];

        float sellValue = Converter.GetSilverEquivalent(wantedResource); 
        float spendValue = Converter.GetSilverEquivalent(new Resource(payType, activeParticipant.offeredResources[payType]));
        spendValue -= Converter.GetSilverEquivalent(new Resource(payType, createdExchange.activeCommitted[payType]));
        float wantValue = Converter.GetSilverEquivalent(new Resource(payType, passiveParticipant.wantedResources[payType])); 
        if (spendValue > wantValue)
            spendValue = wantValue;

        if (spendValue > sellValue)
        {
            if (forceSilver == true)
            {
                createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, sellValue * silverPercentage));
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, sellValue * (1 - silverPercentage)));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, sellValue));
            }
            else
            {
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, sellValue));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, sellValue));
            } 
        }
        else if (spendValue > 0)
        {
            if (forceSilver == true)
            {
                createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, spendValue * silverPercentage));
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, spendValue * (1 - silverPercentage)));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, spendValue));
            }
            else
            {
                createdExchange.CommitResource(true, Converter.GetResourceEquivalent(payType, spendValue));
                createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, spendValue));
            }   
        } 
    }


    void AttemptSilverPurchase(LocalExchange createdExchange, Resource shoppedResource )
    {
        Participant activeParticipant = createdExchange.activeParticipant;
        Participant passiveParticipant = createdExchange.passiveParticipant; 
        
        Resource wantedResource = new Resource(shoppedResource.type, shoppedResource.amount);
        wantedResource.amount -= createdExchange.passiveCommitted[shoppedResource.type];

        float sellValue = Converter.GetSilverEquivalent(new Resource(wantedResource.type, wantedResource.amount));
        float reservedSilver = Determiner.DetermineSilverReservation(activeParticipant.linkedEcoBlock);
        float spendValue = activeParticipant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount - reservedSilver;


        if (spendValue > sellValue)
        {
            createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, sellValue));
            createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, sellValue));
       //     Debug.Log(" Buying ALL with silver!  for " + activeParticipant.participantName + "  shopping at " + passiveParticipant.participantName + "  resource " + wantedResource.type + wantedResource.amount + "  Actually buying: " + Converter.GetResourceEquivalent(wantedResource.type, sellValue).type + Converter.GetResourceEquivalent(wantedResource.type, sellValue).amount + "  for silver:" + sellValue);

        }
        else if (spendValue > 0)
        {
            createdExchange.CommitResource(true, new Resource(Resource.Type.Silver, spendValue));
            createdExchange.CommitResource(false, Converter.GetResourceEquivalent(wantedResource.type, spendValue));
       //     Debug.Log(" Buying AFFORDABLE with silver!  for " + activeParticipant.participantName + "  shopping at " + passiveParticipant.participantName + "  resource " + wantedResource.type + wantedResource.amount + "  Actually buying: " + Converter.GetResourceEquivalent(wantedResource.type, spendValue).type + Converter.GetResourceEquivalent(wantedResource.type, spendValue).amount + "  for silver:" + spendValue);

        }

    }
    //public LocalExchange CreateLocalPurchase(LocalMarket market, Participant activeParticipant, Participant passiveParticipant, List<Resource> shoppingList)
    //{
    //    LocalExchange createdExchange = new LocalExchange();

    //    createdExchange.exchangeName = "LocPurch" + activeParticipant.participantName + WorldController.Instance.GetWorld().completedCycles;
    //    createdExchange.activeParticipant = activeParticipant;
    //    createdExchange.passiveParticipant = passiveParticipant;
    //    createdExchange.localMarket = market;
    //    createdExchange.shoppingList = shoppingList;


    //    return createdExchange;
    //}



    public LocalExchange CreateLocalExchange(LocalExchange.ExchangeType type, LocalMarket market, Participant activeParticipant, Participant passiveParticipant, List<Resource> shoppingList)
    {
        LocalExchange createdExchange = new LocalExchange();

        createdExchange.exchangeName = "LocExch" +type + activeParticipant.participantName + WorldController.Instance.GetWorld().completedCycles;
        createdExchange.exchangeType = type;
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
                foreach (Resource.Type restype in activeParticipant.resourceSellPriority.Keys)
                    if (activeParticipant.resourceSellPriority[restype] == 0) // remaning types               
                        if (activeParticipant.offeredResources.ContainsKey(restype) && passiveParticipant.wantedResources.ContainsKey(restype))
                            AttemptResourcePurchase(createdExchange, shopRes, restype);
            }
         

            // Determine what to pay silver for
            if (shopRes.amount > createdExchange.passiveCommitted[shopRes.type])
            {
                AttemptSilverPurchase(createdExchange, shopRes);
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
            createdExchange.ResolveExchange();  

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
}
