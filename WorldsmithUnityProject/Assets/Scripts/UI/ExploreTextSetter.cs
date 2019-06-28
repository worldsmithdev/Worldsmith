using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreTextSetter : MonoBehaviour
{
    ExploreUI exploreUI; 
    
    void Start()
    {
        exploreUI = UIController.Instance.exploreUI;
    }

    public void SetClickedLocation(Location location)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Location;
        string line1 = "Location: " + location.elementID;
        string line2 = "";
        if (location.currentFaction == 5 && location.greekType == 1)
            line2 += " - Dorian Greek";
        else if (location.currentFaction == 5 && location.greekType == 2)
            line2 += " - Ionian Greek";
        line2 += "\n";
        string line3 = "Total Silver ";
        float total = 0f;
        foreach (Population population in location.localRuler.GetControlledPopulations())
        {
            total += population.resourcePortfolio[Resource.Type.Silver].amount;
      //      Debug.Log("adding silver: " + population.resourcePortfolio[Resource.Type.Silver].amount + " for " + population.blockID);
        }
        line3 += "" + total + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }

    public void SetClickedCharacter(Character character)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Character;
        string line1 = "Character: " + character.elementID + "\n";
        string line2 = "Description: " + character.description + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }

    public void SetClickedItem(Item item)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Item;
        string line1 = "Item: " + item.elementID + "\n";
        string line2 = "Description: " + item.description + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedCreature(Creature creature)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Creature;
        string line1 = "Creature: " + creature.elementID + "\n";
        string line2 = "Description: " + creature.description + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedRuler(Ruler ruler)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Ruler;
        string line1 = "Ruler: " + ruler.blockID + "\n";
        string line2 = "Authority - Type: " + ruler.authorityType + " Attitude: " + ruler.attitude + "\n\n";
        string line3 = "Owns - ";
        foreach (Resource.Type restype in ruler.resourcePortfolio.Keys)
            if (ruler.resourcePortfolio[restype].amount > 0)
                line3 += "" + restype + ": " + ruler.resourcePortfolio[restype].amount.ToString("F1") + ", ";
        line3 += "\n\n";

        string line4 = "Levied - ";
        foreach (Resource res in ruler.cycleLeviedResources)
            if (res.amount > 0)
                line4 += "" + res.type + ": " + res.amount.ToString("F1") + ", ";
        line4 += "\n\n";

        string line5 = "\n";
        string line6 = "\n";
        string line7 = "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedWarband(Warband warband)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Warband;
        string line1 = "Warband: " + warband.blockID + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedPopulation(Population population)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Population;
        string line1 = "" + population.blockID + "  " + population.amount + " / " + population.GetHomeLocation().GetTotalPopulation() + "\n"; 
        string line2 = "Owns ";
        foreach (Resource.Type restype in population.resourcePortfolio.Keys)
            if (population.resourcePortfolio[restype].amount > 0)
                line2 += "" + restype.ToString().Substring(0, 2).ToUpper() + "" + population.resourcePortfolio[restype].amount.ToString("F0") + " ";
        line2 += "\n";

        bool generated = false;
        bool created = false;
        string line3 = "";
        string line4 = "";
        string line5 = "";
        foreach (Resource.Type restype in population.cycleGeneratedResources.Keys)
            if (population.cycleGeneratedResources[restype] > 0)
                generated = true;
        foreach (Resource.Type restype in population.cycleCreatedResources.Keys)
            if (population.cycleCreatedResources[restype] > 0)
                created = true;
        if (generated == true)
        {
            line4 += "Generated ";
            line5 += "Levied ";
            foreach (Resource.Type restype in population.cycleGeneratedResources.Keys)
                if (population.cycleGeneratedResources[restype] > 0)
                {
                    line4 += "+" + restype.ToString().Substring(0,2).ToUpper() + "" + population.cycleGeneratedResources[restype].ToString("F0") + "  ";
                    foreach (Resource res in population.cyclePaidLevyResources)
                        if (res.type == restype && res.amount > 0)
                            line5 += "-" + restype.ToString().Substring(0, 2).ToUpper() + " " + res.amount.ToString("F0") +  "  ";
                }
        }
        else if (created == true)
        {
            line4 += "Created ";
            line5 += "Levied ";
            foreach (Resource.Type restype in population.cycleCreatedResources.Keys)
                if (population.cycleCreatedResources[restype] > 0)
                {
                    line4 += "+" + restype.ToString().Substring(0, 2).ToUpper() + "" + population.cycleCreatedResources[restype].ToString("F0") +"  ";
                    foreach (Resource res in population.cyclePaidLevyResources)
                        if (res.type == restype && res.amount > 0)
                            line5 += "-" + restype.ToString().Substring(0, 2).ToUpper() + "" + res.amount.ToString("F0") + "  ";
                }
        }
        else
        {
            line4 += "None Generated";
            line5 += "None Created";
        }
        line4 += "\n";
        line5 += "\n";


        string line6 = "Desired  WH" + population.cycleDesiredFoodConsumption.ToString("F0") + "  Actual  -WH" + population.cycleActualFoodConsumption.ToString("F0") + "\n";
        string line7 = "Shortages  WH "  + population.successiveFoodConsumptionShortage + "x  Hunger " + population.GetAverageFoodShortage().ToString("F0") + "%\n";
        string line8  = "Desired  WA" + population.cycleDesiredComfortConsumption.ToString("F0") + "  Actual  -WA" + population.cycleActualComfortConsumption.ToString("F0") + "\n";
        string line9 = "Shortages  WA" + population.successiveComfortConsumptionShortage + "x  Deprived  WA" + population.GetAverageLuxuryShortage().ToString("F0") + "%\n";


        string line10 = "Surplus  ";
        foreach (Resource.Type restype in population.cycleLocalSurplusResources.Keys)
            if (population.cycleLocalSurplusResources[restype] > 0)
                line10 += "" + restype.ToString().Substring(0, 2).ToUpper() + "" + population.cycleLocalSurplusResources[restype].ToString("F0") + " ";
        line10 += "\n";

        string line11 = "Wanted  ";
        foreach (Resource.Type restype in population.cycleLocalWantedResources.Keys)
            if (population.cycleLocalWantedResources[restype] > 0)
                line11 += "" + restype.ToString().Substring(0, 2).ToUpper() + "" + population.cycleLocalWantedResources[restype].ToString("F0") + " ";
        line11 += "\n";
        string line12 = "\n";

        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12;
    }
    public void SetClickedTerritory(Territory territory)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Territory;
        Location loc = TerritoryController.Instance.GetLocationForTerritory(territory);
        string line1 = "Territory: " + territory.blockID + "\n";

        string line2 = "Cycle Farmed Manpower: " + territory.cycleFarmedManpower + "\n";
        string line3 = "Cycle Natural Manpower: " + territory.cycleNaturalManpower + "\n";

        string line4 = "\n";
        string line5 = "\n";
        string line6 = "\n";
        string line7 = "\n";
        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }


    // LAYOUT ----------------------------------
    public void SetDefaultLayoutContentText()
    {
        Location selectedLoc = LocationController.Instance.GetSelectedLocation();
        string line1 = "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.layoutClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
        public void SetClickedBuildingText(Building.BuildingType type)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Building;
        string line1 = "Building Type: " + type + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.layoutClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }

    // EXCHANGE ----------------------------------

    public void SetDefaultExchangeContentText()
    {
        Location selectedLoc = LocationController.Instance.GetSelectedLocation();
        string line1 = "Local Market with "+ selectedLoc.localMarket.participantList.Count +" Participants\n";
        string offer1 = "";
        string wants1 = "";        
        foreach (Resource.Type restype in selectedLoc.localMarket.participantList[0].offeredResources.Keys )        
            if (selectedLoc.localMarket.participantList[0].offeredResources[restype] > 0)
                offer1 += "" + selectedLoc.localMarket.participantList[0].offeredResources[restype] + " " + restype + ", ";
        foreach (Resource.Type restype in selectedLoc.localMarket.participantList[0].wantedResources.Keys)
            if (selectedLoc.localMarket.participantList[0].wantedResources[restype] > 0)
                wants1 += "" + selectedLoc.localMarket.participantList[0].wantedResources[restype] + " " + restype + ", ";
        string line2 = "" + selectedLoc.localMarket.participantList[0].linkedEcoBlock.blockID + " offers: " + offer1 + " - wants: "+ wants1 + "\n";

        string offer2 = "";
        string wants2 = "";
        foreach (Resource.Type restype in selectedLoc.localMarket.participantList[1].offeredResources.Keys)
            if (selectedLoc.localMarket.participantList[1].offeredResources[restype] > 0)
                offer2 += "" + selectedLoc.localMarket.participantList[1].offeredResources[restype] + " " + restype + ", ";
        foreach (Resource.Type restype in selectedLoc.localMarket.participantList[1].wantedResources.Keys)
            if (selectedLoc.localMarket.participantList[1].wantedResources[restype] > 0)
                wants2 += "" + selectedLoc.localMarket.participantList[1].wantedResources[restype] + " " + restype + ", ";
        string line3 = "" + selectedLoc.localMarket.participantList[1].linkedEcoBlock.blockID + " offers: " + offer2 + " - wants: " + wants2 + "\n";

        string offer3 = "";
        string wants3 = "";
        foreach (Resource.Type restype in selectedLoc.localMarket.participantList[2].offeredResources.Keys)
            if (selectedLoc.localMarket.participantList[2].offeredResources[restype] > 0)
                offer3 += "" + selectedLoc.localMarket.participantList[2].offeredResources[restype] + " " + restype + ", ";
        foreach (Resource.Type restype in selectedLoc.localMarket.participantList[2].wantedResources.Keys)
            if (selectedLoc.localMarket.participantList[2].wantedResources[restype] > 0)
                wants3 += "" + selectedLoc.localMarket.participantList[2].wantedResources[restype] + " " + restype + ", ";
        string line4 = "" + selectedLoc.localMarket.participantList[2].linkedEcoBlock.blockID + " offers: " + offer3 + " - wants: " + wants3 + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.exchangeClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedExchangeText(Building.BuildingType type)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Exchange;
        string line1 = "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        exploreUI.exchangeClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedMarketText(LocalMarket locmarket)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Market;
        string line1 = "" + locmarket.marketName + " Participants: " + locmarket.participantList.Count +  "\n";
        string line2 = "total offered: ";
        foreach (Resource.Type restype in locmarket.totalOfferedResources.Keys)
            line2 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + locmarket.totalOfferedResources[restype].ToString("F0") + " ";
        line2 += "\n";
        string line3 = "total wanted: ";
        foreach (Resource.Type restype in locmarket.totalWantedResources.Keys)
            line3 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + locmarket.totalWantedResources[restype].ToString("F0") + " ";
        line3 += "\n";

        //string line4 = "total claimed: ";
        //float totalSilverClaimed = 0f;
        //foreach (Resource.Type restype in locmarket.totalClaimedResources.Keys)
        //{
        //    totalSilverClaimed += Converter.GetSilverEquivalent(new Resource(restype, locmarket.totalClaimedResources[restype]));
        //    line4 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + locmarket.totalClaimedResources[restype].ToString("F0") + " ";
        //}
        //line4 +=  " silver value: " + totalSilverClaimed + "\n";
        //string line5 = "total deducted: " ;
        //float totalSilverDeducted = 0f;
        //foreach (Resource.Type restype in locmarket.totalDeductedResources.Keys)
        //{
        //    totalSilverDeducted += Converter.GetSilverEquivalent(new Resource(restype, locmarket.totalDeductedResources[restype]));
        //    line5 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + locmarket.totalDeductedResources[restype].ToString("F0") + " ";
        //} 
        //line5 += " silver value: " + totalSilverDeducted + "\n";
        //string line6 = "" + "\n";


        //string line7 = "sold percentage: ";
        //foreach (Resource.Type restype in locmarket.resourceSoldPercentages.Keys)
        //    line7 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + locmarket.resourceSoldPercentages[restype].ToString("F0") + " ";
        //line7 += "\n"; 
        exploreUI.exchangeClickedText.text = line1 + line2 + line3  ;
    }
    public void SetClickedParticipantText(Participant participant)
    {
        exploreUI.clickedType = ExploreUI.ClickedTypes.Participant;
        string line1 = "" + participant.participantName + "  type: "  + participant.type +" prio: " + participant.priorityLevel + "\n";
        string line2 = "Linked EB: " + participant.linkedEcoBlock.blockID  + " (Current) Spending power: " + participant.storedSpendingPower.ToString("F1") + "\n";

        string line3 = "offered: ";
        foreach (Resource.Type restype in participant.initialOfferedResources.Keys)
            line3 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + participant.initialOfferedResources[restype].ToString("F1") + " ";
        line3 += "\n";

        string line4 = "wanted: ";
        foreach (Resource.Type restype in participant.initialWantedResources.Keys)
            line4 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + participant.initialWantedResources[restype].ToString("F1") + " ";
        line4 += "\n";

        string line5 = "shopped: ";
        foreach (Resource.Type restype  in participant.totalShoppingList.Keys)
            if (participant.totalShoppingList[restype] > 0)
              line5 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + participant.totalShoppingList[restype].ToString("F1") + " ";
        line5 += "\n";


        string line6 = "deducted: ";
        //foreach (Resource.Type restype in participant.deductedResources.Keys)
        //    line6 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + participant.deductedResources[restype].ToString("F1") + " ";
        //line6 += "\n";

        string line7 = "Creditstatus: " + participant.silverCreditStatus + "  silver received: " + participant.receivedSilver + "  silver paid: " + participant.paidSilver + "\n";
        //string line8 = "spending power: " + participant.GetSpendingPower();

        //string line6 = "out traded: ";
        //foreach (Resource.Type restype in participant.outTradedResources.Keys)
        //    line6 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + participant.outTradedResources[restype].ToString("F0") + " ";
        //line6 += "\n";

        //string line7 = "in traded: ";
        //foreach (Resource.Type restype in participant.inTradedResources.Keys)
        //    line7 += " " + restype.ToString().Substring(0, 2).ToUpper() + "" + participant.inTradedResources[restype].ToString("F0") + " ";
        //line7 += "\n";


        //string line8 = "purchased: ";
        //foreach (Resource res in participant.purchasedResources)
        //    line8 += " " + res.type.ToString().Substring(0, 2).ToUpper() + "" + res.amount.ToString("F0") + " ";
        //line8 += "\n";

         
        exploreUI.exchangeClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }

    public void SetClickedExchangeText(Exchange exchange)
    {
        LocalExchange locExchange = (LocalExchange)exchange;
        exploreUI.clickedType = ExploreUI.ClickedTypes.Exchange;

     

        string line1 = "Active Resources: ";
        foreach (Resource res in locExchange.activeResources)
            line1 += " " + res.type.ToString().Substring(0, 2).ToUpper() + "" + res.amount.ToString("F1") + " ";
        line1 += "\n";

        string line2 = "Passive Resources: ";
        foreach (Resource res in locExchange.passiveResources)
            line2 += " " + res.type.ToString().Substring(0, 2).ToUpper() + "" + res.amount.ToString("F1") + " ";
        line2 += "\n";

        string line3 = "Active Participant: " + locExchange.activeParticipant.participantName + "\n";
        line3 += "Passive Participant: " + locExchange.passiveParticipant.participantName + "\n";
         

        exploreUI.exchangeClickedText.text = line1 + line2 + line3;

    }

}
