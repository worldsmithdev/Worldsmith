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
    public bool forceSilverOnRegionalExchange = true;
    public float regionalForcedSilverPercentage = 0.1f;

    public float exchangeRateFreeSupplier = 0.7f;
    public float exchangeRateFreeHub = 0.8f;
    public float exchangeRateForcedSupplier = 0.5f;
    public float exchangeRateForcedHub = 0.6f;

    public List<Resource.Type> globalAcceptedTypes = new List<Resource.Type>();
    public Dictionary<Resource.Type, float> globalImportPercentages = new Dictionary<Resource.Type, float>(); 

    void Awake()
    {
        Instance = this;
        globalAcceptedTypes.Add(Resource.Type.Wheat);
        globalImportPercentages.Add(Resource.Type.Wares, 0.2f);
        globalImportPercentages.Add(Resource.Type.Silver, 0.8f);
    }


    public LocalExchange CreateLocalExchange (LocalExchange.Type type, LocalMarket market, Participant activeParticipant, Participant passiveParticipant, List<Resource> shoppingList)
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
    public float GetExchangeRate (Ruler ruler)
    {
        Location loc = ruler.GetHomeLocation();
        float rate = 0f;
        if (ruler.rulerHierarchy == Ruler.Hierarchy.Reined)
        {
            if (loc.hubType == 1)
                rate = exchangeRateForcedSupplier;
            else
                rate = exchangeRateForcedHub;
        }
        else
        {
            if (loc.hubType == 1)
                rate = exchangeRateFreeSupplier;
            else
                rate = exchangeRateFreeHub;
        }
        if (rate == 0f)
            Debug.Log("Returning ExchangeRate of 0.0 for " + ruler.blockID);
        return rate;
    }
}
