﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeController : MonoBehaviour
{
     public static ExchangeController Instance { get; protected set; }

    public List<LocalExchange> cycleLocalExchanges = new List<LocalExchange>();
    public List<LocalExchange> archivedLocalExchanges = new List<LocalExchange>();

    public ExchangeCreator exchangeCreator;

    Exchange selectedExchange;
    public bool forceSilverOnLocalExchange = false;

    public 
    void Awake()
    {
        Instance = this;
    }


    public LocalExchange CreateLocalExchange (LocalMarket market, Participant activeParticipant, Participant passiveParticipant, List<Resource> shoppingList)
    {
     return   exchangeCreator.CreateLocalExchange(market, activeParticipant, passiveParticipant, shoppingList);  
    }


    public void SetSelectedExchange (Exchange exch)
    {
        selectedExchange = exch;
    }
    public Exchange GetSelectedExchange()
    {
        return selectedExchange;
    }
}
