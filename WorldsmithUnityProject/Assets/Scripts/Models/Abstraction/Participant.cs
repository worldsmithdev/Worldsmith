using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant 
{
    public string participantName;
    public EcoBlock linkedEcoBlock;
    public enum Type { Population, Ruler }
    public enum PriorityLevel { LO, MID, HI }
    public Type type;
    public PriorityLevel priorityLevel;
    public Dictionary<Resource.Type, float> offeredResources;
    public Dictionary<Resource.Type, float> wantedResources;
    public Dictionary<Resource.Type, float> claimedResources;
    public Dictionary<Resource.Type, float> deductedResources;



    public Dictionary<Resource.Type, float> outTradedResources;
    public Dictionary<Resource.Type, float> inTradedResources;


    public float storedSpendingPower;
    public float totalDeductedValue;
    public float totalClaimedValue;
    public float silverCreditStatus;
    public float paidSilver;
    public float receivedSilver;

    public List<Resource> purchasedResources;

    
    public float GetPotentialSpendingPower()
    {
        Market market = MarketController.Instance.GetMarketForParticipant(this);
        float totalAmount = 0f;
        totalAmount += linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount;
        foreach (Resource.Type restype in offeredResources.Keys)
            if (market.wantedResourceTypes.Contains(restype))
            {
                if (market.totalWantedResources[restype] >= offeredResources[restype])
                {
                    totalAmount += Converter.GetSilverEquivalent(new Resource(restype, offeredResources[restype]));
                }
                else
                {
                    totalAmount += Converter.GetSilverEquivalent(new Resource(restype, market.totalWantedResources[restype]));
                }
            }
        storedSpendingPower = totalAmount;
        return totalAmount;
    }
    public float GetClaimedValue()
    {
        Market market = MarketController.Instance.GetMarketForParticipant(this);
        float totalAmount = 0f;
         
        foreach (Resource.Type restype in claimedResources.Keys)
            if (claimedResources[restype] > 0)
            { 
                    totalAmount += Converter.GetSilverEquivalent(new Resource(restype, offeredResources[restype])); 
            }

        return totalAmount;
    }
}
