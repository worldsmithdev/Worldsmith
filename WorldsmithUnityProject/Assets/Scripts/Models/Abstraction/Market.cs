using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market  
{

    public string marketName;
    public int exchangesAmount;
    // Collects all resources that are wanted and offered, the parties they are offered by, and the priority they have in shopping around
    public List<Participant> participantList = new List<Participant>();

    public List<Resource> activelyExchangedResources = new List<Resource>();
    public List<Resource> passivelyExchangedResources = new List<Resource>();
  


    public Dictionary<Resource.Type, float> resourceSoldPercentages = new Dictionary<Resource.Type, float>();

    public float pledgedSilver;

        public Market()
    {

    }

  

   

    public virtual void ResolveMarket()
    {

    }

  

}
