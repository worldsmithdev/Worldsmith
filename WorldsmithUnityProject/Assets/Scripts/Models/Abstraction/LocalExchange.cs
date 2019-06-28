﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalExchange : Exchange
{

   
    public Participant activeParticipant;
    public Participant passiveParticipant;

    public List<Resource > activeResources = new List<Resource >();
    public List<Resource > passiveResources = new List<Resource >();
 //   public Dictionary<Resource.Type, float> activeResources = new Dictionary<Resource.Type, float>();
 //   public Dictionary<Resource.Type, float> passiveResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> activeCommitted = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> passiveCommitted = new Dictionary<Resource.Type, float>();


    public LocalMarket localMarket;
    public List<Resource> shoppingList = new List<Resource>();
    public List<Resource> orderedShoppingList = new List<Resource>();

    public LocalExchange()
    {
        foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)
        {
            activeCommitted.Add(restype, 0);
            passiveCommitted.Add(restype, 0);
        }
    }


    public void CommitResource (bool isActive,  Resource resource)
    {
        if (resource.amount > 0)
        {
            if (isActive == true)
            {
                activeResources.Add(resource);
                if (activeCommitted.ContainsKey(resource.type))
                    activeCommitted[resource.type] += resource.amount;
                else
                    activeCommitted.Add(resource.type, resource.amount);
            }
            else
            {
                passiveResources.Add(resource);
                if (passiveCommitted.ContainsKey(resource.type))
                    passiveCommitted[resource.type] += resource.amount;
                else
                    passiveCommitted.Add(resource.type, resource.amount);
            }
        }
  
         

    } 
     
    public override void ResolveExchange()
    {
        //if (activeResources.Count == 0 && passiveResources.Count == 0)
        //{
        //    Debug.Log("removing " + exchangeName);
        //    ExchangeController.Instance.cycleLocalExchanges.Remove(this);
        //    ExchangeController.Instance.archivedLocalExchanges.Remove(this);

        //    foreach (LocalExchange locexch in ExchangeController.Instance.archivedLocalExchanges)
        //    {
        //        Debug.Log(exchangeName + " while list has " + locexch.exchangeName);
        //    }
        //} 
            foreach (Resource res in activeResources)
            {
                activeParticipant.linkedEcoBlock.resourcePortfolio[res.type].amount -= res.amount;
            activeParticipant.offeredResources[res.type] -= res.amount;
            //       Debug.Log("Active P " + activeParticipant.linkedEcoBlock.blockID + " pays " + res.amount + res.type);
            passiveParticipant.linkedEcoBlock.resourcePortfolio[res.type].amount += res.amount;
                passiveParticipant.wantedResources[res.type] -= res.amount;

            //        Debug.Log("Passive P " + passiveParticipant.linkedEcoBlock.blockID + " gets " + res.amount + res.type);

        }
        foreach (Resource res in passiveResources)
            {
                activeParticipant.linkedEcoBlock.resourcePortfolio[res.type].amount += res.amount;
            activeParticipant.wantedResources[res.type] -= res.amount;

            //      Debug.Log("Active P " + activeParticipant.linkedEcoBlock.blockID + " gets " + res.amount + res.type);
            passiveParticipant.linkedEcoBlock.resourcePortfolio[res.type].amount -= res.amount;
            passiveParticipant.offeredResources[res.type] -= res.amount;
          //     Debug.Log("Passive P " + passiveParticipant.linkedEcoBlock.blockID + " pays " + res.amount + res.type);

            }

        ExchangeController.Instance.cycleLocalExchanges.Add(this);
        ExchangeController.Instance.archivedLocalExchanges.Add(this);
        localMarket.localExchangesList.Add(this);


    }


}
