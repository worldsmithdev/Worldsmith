﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalExchange : Exchange
{

    public enum Type { Unassigned, Free, Forced, Seized };

    public Type  type;
    public RegionalMarket regionalMarket;

    public EcoBlock activeParty;
    public EcoBlock passiveParty;
    public List<Resource> activeResources = new List<Resource>();
    public List<Resource> passiveResources = new List<Resource>();



    public override void ResolveExchange()
    {
        foreach (Resource res in activeResources)
        {
            if (res != null && res.amount > 0)
            {
                activeParty.resourcePortfolio[res.type].amount -= res.amount;
                passiveParty.resourcePortfolio[res.type].amount += res.amount;
            } 
        }
        foreach (Resource res in passiveResources)
        {
            if (res != null && res.amount > 0)
            {
                activeParty.resourcePortfolio[res.type].amount += res.amount;
                passiveParty.resourcePortfolio[res.type].amount -= res.amount;
            }
        }
    } 

    public void CommitResource(bool byActiveParty, Resource resource)
    {
        if (resource.amount > 0)
        {
            if (byActiveParty == true)
            {
                activeResources.Add(resource); 
                if (activeParty.cycleRegionalCommittedResources.ContainsKey(resource.type))
                    activeParty.cycleRegionalCommittedResources[resource.type] += resource.amount;
                else
                    activeParty.cycleRegionalCommittedResources.Add(resource.type, resource.amount);
            }
            else if (byActiveParty == false)
            {
                passiveResources.Add(resource); 
                if (passiveParty.cycleRegionalCommittedResources.ContainsKey(resource.type))
                    passiveParty.cycleRegionalCommittedResources[resource.type] += resource.amount;
                else
                    passiveParty.cycleRegionalCommittedResources.Add(resource.type, resource.amount);
            }
        }
    }
}
