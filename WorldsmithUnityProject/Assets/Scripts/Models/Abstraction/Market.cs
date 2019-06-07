using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market  
{

    public string marketName;
    // Collects all resources that are wanted and offered, the parties they are offered by, and the priority they have in shopping around
    public List<Participant> participantList = new List<Participant>();
    public List<Resource.Type> tradedResourceTypes = new List<Resource.Type>();

    public float pledgedSilver;

        public Market()
    {

    }

    public struct Participant
    {
        public string participantName;
        public EcoBlock linkedEcoBlock;
        public enum Type { Population, Ruler}
        public enum PriorityLevel { LO, MID, HI}
        public Type type;
        public PriorityLevel priorityLevel;
        public Dictionary<Resource.Type, float> offeredResources;
        public Dictionary<Resource.Type, float> wantedResources;
        public Dictionary<Resource.Type, float> claimedResources; 
        public Dictionary<Resource.Type, float> deductedResources; 
        public Dictionary<Resource.Type, float> outTradedResources;
        public Dictionary<Resource.Type, float> inTradedResources;

       
        public float silverOwed;
        public float silverOwing;

        public List<Resource> purchasedResources;

        public float GetSpendingPower()
        {
            float totalAmount = 0f;
            totalAmount += linkedEcoBlock.resourcePortfolio[Resource.Type.Silver].amount;
            foreach (Resource.Type restype in offeredResources.Keys)            
                totalAmount += Converter.GetSilverEquivalent(new Resource(restype, offeredResources[restype]));
            return totalAmount;
        }
    }

   

    public virtual void ResolveMarket()
    {

    }

  

}
