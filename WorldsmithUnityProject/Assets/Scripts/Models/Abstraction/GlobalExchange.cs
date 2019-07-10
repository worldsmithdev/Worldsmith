using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalExchange : Exchange
{
    public GlobalMarket globalMarket;

    public EcoBlock activeParty; 
    public List<Resource> activeResources = new List<Resource>();
    public List<Resource> receivedResources = new List<Resource>();




    public void CommitResource( Resource resource)
    {
        if (resource.amount > 0)
        { 
             activeResources.Add(resource);  
        }
    }

    public override void ResolveExchange()
    {
        foreach (Resource res in activeResources)        
            if (res != null && res.amount > 0)
            {
                activeParty.resourcePortfolio[res.type].amount -= res.amount; 
            }
        foreach (Resource res in receivedResources)        
            if (res != null && res.amount > 0)
                if (res != null && res.amount > 0)
                {
                    activeParty.resourcePortfolio[res.type].amount += res.amount;  
                }
    }


}
