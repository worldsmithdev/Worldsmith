using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMarket : Market
{
     
    string debugLocName = "Gela"; 
    Location debugLoc; 



    public List<LocalExchange> localExchangesList = new List<LocalExchange>();

    List<Participant> participantsHigh = new List<Participant>();
    List<Participant> participantsMid = new List<Participant>();
    List<Participant> participantsLow = new List<Participant>();
   

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

         
       
        newParticipant.participantName = "P" + newParticipant.linkedEcoBlock.blockID + WorldController.Instance.GetWorld().completedCycles; 
        participantList.Add(newParticipant);

    }
    public void RegisterAsRuler (Ruler ruler)
    {
        Participant newParticipant = new Participant();

        newParticipant.linkedEcoBlock = ruler;
        newParticipant.type = Participant.Type.Ruler;
        if (ruler.isLocalRuler == true)
            newParticipant.priorityLevel = Participant.PriorityLevel.HI; 
        else
            newParticipant.priorityLevel = Participant.PriorityLevel.MID;

        newParticipant.resourceBuyPriority = ruler.resourceBuyPriority;
        newParticipant.resourceSellPriority = ruler.resourceSellPriority;

        foreach (Resource.Type restype in ruler.cycleLocalSurplusResources.Keys)
        {
            newParticipant.initialOfferedResources.Add(restype, ruler.cycleLocalSurplusResources[restype]);
            newParticipant.offeredResources.Add(restype, ruler.cycleLocalSurplusResources[restype]);
        }
        foreach (Resource.Type restype in ruler.cycleLocalWantedResources.Keys)
        {
            newParticipant.initialWantedResources.Add(restype, ruler.cycleLocalWantedResources[restype]);
            newParticipant.wantedResources.Add(restype, ruler.cycleLocalWantedResources[restype]);
        }  

    //    Debug.Log("name" + newParticipant.linkedEcoBlock.blockID);
        newParticipant.participantName = "P" + newParticipant.linkedEcoBlock.blockID + WorldController.Instance.GetWorld().completedCycles;
        participantList.Add(newParticipant);
    }
    void ShopAndExchange (Participant participant, List<Participant> pList)
    {
        foreach (Participant passiveParticipant in pList)
        {
            if (participant != passiveParticipant)
            {
                participant.shoppingList = new List<Resource>();

                // Check each shops'wares
                // Then create a shopping list
                foreach (Resource.Type restype in passiveParticipant.offeredResources.Keys)
                    if (participant.wantedResources.ContainsKey(restype) && passiveParticipant.offeredResources[restype] > 0)
                    {                         
                            if (participant.wantedResources[restype] >= passiveParticipant.offeredResources[restype])
                            {
                          //    Debug.Log(participant.participantName + " shopped for what was available of " + restype + " at " + passiveParticipant.participantName);
                                participant.shoppingList.Add(new Resource(restype, passiveParticipant.offeredResources[restype])); // take all
                                participant.totalShoppingList[restype] += passiveParticipant.offeredResources[restype];
                            }
                            else
                            {
                          //    Debug.Log(participant.participantName + " shopped for what they wanted of " + restype + participant.wantedResources[restype]+ " at " + passiveParticipant.participantName);
                                participant.shoppingList.Add(new Resource(restype, participant.wantedResources[restype])); // take desired
                                participant.totalShoppingList[restype] += participant.wantedResources[restype]; 
                            }                        
                    }

                float shoppingValue = 0f;
                foreach (Resource shopres in participant.shoppingList)                
                    shoppingValue += Converter.GetSilverEquivalent(shopres); 
                
                if (shoppingValue > 0)
                {
               //   Debug.Log("Creating exchange for active: " + participant.participantName + " with passive: " + passiveParticipant.participantName);
                    ExchangeController.Instance.CreateLocalExchange(LocalExchange.Type.Market, this, participant, passiveParticipant, participant.shoppingList);
                }

            }
        }
    }

    void ShopForProfit(Participant participant, List<Participant> pList)
    {

        foreach (Participant passiveParticipant in pList)
        {
            if (participant != passiveParticipant)
            {
                participant.shoppingList = new List<Resource>();

                foreach (Resource.Type restype in participant.linkedEcoBlock.localProfitResourceTypes)
                {
                    if (passiveParticipant.offeredResources.ContainsKey(restype) && passiveParticipant.offeredResources[restype] > 0)
                    {                        
                        participant.shoppingList.Add(new Resource(restype, passiveParticipant.offeredResources[restype]));                        
                    } 
                }

                float shoppingValue = 0f;
                foreach (Resource shopres in participant.shoppingList)
                    shoppingValue += Converter.GetSilverEquivalent(shopres);

                if (shoppingValue > 0)
                    ExchangeController.Instance.CreateLocalExchange(LocalExchange.Type.Purchase, this, participant, passiveParticipant, participant.shoppingList);
            }
        }
 
    }
    void ShopAround (Participant participant)
    {
        ShopAndExchange(participant, participantsHigh);
        ShopAndExchange(participant, participantsMid);
        ShopAndExchange(participant, participantsLow);

        ShopForProfit(participant, participantsHigh);
        ShopForProfit(participant, participantsMid);
        ShopForProfit(participant, participantsLow); 
    }

    public override void ResolveMarket()
    {
         
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
      //      if (currentLocName == debugLocName)
                ShopAround(participant);

        foreach (Participant participant in participantsMid)
    //        if (currentLocName == debugLocName)
                ShopAround(participant);

        foreach (Participant participant in participantsLow)
    //        if (currentLocName == debugLocName)
                ShopAround(participant); 
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
