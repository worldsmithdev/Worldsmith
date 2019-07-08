using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMarket : Market
{
    public List<GlobalExchange> globalExchangesList = new List<GlobalExchange>();

    List<Participant> participantsHigh = new List<Participant>();
    List<Participant> participantsMid = new List<Participant>();
    List<Participant> participantsLow = new List<Participant>();


    public GlobalMarket(Location loc)
    {
        marketName = "GlobalMarket" + WorldController.Instance.GetWorld().completedCycles;
        MarketController.Instance.cycleGlobalMarkets.Add(this);
        MarketController.Instance.archivedGlobalMarkets[loc].Add(this);
    }

    public void RegisterAsRuler(Ruler ruler)
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
}
