using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market  
{

    public string marketName;
    // Collects all resources that are wanted and offered, the parties they are offered by, and the priority they have in shopping around
    public List<Participant> participantList = new List<Participant>();
    public List<Resource.Type> tradedResourceTypes = new List<Resource.Type>();

    public List<Resource.Type> wantedResourceTypes = new List<Resource.Type>();
    public Dictionary<Resource.Type, float> totalWantedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> totalOfferedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> totalClaimedResources = new Dictionary<Resource.Type, float>(); 
    public Dictionary<Resource.Type, float> totalDeductedResources = new Dictionary<Resource.Type, float>(); 


    public Dictionary<Resource.Type, float> resourceSoldPercentages = new Dictionary<Resource.Type, float>();

    public float pledgedSilver;

        public Market()
    {

    }

  

   

    public virtual void ResolveMarket()
    {

    }

  

}
