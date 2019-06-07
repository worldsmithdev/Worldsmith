using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMarket : Market
{

    public Dictionary<Resource.Type, float> totalOfferedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> sumClaimedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> actualClaimedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> totalDeductedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> resourceSoldPercentages = new Dictionary<Resource.Type, float>();





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
        else
            newParticipant.priorityLevel = Participant.PriorityLevel.LO;
        newParticipant.offeredResources = new Dictionary<Resource.Type, float>();
        newParticipant.wantedResources = new Dictionary<Resource.Type, float>();
        newParticipant.claimedResources = new Dictionary<Resource.Type, float>();
        newParticipant.deductedResources = new Dictionary<Resource.Type, float>();
        newParticipant.outTradedResources = new Dictionary<Resource.Type, float>();
        newParticipant.inTradedResources = new Dictionary<Resource.Type, float>();
        newParticipant.purchasedResources = new List<Resource>();
        newParticipant.silverOwing = 0f;

        foreach (Resource.Type restype in population.cycleLocalSurplusResources.Keys)        
            newParticipant.offeredResources.Add(restype, population.cycleLocalSurplusResources[restype]);
        foreach (Resource.Type restype in population.cycleLocalWantedResources.Keys)
            newParticipant.wantedResources.Add(restype, population.cycleLocalWantedResources[restype]);

        if (population.GetHomeLocation().elementID == "Egenimos")
        {
            debugLoc = population.GetHomeLocation(); 
        }
        newParticipant.participantName = "PTC" + newParticipant.linkedEcoBlock.blockID + WorldController.Instance.GetWorld().completedCycles; 
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


    public override void ResolveMarket()
    { 
        // Collect all resources offered by all parties
                foreach (Participant participant in participantList)        
            foreach (Resource.Type restype in participant.offeredResources.Keys)            
                if (participant.offeredResources[restype] > 0)
                {
                    if (totalOfferedResources.ContainsKey(restype))
                        totalOfferedResources[restype] += participant.offeredResources[restype];
                    else
                        totalOfferedResources.Add(restype, participant.offeredResources[restype]);
                }
        // Divide parties into priority-based subsets
        foreach (Participant participant in participantList)
        {
            if (participant.priorityLevel == Participant.PriorityLevel.HI)
                participantsHigh.Add(participant);
            else if (participant.priorityLevel == Participant.PriorityLevel.MID)
                participantsMid.Add(participant);
            else
                participantsLow.Add(participant);
        }
        // Shuffle to randomize
        ShuffleParticipants();

         
        foreach (Participant participant in participantsHigh)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ClaimResources(participant);
        foreach (Participant participant in participantsMid)
             if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ClaimResources(participant);
        foreach (Participant participant in participantsLow)
             if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ClaimResources(participant);

        // Match dictionaries
        foreach (Resource.Type restype in sumClaimedResources.Keys)
            totalDeductedResources.Add(restype, 0);

        foreach (Participant participant in participantsHigh)
             if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                DeductClaimedResources(participant);
        foreach (Participant participant in participantsMid)
             if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                DeductClaimedResources(participant);
        foreach (Participant participant in participantsLow)
             if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                DeductClaimedResources(participant);

        foreach (Participant participant in participantsHigh)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                MakeBalances(participant);
        foreach (Participant participant in participantsMid)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                MakeBalances(participant);
        foreach (Participant participant in participantsLow)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                MakeBalances(participant);

        foreach (Participant participant in participantsHigh)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ResolveBalances(participant);
        foreach (Participant participant in participantsMid)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ResolveBalances(participant);
        foreach (Participant participant in participantsLow)
            if (((Population)participant.linkedEcoBlock).GetHomeLocation() == debugLoc)
                ResolveBalances(participant);

        //CompileSoldPercentages();

        //foreach (Participant participant in participantList)
        //    SellResources(participant); 
        //foreach (Participant participant in participantList)
        //    BalancePayments(participant); 
    }


    void ClaimResources (Participant participant)
    {
        foreach (Resource.Type wantedrestype in participant.wantedResources.Keys)
            if (participant.wantedResources[wantedrestype] > 0) // wants its
            {             
                if (totalOfferedResources.ContainsKey(wantedrestype)) // it's on offer
                {
                    if (totalOfferedResources[wantedrestype] > 0) // definitely on offer
                    {
                        if (participant.wantedResources[wantedrestype] >= totalOfferedResources[wantedrestype]) // wants more/all that is available        
                        {
                            float maxClaimValue = participant.GetSpendingPower();
                            float claimedResourceValue = Converter.GetSilverEquivalent(new Resource(wantedrestype, participant.wantedResources[wantedrestype]));
                            if (maxClaimValue > claimedResourceValue)
                            {
                                ClaimResource(participant, new Resource(wantedrestype, totalOfferedResources[wantedrestype]));
                            }
                            else
                            { 
                                ClaimResource(participant, new Resource(wantedrestype, Converter.GetResourceEquivalent(wantedrestype, maxClaimValue).amount)); 
                            }

                        }
                            
                        else // wants less than is available                        
                        {
                            float maxClaimValue = participant.GetSpendingPower();
                            float claimedResourceValue = Converter.GetSilverEquivalent(new Resource(wantedrestype, participant.wantedResources[wantedrestype]));

                            if (maxClaimValue > claimedResourceValue)
                            {
                                ClaimResource(participant, new Resource(wantedrestype, participant.wantedResources[wantedrestype]));
                            }
                            else
                            {
                                ClaimResource(participant, new Resource(wantedrestype, Converter.GetResourceEquivalent(wantedrestype, maxClaimValue).amount));
                            }

                        }

                    }    
                }
            }   
    }
    void DeductClaimedResources (Participant participant)
    { 
        foreach (Resource.Type restype in sumClaimedResources.Keys)
        {           
            if (sumClaimedResources[restype] > 0)
            {                
                if (sumClaimedResources[restype] > totalDeductedResources[restype]) // not all that need to be claimed, have been claimed already
                {
                    float remainedToClaim = sumClaimedResources[restype] - totalDeductedResources[restype];
                    if (participant.offeredResources.ContainsKey(restype)) // has the resource
                    {
                         if (participant.offeredResources[restype] > 0) // actually has it
                        {
                            if (participant.offeredResources[restype] > remainedToClaim) // has more than is still left to claim
                            {
                                participant.deductedResources.Add(restype, remainedToClaim);
                                totalDeductedResources[restype] += remainedToClaim;
                            }
                            else // offers all it has offered
                            {
                                participant.deductedResources.Add(restype, participant.offeredResources[restype]);
                                totalDeductedResources[restype] += participant.offeredResources[restype]; 
                            }
                        }
                    }
                }
            }
        }
    }

    void MakeBalances (Participant participant)
    {
        float totalClaimedValue = 0f;
        float totalDeductedValue = 0f;

        foreach (Resource.Type restype in participant.claimedResources.Keys)
            if (participant.claimedResources[restype] > 0)
            {
           //     Debug.Log(" silver equiv for " + restype + participant.claimedResources[restype] + " is " + Converter.GetSilverEquivalent(new Resource(restype, participant.claimedResources[restype])));
                totalClaimedValue += Converter.GetSilverEquivalent(new Resource(restype, participant.claimedResources[restype]));

            }

        foreach (Resource.Type restype in participant.deductedResources.Keys)
            if (participant.deductedResources[restype] > 0)
            {
                totalDeductedValue += Converter.GetSilverEquivalent(new Resource(restype, participant.deductedResources[restype]));

            }

        Debug.Log("pop " + participant.linkedEcoBlock.blockID + " tot claim: " + totalClaimedValue + " tot deduc: "+ totalDeductedValue); 
        if (totalClaimedValue > totalDeductedValue) // if claimed is higher: pay silver into market 
        {
            participant.silverOwing = (totalClaimedValue - totalDeductedValue);
            participant.silverOwed = 0;
        }
        else   // if deducted is higher: take silver out of market 
        {
            participant.silverOwing = 0;
            participant.silverOwed = (totalDeductedValue - totalClaimedValue);
        }
        Debug.Log("pop " + participant.linkedEcoBlock.blockID + " is owing " + participant.silverOwing);
        Debug.Log("pop " + participant.linkedEcoBlock.blockID + " is owed " + participant.silverOwed   );
    }

    void ResolveBalances (Participant participant)
    {
        // add claimed to resourceportfolio
        foreach (Resource.Type restype in participant.claimedResources.Keys)
        {
            participant.linkedEcoBlock.resourcePortfolio[restype].amount += participant.claimedResources[restype];
        }
        // remove deducted from resportfolio
        foreach (Resource.Type restype in participant.deductedResources.Keys)
        {
            participant.linkedEcoBlock.resourcePortfolio[restype].amount -= participant.deductedResources[restype];
        }

        if (participant.silverOwing > 0 )
        {
            participant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount -= participant.silverOwing;
        }
        else if (participant.silverOwed > 0)
        {
            participant.linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount += participant.silverOwed;
        }
        // pay or receive silver from/to resport


    }






    void SellResources (Participant participant)
    {
        foreach (Resource.Type restype in participant.offeredResources.Keys) 
            if (resourceSoldPercentages.ContainsKey(restype))
            {
                if (participant.offeredResources[restype] > 0)
                {
                    SellResource(participant, new Resource(restype, (participant.offeredResources[restype] * resourceSoldPercentages[restype])));
                }
            }       
    }
    public void SellResource(Participant participant, Resource resource)
    {
        participant.linkedEcoBlock.resourcePortfolio[resource.type].amount -= resource.amount;
        participant.silverOwing += resource.GetSilverValue();
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
        participant.claimedResources.Add(resource.type, resource.amount);


        if (sumClaimedResources.ContainsKey(resource.type))        
            sumClaimedResources[resource.type] += resource.amount;        
        else
            sumClaimedResources.Add(resource.type, resource.amount);

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
