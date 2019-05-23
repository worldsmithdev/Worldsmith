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
        string line3 =       "\n";
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
        string line1 = "Population: " + population.blockID + "\n";
        string line2 = "People: " + population.amount + " / " + population.GetHomeLocation().GetTotalPopulation() + " Total\n";

        string line3 = "Owns - ";
        foreach (Resource.Type restype in population.resourcePortfolio.Keys)
            if (population.resourcePortfolio[restype].amount > 0)
                line3 += "" + restype + ": " + population.resourcePortfolio[restype].amount.ToString("F1") + ", ";
        line3 += "\n\n";

        bool generated = false;
        bool created = false;
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
            line4 += "Generated - ";
            line5 += "Levy Paid - ";
            foreach (Resource.Type restype in population.cycleGeneratedResources.Keys)
                if (population.cycleGeneratedResources[restype] > 0)
                {
                    line4 += "" + restype + ": " + population.cycleGeneratedResources[restype].ToString("F1") + ", ";
                    foreach (Resource res in population.cyclePaidLevyResources)
                        if (res.type == restype && res.amount > 0)
                            line5 += "" + restype + ": " + res.amount.ToString("F1") + ", ";
                }
        }
        else if (created == true)
        {
            line4 += "Created - ";
            line5 += "Levy Paid - ";
            foreach (Resource.Type restype in population.cycleCreatedResources.Keys)
                if (population.cycleCreatedResources[restype] > 0)
                {
                    line4 += "" + restype + ": " + population.cycleCreatedResources[restype].ToString("F1") + ", ";
                    foreach (Resource res in population.cyclePaidLevyResources)
                        if (res.type == restype && res.amount > 0)
                            line5 += "" + restype + ": " + res.amount.ToString("F1") + ", ";
                }
        }
        else
        {
            line4 += "None Generated";
            line5 += "None Created";
        }
        line4 += "\n";
        line5 += "\n\n";


        string line6 = "Food Consumption - Desired: " + population.cycleDesiredFoodConsumption.ToString("F1") + " Actual: " + population.cycleActualFoodConsumption.ToString("F1") + "\n";
        string line7 = "Successive Food Shortages: " + population.successiveFoodConsumptionShortage + " Avg Hunger Level: " + population.GetAverageFoodShortage().ToString("F0") + "%\n";
        string line8 = "Luxury Consumption - Desired: " + population.cycleDesiredLuxuryConsumption.ToString("F1") + " Actual: " + population.cycleActualLuxuryConsumption.ToString("F1") + "\n";
        string line9 = "Successive Luxury Shortages: " + population.successiveLuxuryConsumptionShortage + " Avg Deprivation Level: " + population.GetAverageLuxuryShortage().ToString("F0") + "%\n";

        exploreUI.overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9;
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
}
