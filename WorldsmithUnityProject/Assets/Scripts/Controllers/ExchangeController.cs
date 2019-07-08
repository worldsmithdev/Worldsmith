using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeController : MonoBehaviour
{
     public static ExchangeController Instance { get; protected set; }

    public List<LocalExchange> cycleLocalExchanges = new List<LocalExchange>();
    public List<LocalExchange> archivedLocalExchanges = new List<LocalExchange>();
    public List<RegionalExchange> cycleRegionalExchanges = new List<RegionalExchange>();
    public List<RegionalExchange> archivedRegionalExchanges = new List<RegionalExchange>();
    public List<GlobalExchange> cycleGlobalExchanges = new List<GlobalExchange>();
    public List<GlobalExchange> archivedGlobalExchanges = new List<GlobalExchange>();

    public ExchangeCreator exchangeCreator;

    Exchange selectedExchange;
    public bool forceSilverOnLocalExchange = false;

    public 
    void Awake()
    {
        Instance = this;
    }


    public LocalExchange CreateLocalExchange (LocalExchange.ExchangeType type, LocalMarket market, Participant activeParticipant, Participant passiveParticipant, List<Resource> shoppingList)
    {
     return   exchangeCreator.CreateLocalExchange(type, market, activeParticipant, passiveParticipant, shoppingList);  
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
