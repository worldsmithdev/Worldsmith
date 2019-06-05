using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMarket : Market
{

    public Dictionary<Resource.Type, float> totalOfferedResources = new Dictionary<Resource.Type, float>();

    List<Participant> participantsHigh = new List<Participant>();
    List<Participant> participantsMid = new List<Participant>();
    List<Participant> participantsLow = new List<Participant>();

    Dictionary<Resource.Type, float> resourceSoldPercentages = new Dictionary<Resource.Type, float>();

    Population debugPop;
    Participant debugParticipant;

    public LocalMarket(Location loc)
    {
        marketName = "LocalMarket " + WorldController.Instance.GetWorld().completedCycles;
        MarketController.Instance.cycleLocalMarkets.Add(this);
        MarketController.Instance.archivedLocalMarkets[loc].Add(this);
    }


    public void RegisterAsPopulation (Population population)
    {
        Participant newParticipant = new Participant();

   

        
        newParticipant.linkedEcoBlock = population;
        newParticipant.type = Participant.Type.Population;
        if (population.classType == Population.ClassType.Citizen)
            newParticipant.priorityLevel = Participant.PriorityLevel.High;
        else
            newParticipant.priorityLevel = Participant.PriorityLevel.Low;
        newParticipant.offeredResources = new Dictionary<Resource.Type, float>();
        newParticipant.wantedResources = new Dictionary<Resource.Type, float>();
        newParticipant.purchasedResources = new List<Resource>();
        newParticipant.soldValue = 0f;

        foreach (Resource.Type restype in population.cycleLocalSurplusResources.Keys)        
            newParticipant.offeredResources.Add(restype, population.cycleLocalSurplusResources[restype]);
        foreach (Resource.Type restype in population.cycleLocalWantedResources.Keys)
            newParticipant.wantedResources.Add(restype, population.cycleLocalWantedResources[restype]);

        if (population.GetHomeLocation().elementID == "Egenimos" && population.classType == Population.ClassType.Habitant && population.laborType == Population.LaborType.Housekeeper)
        {
            debugPop = population; 
        }
        newParticipant.participantName = "PTC" + newParticipant.linkedEcoBlock.blockID + WorldController.Instance.GetWorld().completedCycles; 
        participantList.Add(newParticipant);

    }
    public void RegisterAsRuler (Ruler ruler)
    {

    }


    public override void ResolveMarket()
    { 
        // Collect all resources offered by all parties
        CompileTotalOffered();
        // Divide parties into priority-based subsets
        AssignPriorities();
        // Shuffle to randomize
        ShuffleParticipants();



        // Take dibs on resources to buy
        //foreach (Participant participant in participantsHigh)
        //    ShopForResources(participant);
        //foreach (Participant participant in participantsMid)
        //    ShopForResources(participant);
        foreach (Participant participant in participantsLow)
            if (participant.linkedEcoBlock == debugPop)
            ShopForResources(participant);

        //CompileSoldPercentages();

        //foreach (Participant participant in participantList)
        //    SellResources(participant); 
        //foreach (Participant participant in participantList)
        //    BalancePayments(participant); 
    }


    void ShopForResources (Participant participant)
    {
        foreach (Resource.Type wantedrestype in participant.wantedResources.Keys)
            if (participant.wantedResources[wantedrestype] > 0)
            {             
                if (totalOfferedResources.ContainsKey(wantedrestype))
                {
                    if (participant.wantedResources[wantedrestype] >= totalOfferedResources[wantedrestype]) // wants more than is available
                    {
                        if (totalOfferedResources[wantedrestype] > 0)                        
                            ClaimResource(participant, new Resource(wantedrestype, totalOfferedResources[wantedrestype]));
                    }
                    else // wants less than is available
                    {
                        ClaimResource(participant, new Resource(wantedrestype, participant.wantedResources[wantedrestype])); 
                    } 
                }
            }
        foreach (Resource res in participant.purchasedResources)        
            if (res != null)
              participant.buyValue += res.GetSilverValue();        
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
        participant.soldValue += resource.GetSilverValue();
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
        bool debugging = false;
        if ((Population)participant.linkedEcoBlock == debugPop)
        {
            debugging = true;
        }
        if (participant.GetSpendingPower() > resource.GetSilverValue())
        {
        //    Debug.Log(participant.linkedEcoBlock.blockID + " is buying: " + resource.amount + resource.type);
            participant.purchasedResources.Add(resource);
            totalOfferedResources[resource.type] -= resource.amount;
            if (tradedResourceTypes.Contains(resource.type) == false)
                tradedResourceTypes.Add(resource.type);


            if (debugging)
            {
                Debug.Log(debugPop.blockID + " has claimed: " + resource.type + " amount: " + resource.amount);
                Debug.Log("now available at market: " + totalOfferedResources[resource.type]);
                Debug.Log("market: " + marketName);
            }
        }
        else
        {

            Resource adjustedResource = Converter.GetResourceEquivalent(resource.type, participant.GetSpendingPower());
         //   Debug.Log(participant.linkedEcoBlock.blockID + " is partially buying: " + adjustedResource.amount + adjustedResource.type);

            participant.purchasedResources.Add(adjustedResource);
            totalOfferedResources[adjustedResource.type] -= adjustedResource.amount;
            if (tradedResourceTypes.Contains(adjustedResource.type) == false)
                tradedResourceTypes.Add(adjustedResource.type);
            if (debugging)
            {
                Debug.Log(debugPop.blockID + " has claimed: " + adjustedResource.type + " amount: " + adjustedResource.amount);
                Debug.Log("now vailable at market: " + totalOfferedResources[adjustedResource.type]);
            }
        }
     
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
    void CompileTotalOffered()
    {
        foreach (Participant participant in participantList)        
            foreach (Resource.Type restype in participant.offeredResources.Keys)            
                if (participant.offeredResources[restype] > 0)
                {
                    if (totalOfferedResources.ContainsKey(restype))
                        totalOfferedResources[restype] += participant.offeredResources[restype];
                    else
                        totalOfferedResources.Add(restype, participant.offeredResources[restype]);
                } 
    }
   void AssignPriorities()
    {
        foreach (Participant participant in participantList)
        {
            if (participant.priorityLevel == Participant.PriorityLevel.High)
                participantsHigh.Add(participant);
            else if (participant.priorityLevel == Participant.PriorityLevel.Mid)
                participantsMid.Add(participant);
            else 
                participantsLow.Add(participant);
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
