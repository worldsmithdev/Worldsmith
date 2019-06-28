using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMarket : Market
{


    public List<LocalExchange> localExchangesList = new List<LocalExchange>();




    List<Participant> participantsHigh = new List<Participant>();
    List<Participant> participantsMid = new List<Participant>();
    List<Participant> participantsLow = new List<Participant>();


    Location debugLoc;
    Participant debugParticipant;

    public LocalMarket(Location loc)
    {
        marketName = "LocalMarket" + WorldController.Instance.GetWorld().completedCycles;
        MarketController.Instance.cycleLocalMarkets.Add(this);
        MarketController.Instance.archivedLocalMarkets[loc].Add(this);
    }


    public void RegisterAsPopulation (Population population)
    {
        Participant newParticipant = new Participant();
        
        newParticipant.linkedEcoBlock = population;
        newParticipant.type = Participant.Type.Population;
        if (population.classType == Population.ClassType.Citizen || population.laborType == Population.LaborType.Leisure)
            newParticipant.priorityLevel = Participant.PriorityLevel.HI;
        else if (population.laborType == Population.LaborType.Artisan)
            newParticipant.priorityLevel = Participant.PriorityLevel.MID;
        else
            newParticipant.priorityLevel = Participant.PriorityLevel.LO;

        newParticipant.resourceBuyPriority = population.resourceBuyPriority;
        newParticipant.resourceSellPriority = population.resourceSellPriority;

        //newParticipant.claimedResources = new Dictionary<Resource.Type, float>();
        //newParticipant.deductedResources = new Dictionary<Resource.Type, float>();
        //newParticipant.outTradedResources = new Dictionary<Resource.Type, float>();
        //newParticipant.inTradedResources = new Dictionary<Resource.Type, float>();
        //newParticipant.purchasedResources = new List<Resource>();
        //newParticipant.silverCreditStatus = 0f;
        //newParticipant.paidSilver = 0f;
        //newParticipant.receivedSilver = 0f;

        foreach (Resource.Type restype in population.cycleLocalSurplusResources.Keys)
        {
            newParticipant.initialOfferedResources.Add(restype, population.cycleLocalSurplusResources[restype]);
            newParticipant.offeredResources.Add(restype, population.cycleLocalSurplusResources[restype]);
        }
        foreach (Resource.Type restype in population.cycleLocalWantedResources.Keys)
        {
            newParticipant.initialWantedResources.Add(restype, population.cycleLocalWantedResources[restype]);
            newParticipant.wantedResources.Add(restype, population.cycleLocalWantedResources[restype]);
        }

        if (population.GetHomeLocation().elementID == "Egenimos")
        {
            debugLoc = population.GetHomeLocation(); 
        }
        newParticipant.participantName = "P" + newParticipant.linkedEcoBlock.blockID + WorldController.Instance.GetWorld().completedCycles; 
        participantList.Add(newParticipant);

    }
    public void RegisterAsRuler (Ruler ruler)
    {
        //Participant newParticipant = new Participant();

        //newParticipant.linkedEcoBlock = population;
        //newParticipant.type = Participant.Type.Population;
        //if (population.classType == Population.ClassType.Citizen)
        //    newParticipant.priorityLevel = Participant.PriorityLevel.HI;
        //else
        //    newParticipant.priorityLevel = Participant.PriorityLevel.LO;
        //newParticipant.offeredResources = new Dictionary<Resource.Type, float>();
        //newParticipant.wantedResources = new Dictionary<Resource.Type, float>();
        //newParticipant.purchasedResources = new List<Resource>();
        //newParticipant.soldValue = 0f;

        //foreach (Resource.Type restype in population.cycleLocalSurplusResources.Keys)
        //    newParticipant.offeredResources.Add(restype, population.cycleLocalSurplusResources[restype]);
        //foreach (Resource.Type restype in population.cycleLocalWantedResources.Keys)
        //    newParticipant.wantedResources.Add(restype, population.cycleLocalWantedResources[restype]);

        //if (population.GetHomeLocation().elementID == "Egenimos" && population.classType == Population.ClassType.Habitant && population.laborType == Population.LaborType.Housekeeper)
        //{
        //    debugPop = population;
        //}
        //newParticipant.participantName = "PTC" + newParticipant.linkedEcoBlock.blockID + WorldController.Instance.GetWorld().completedCycles;
        //participantList.Add(newParticipant);
    }
    void ShopAndExchange (Participant participant, List<Participant> pList)
    {
        foreach (Participant passiveParticipant in pList)
        {
            if (participant != passiveParticipant)
            {
                participant.shoppingList = new List<Resource>();
                foreach (Resource.Type restype in passiveParticipant.offeredResources.Keys)
                    if (participant.wantedResources.ContainsKey(restype))
                    { 
                        if (passiveParticipant.offeredResources[restype] > 0)
                        {
                            if (participant.wantedResources[restype] >= passiveParticipant.offeredResources[restype])
                            {
                               Debug.Log(participant.participantName + " shopped for what was available of " + restype + " at " + passiveParticipant.participantName);
                                participant.shoppingList.Add(new Resource(restype, passiveParticipant.offeredResources[restype])); // take all
                                participant.totalShoppingList[restype] += passiveParticipant.offeredResources[restype];
                            }
                            else
                            {
                                Debug.Log(participant.participantName + " shopped for what they wanted of " + restype + " at " + passiveParticipant.participantName);
                                participant.shoppingList.Add(new Resource(restype, participant.wantedResources[restype])); // take desired
                                participant.totalShoppingList[restype] += participant.wantedResources[restype]; 
                            }
                        }
                    }

                float shoppingValue = 0f;
                foreach (Resource shopres in participant.shoppingList)                
                    shoppingValue += Converter.GetSilverEquivalent(shopres); 
                
                if (shoppingValue > 0)
                {
                  Debug.Log("Creating exchange for active: " + participant.participantName + " with passive: " + passiveParticipant.participantName);
                    ExchangeController.Instance.CreateLocalExchange(this, participant, passiveParticipant, participant.shoppingList);
                }

            }
        }
    }

    void ShopAround (Participant participant)
    {
        ShopAndExchange(participant, participantsHigh);
        ShopAndExchange(participant, participantsMid);
        ShopAndExchange(participant, participantsLow); 
    }

    public override void ResolveMarket()
    {
        // Compile types and amounts of wanted resources
        //foreach (Participant participant in participantList)
        //{
        //    foreach (Resource.Type restype in participant.wantedResources.Keys)
        //    {
        //        if (this.wantedResourceTypes.Contains(restype) == false)
        //            this.wantedResourceTypes.Add(restype);
        //        if (this.totalWantedResources.ContainsKey(restype) == false)
        //            this.totalWantedResources.Add(restype, participant.wantedResources[restype]);
        //        else
        //            this.totalWantedResources[restype] += participant.wantedResources[restype];
        //    }
        //}

        // Divide Ps into priority-based sublists
        foreach (Participant participant in participantList)
        {
            if (participant.priorityLevel == Participant.PriorityLevel.HI)
                participantsHigh.Add(participant);
            else if (participant.priorityLevel == Participant.PriorityLevel.MID)
                participantsMid.Add(participant);
            else
                participantsLow.Add(participant);
        }
        // Shuffle each priority list
        ShuffleParticipants();


        foreach (Participant participant in participantsHigh)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ShopAround(participant);

        foreach (Participant participant in participantsMid)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ShopAround(participant);

        foreach (Participant participant in participantsLow)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ShopAround(participant);


 
        //// Collect all resources offered by all Ps and add them to totalOfferedResources dict
        //foreach (Participant participant in participantList)
        //{
        //    foreach (Resource.Type restype in participant.offeredResources.Keys)
        //        if (participant.offeredResources[restype] > 0)
        //        {
        //            if (totalOfferedResources.ContainsKey(restype))
        //                totalOfferedResources[restype] += participant.offeredResources[restype];
        //            else
        //                totalOfferedResources.Add(restype, participant.offeredResources[restype]);
        //        }
        //} 

        // // In priority order, each P claims a portion of the offered resources
        //foreach (Participant participant in participantsHigh)
        //    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        ClaimResources(participant);

        //foreach (Participant participant in participantsMid)
        //     if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        ClaimResources(participant);

        //foreach (Participant participant in participantsLow)
        //     if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        ClaimResources(participant);

        //// Prepare dictionary
        //foreach (Resource.Type restype in totalClaimedResources.Keys)
        //    totalDeductedResources.Add(restype, 0);

        //// In priority order, each P sells their resources as far as they're claimed
        //foreach (Participant participant in participantsHigh)
        //     if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        DeductResources(participant);
        //foreach (Participant participant in participantsMid)
        //     if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        DeductResources(participant);
        //foreach (Participant participant in participantsLow)
        //     if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        DeductResources(participant);

        //// In priority older, each P squares their sell value against their take value, then gets either Pos or Neg credit
        //foreach (Participant participant in participantsHigh)
        //    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        SetCreditStatus(participant);
        //foreach (Participant participant in participantsMid)
        //    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        SetCreditStatus(participant);
        //foreach (Participant participant in participantsLow)
        //    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        SetCreditStatus(participant);


        //foreach (Participant participant in participantList)
        //    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        ProcessCredit(participant);

        //foreach (Participant participant in participantList)
        //    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        //        ResolveCredit(participant);



        //// For each P, if they have a Poscredit, collect silver from the Ps with Neg credit
        ////foreach (Participant participant in participantsHigh)
        ////    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        ////        ProcessCredit(participant);
        ////foreach (Participant participant in participantsMid)
        ////    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        ////        ProcessCredit(participant);
        ////foreach (Participant participant in participantsLow)
        ////    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        ////        ProcessCredit(participant);

        ////foreach (Participant participant in participantsHigh)
        ////    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        ////        ResolveCredit(participant);
        ////foreach (Participant participant in participantsMid)
        ////    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        ////        ResolveCredit(participant);
        ////foreach (Participant participant in participantsLow)
        ////    if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
        ////        ResolveCredit(participant);


        //CompileSoldPercentages();

        //foreach (Participant participant in participantList)
        //    SellResources(participant); 
        //foreach (Participant participant in participantList)
        //    BalancePayments(participant); 
    }


    // For each resource P wants, if it's on offer, claim as much as possible to their max wants, 
    void ClaimResources (Participant participant)
    {
        //foreach (Resource.Type wantedrestype in participant.wantedResources.Keys)
        //    if (participant.wantedResources[wantedrestype] > 0) // wants its
        //    {
        //      //  Debug.Log("P " + participant.participantName + " wants "+ participant.wantedResources[wantedrestype] +wantedrestype);
        //        if (totalOfferedResources.ContainsKey(wantedrestype)) // it's on offer
        //        {
        //            if (totalOfferedResources[wantedrestype] > 0) // verify on offer
        //            {
        //              //  Debug.Log("P " + participant.participantName + " sees on offer  " + totalOfferedResources[wantedrestype] + wantedrestype);
        //                if (participant.wantedResources[wantedrestype] >= totalOfferedResources[wantedrestype]) // wants more/all that is available        
        //                {
        //                    float maxClaimValue = participant.GetPotentialSpendingPower() - participant.GetClaimedValue();
        //                    float claimedResourceValue = Converter.GetSilverEquivalent(new Resource(wantedrestype, participant.wantedResources[wantedrestype]));
        //                    if (maxClaimValue > claimedResourceValue)
        //                    {
        //                        ClaimResource(participant, new Resource(wantedrestype, totalOfferedResources[wantedrestype]));
        //                    }
        //                    else
        //                    { 
        //                        ClaimResource(participant, new Resource(wantedrestype, Converter.GetResourceEquivalent(participant, wantedrestype, maxClaimValue).amount)); 
        //                    }
        //                }                            
        //                else // wants less than is available                        
        //                {
        //                    float maxClaimValue = participant.GetPotentialSpendingPower();
        //                    float claimedResourceValue = Converter.GetSilverEquivalent(new Resource(wantedrestype, participant.wantedResources[wantedrestype]));

        //                    if (maxClaimValue > claimedResourceValue)
        //                    {
        //                        ClaimResource(participant, new Resource(wantedrestype, participant.wantedResources[wantedrestype]));
        //                    }
        //                    else
        //                    {
        //                        ClaimResource(participant, new Resource(wantedrestype, Converter.GetResourceEquivalent(participant, wantedrestype, maxClaimValue).amount));
        //                    }
        //                }
        //            }    
        //        }
        //    }
    } 
            //actualClaimedResources.Add(restype, 0); 
            //totalDeductedResources.Add(restype, 0);

        // For each res type in totalClaimed dict, if there is still more claimeds to be deducted, if P has it on offer, deduct it from them
    void DeductResources (Participant participant)
    { 
        foreach (Resource.Type restype in totalClaimedResources.Keys)
        {           
            if (totalClaimedResources[restype] > 0)
            {                
                if (totalClaimedResources[restype] > totalDeductedResources[restype]) // not all that can be claimed, have been claimed already
                {
                    float remainedToClaim = totalClaimedResources[restype] - totalDeductedResources[restype];
                    if (participant.offeredResources.ContainsKey(restype)) // offers the resource
                    {
                         if (participant.offeredResources[restype] > 0) // verify that offers it
                        {
                            if (participant.offeredResources[restype] > remainedToClaim) // has more than is still left to claim
                            {
                                DeductResource(participant, new Resource (restype, remainedToClaim));  
                            }
                            else // offers all it has offered
                            {
                                DeductResource(participant, new Resource(restype, participant.offeredResources[restype])); 
                            }
                        }
                    }
                }
            }
        }
    }

    // For each resource that was deducted, add value to total. 
    void SetCreditStatus (Participant participant)
    {
        participant.totalDeductedValue = 0f;
        participant.totalClaimedValue = 0f;

        // Determine totals
        foreach (Resource.Type restype in participant.deductedResources.Keys)
            if (participant.deductedResources[restype] > 0)
                participant.totalDeductedValue += Converter.GetSilverEquivalent(new Resource(restype, participant.deductedResources[restype]));
        foreach (Resource.Type restype in participant.claimedResources.Keys)
            if (participant.claimedResources[restype] > 0)
                participant.totalClaimedValue += Converter.GetSilverEquivalent(new Resource(restype, participant.claimedResources[restype]));


        participant.silverCreditStatus = (participant.totalDeductedValue - participant.totalClaimedValue);
 
         Debug.Log("pop " + participant.linkedEcoBlock.blockID + " tot deducted: " + participant.totalDeductedValue + " tot claimed: " + participant.totalClaimedValue);
        Debug.Log("P " + participant.participantName + " creditstatus " + participant.silverCreditStatus);



    }

    void ProcessCredit(Participant participant)
    { 
        if (participant.silverCreditStatus > 0) // is owed silver by other Ps
        {
            foreach (Participant payingParti in participantList)
            {
                if ( participant.silverCreditStatus > participant.receivedSilver && payingParti.silverCreditStatus < 0)
                {
                    float openCredit = participant.silverCreditStatus - participant.receivedSilver;
                    float maxPayable = Mathf.Abs(payingParti.silverCreditStatus) - payingParti.paidSilver;
                    float toPay;

                    if (openCredit >= maxPayable)
                        toPay = maxPayable;
                    else
                        toPay = openCredit;

                    participant.receivedSilver += toPay;
                    payingParti.paidSilver += toPay;
                }
            }
          
        }
    }
    void ResolveCredit(Participant participant)
    {
        participant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount += participant.receivedSilver;
        participant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount -= participant.paidSilver;
    }
    void ResolveBalances (Participant participant)
    {
        //// add claimed to resourceportfolio
        //foreach (Resource.Type restype in participant.claimedResources.Keys)
        //{
        //    participant.linkedEcoBlock.resourcePortfolio[restype].amount += participant.claimedResources[restype];
        //}
        //// remove deducted from resportfolio
        //foreach (Resource.Type restype in participant.deductedResources.Keys)
        //{
        //    participant.linkedEcoBlock.resourcePortfolio[restype].amount -= participant.deductedResources[restype];
        //}

        //if (participant.creditPositive > 0 )
        //{
        //    participant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount -= participant.creditPositive;
        //}
        //else if (participant.silverCreditStatus > 0)
        //{
        //    participant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount += participant.silverCreditStatus;
        //}
        //// pay or receive silver from/to resport


    }






    void SellResources (Participant participant)
    {
        //foreach (Resource.Type restype in participant.offeredResources.Keys) 
        //    if (resourceSoldPercentages.ContainsKey(restype))
        //    {
        //        if (participant.offeredResources[restype] > 0)
        //        {
        //            SellResource(participant, new Resource(restype, (participant.offeredResources[restype] * resourceSoldPercentages[restype])));
        //        }
        //    }       
    }
    public void SellResource(Participant participant, Resource resource)
    {
    //    participant.linkedEcoBlock.resourcePortfolio[resource.type].amount -= resource.amount;
    //    participant.creditPositive += resource.GetSilverValue();
    }
    void BalancePayments (Participant participant)
    {
        // each part has chosen from among the market what they want. Then sells proportion of their stuff according to total proportion of that resource is sold, is left with
        // a positive balance.
        // It will pay for its chosen resources from that positive balance first, then from its silver portfolio. If it has more sold than buying, get it in silver

    //    Debug.Log(participant.linkedEcoBlock.blockID + " has buyvalue: " + participant.buyValue + " and sellvalue: " + participant.soldValue);


    }



    void ClaimResource(Participant participant, Resource resource)
    {
     //   Debug.Log("Claiming " + resource.amount + resource.type + " for  " + participant.participantName);
        participant.claimedResources.Add(resource.type, resource.amount);
        participant.linkedEcoBlock.resourcePortfolio[resource.type].amount += resource.amount;

        if (totalClaimedResources.ContainsKey(resource.type))        
            totalClaimedResources[resource.type] += resource.amount;        
        else
            totalClaimedResources.Add(resource.type, resource.amount);

        //bool debugging = false;
        //if ((Population)participant.linkedEcoBlock == debugPop)
        //{
        //    debugging = true;
        //}
        //if (participant.GetSpendingPower() > resource.GetSilverValue())
        //{
        ////    Debug.Log(participant.linkedEcoBlock.blockID + " is buying: " + resource.amount + resource.type);
        //    participant.purchasedResources.Add(resource);
        //    totalOfferedResources[resource.type] -= resource.amount;
        //    if (tradedResourceTypes.Contains(resource.type) == false)
        //        tradedResourceTypes.Add(resource.type);


            //    if (debugging)
            //    {
            //        Debug.Log(debugPop.blockID + " has claimed: " + resource.type + " amount: " + resource.amount);
            //        Debug.Log("now available at market: " + totalOfferedResources[resource.type]);
            //        Debug.Log("market: " + marketName);
            //    }
            //}
            //else
            //{

            //    Resource adjustedResource = Converter.GetResourceEquivalent(resource.type, participant.GetSpendingPower());
            // //   Debug.Log(participant.linkedEcoBlock.blockID + " is partially buying: " + adjustedResource.amount + adjustedResource.type);

            //    participant.purchasedResources.Add(adjustedResource);
            //    totalOfferedResources[adjustedResource.type] -= adjustedResource.amount;
            //    if (tradedResourceTypes.Contains(adjustedResource.type) == false)
            //        tradedResourceTypes.Add(adjustedResource.type);
            //    if (debugging)
            //    {
            //        Debug.Log(debugPop.blockID + " has claimed: " + adjustedResource.type + " amount: " + adjustedResource.amount);
            //        Debug.Log("now vailable at market: " + totalOfferedResources[adjustedResource.type]);
            //    }
            //}

    }

    void DeductResource(Participant participant, Resource resource)
    { 
        participant.deductedResources.Add(resource.type, resource.amount);
        participant.linkedEcoBlock.resourcePortfolio[resource.type].amount -= resource.amount;
        totalDeductedResources[resource.type] += resource.amount; 
    }
    void CompileSoldPercentages()
    {
        foreach (Resource.Type restype in tradedResourceTypes)
        {
            float totalOfferedAmount = 0;
            foreach (Participant parti in participantList)
                if (parti.offeredResources.ContainsKey(restype))
                    if (parti.offeredResources[restype] > 0)
                        totalOfferedAmount += parti.offeredResources[restype];
            float totalPurchasedAmount = 0f;
            foreach (Participant parti in participantList)
                foreach (Resource res in parti.purchasedResources)
                    if (res.type == restype)
                        totalPurchasedAmount += res.amount;
            float totalOfferingSoldPercentage = (totalOfferedAmount * totalPurchasedAmount) / 100;
            resourceSoldPercentages.Add(restype, totalOfferingSoldPercentage);
        }
      
    }
   
  



    void ShuffleParticipants()
    {
        List<Participant> shuffledHigh = new List<Participant>();
        List<Participant> shuffledMid = new List<Participant>();
        List<Participant> shuffledLow = new List<Participant>();
        foreach (Participant part in participantsHigh)
            shuffledHigh.Add(part);
        foreach (Participant part in participantsMid)
            shuffledMid.Add(part);
        foreach (Participant part in participantsLow)
            shuffledLow.Add(part);
        for (int i = 0; i < shuffledHigh.Count; i++)
        {
            Participant temp = shuffledHigh[i];
            int randomIndex = Random.Range(i, shuffledHigh.Count);
            shuffledHigh[i] = shuffledHigh[randomIndex];
            shuffledHigh[randomIndex] = temp;
        }
        for (int i = 0; i < shuffledMid.Count; i++)
        {
            Participant temp = shuffledMid[i];
            int randomIndex = Random.Range(i, shuffledMid.Count);
            shuffledMid[i] = shuffledMid[randomIndex];
            shuffledMid[randomIndex] = temp;
        }
        for (int i = 0; i < shuffledLow.Count; i++)
        {
            Participant temp = shuffledLow[i];
            int randomIndex = Random.Range(i, shuffledLow.Count);
            shuffledLow[i] = shuffledLow[randomIndex];
            shuffledLow[randomIndex] = temp;
        }
        participantsHigh = new List<Participant>();
        participantsMid = new List<Participant>();
        participantsLow = new List<Participant>();
        foreach (Participant participant in shuffledHigh)
            participantsHigh.Add(participant);
        foreach (Participant participant in shuffledMid)
            participantsMid.Add(participant);
        foreach (Participant participant in shuffledLow)
            participantsLow.Add(participant);
    }
}
