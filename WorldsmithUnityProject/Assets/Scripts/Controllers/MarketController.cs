using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketController : MonoBehaviour
{
    public static MarketController Instance { get; protected set; }



    public List<LocalMarket> cycleLocalMarkets = new List<LocalMarket>();
    public Dictionary<Location, List<LocalMarket>> archivedLocalMarkets = new Dictionary<Location, List<LocalMarket>>();
    public List<RegionalMarket> cycleRegionalMarkets = new List<RegionalMarket>();
    public Dictionary<Location, List<RegionalMarket>> archivedRegionalMarkets = new Dictionary<Location, List<RegionalMarket>>();
    public List<GlobalMarket> cycleGlobalMarkets = new List<GlobalMarket>();
    public Dictionary<Location, List<GlobalMarket>> archivedGlobalMarkets = new Dictionary<Location, List<GlobalMarket>>();

    LocalMarket selectedLocalMarket;
    RegionalMarket selectedRegionalMarket;
    GlobalMarket selectedGlobalMarket;
    Participant selectedParticipant;
      

    private void Awake()
    {
        Instance = this;
    }
    public void SetDictionaries()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
        {
            archivedLocalMarkets.Add(loc, new List<LocalMarket>());
            archivedRegionalMarkets.Add(loc, new List<RegionalMarket>());
            archivedGlobalMarkets.Add(loc, new List<GlobalMarket>());

        }
    }

    public void SetSelectedLocalMarket (LocalMarket market)
    {
        selectedLocalMarket = market;
    }
    public void SetSelectedRegionalMarket(RegionalMarket market)
    {
        selectedRegionalMarket = market;
    }
    public void SetSelectedGlobalMarket(GlobalMarket market)
    {
        selectedGlobalMarket = market;
    }
    public void SetSelectedParticipant (Participant participant)
    {
        selectedParticipant = participant;
    }
    public LocalMarket GetSelectedLocalMarket()
    {
        return selectedLocalMarket;
    }
    public RegionalMarket GetSelectedRegionalMarket()
    {
        return selectedRegionalMarket;
    }
    public GlobalMarket GetSelectedGlobalMarket()
    {
        return selectedGlobalMarket;
    }
    public Participant GetSelectedParticipant()
    {
        return selectedParticipant;
    }
    public Market GetLocalMarketForParticipant (Participant givenParticipant)
    {
        foreach (Location loc in archivedLocalMarkets.Keys)        
            foreach (Market market in archivedLocalMarkets[loc])
                if (market.participantList.Contains(givenParticipant))
                    return market; 
        
        Debug.Log("Did not find local market for participant: " + givenParticipant.participantName);
        return null;
    }
}
