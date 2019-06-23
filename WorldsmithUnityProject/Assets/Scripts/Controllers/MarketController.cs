using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketController : MonoBehaviour
{
    public static MarketController Instance { get; protected set; }



    public List<LocalMarket> cycleLocalMarkets = new List<LocalMarket>();
    public Dictionary<Location, List<LocalMarket>> archivedLocalMarkets = new Dictionary<Location, List<LocalMarket>>(); 
    public List<LocalExchange> cycleLocalExchanges = new List<LocalExchange>();
    public List<LocalExchange> archivedLocalExchanges = new List<LocalExchange>();

    LocalMarket selectedLocalMarket;
    Participant selectedParticipant;
      

    private void Awake()
    {
        Instance = this;
    }
    public void SetDictionaries()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)        
            archivedLocalMarkets.Add(loc, new List<LocalMarket>()); 
    }

    public void SetSelectedLocalMarket (LocalMarket market)
    {
        selectedLocalMarket = market;
    }
    public void SetSelectedParticipant (Participant participant)
    {
        selectedParticipant = participant;
    }
    public LocalMarket GetSelectedMarket()
    {
        return selectedLocalMarket;
    }
    public Participant GetSelectedParticipant()
    {
        return selectedParticipant;
    }
    public Market GetMarketForParticipant (Participant givenParticipant)
    {
        foreach (Location loc in archivedLocalMarkets.Keys)        
            foreach (Market market in archivedLocalMarkets[loc])
                if (market.participantList.Contains(givenParticipant))
                    return market;
        
 
        
        Debug.Log("Did not find market for participant: " + givenParticipant.participantName);
        return null;
    }
}
