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
    public Dictionary<Resource.Type, int> resourceBuyPriority = new Dictionary<Resource.Type, int>();
    public Dictionary<Resource.Type, int> resourceSellPriority = new Dictionary<Resource.Type, int>();
    public Dictionary<Resource.Type, float> initialOfferedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> offeredResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> initialWantedResources = new Dictionary<Resource.Type, float>(); 
    public Dictionary<Resource.Type, float> wantedResources = new Dictionary<Resource.Type, float>(); 

     

    public bool spendingSilver = true;
    public float storedSpendingPower;

    public List<Resource> shoppingList = new List<Resource>();
    public Dictionary<Resource.Type, float> totalShoppingList = new Dictionary<Resource.Type, float>();

      

    public Participant()
    {
        ResourceController.Instance.InitiateResourceDictionary(totalShoppingList);
    }
    


    public float GetPotentialSpendingPower(Participant shopParticipant)
    {
        // how much silver value spending power with this given P? 
         
        float totalAmount = 0f;

        if (spendingSilver == true)
               totalAmount += linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount;

        foreach (Resource.Type restype in shopParticipant.wantedResources.Keys)
        {
            if (this.offeredResources.ContainsKey (restype))
            {
                if (this.offeredResources[restype] >= shopParticipant.wantedResources[restype])                
                    totalAmount += Converter.GetSilverEquivalent(new Resource(restype, shopParticipant.wantedResources[restype]));                
                else
                    totalAmount += Converter.GetSilverEquivalent(new Resource(restype, this.offeredResources[restype]));
            }
        }       
        return totalAmount;
    }

    //public float GetPotentialSpendingPower()
    //{
    //    Market market = MarketController.Instance.GetMarketForParticipant(this);
    //    float totalAmount = 0f;
    //    totalAmount += linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount;
    //    foreach (Resource.Type restype in offeredResources.Keys)
    //        if (market.wantedResourceTypes.Contains(restype))
    //        {
    //            if (market.totalWantedResources[restype] >= offeredResources[restype])
    //            {
    //                totalAmount += Converter.GetSilverEquivalent(new Resource(restype, offeredResources[restype]));
    //            }
    //            else
    //            {
    //                totalAmount += Converter.GetSilverEquivalent(new Resource(restype, market.totalWantedResources[restype]));
    //            }
    //        }
    //    storedSpendingPower = totalAmount;
    //    return totalAmount;
    //}
  
}
