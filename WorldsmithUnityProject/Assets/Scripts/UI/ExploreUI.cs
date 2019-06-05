using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using TMPro;

public class ExploreUI : MonoBehaviour
{
    // Any text and UI (and some additional) functionality relating the Explore Screen


    public enum ClickedTypes {None, Location, Character, Item, Creature, Ruler, Warband, Population, Territory, Building, Exchange, Market, Participant}

    public ClickedTypes clickedType;

    public ExploreTextSetter exploreTextSetter;
    
    public GameObject Content1;
    public GameObject Content2;
    public GameObject Content3; 

    int activeToggle = 1;

    // All 
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;

    // Overview
    public TextMeshProUGUI overviewDescriptionText;  
    public TextMeshProUGUI overviewClickedText;
    public TextMeshProUGUI overviewSchematicHeaderText;
    
    // Layout
    public TextMeshProUGUI layoutClickedText;
    public TextMeshProUGUI layoutSchematicHeaderText;
    public TextMeshProUGUI layoutTemplateText;

    // Exchange
    public TextMeshProUGUI exchangeClickedText;
    public TextMeshProUGUI exchangeSchematicHeaderText;


    void Start()
    {
        Content1.SetActive(true);
        Content2.SetActive(false);
        Content3.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            UIController.Instance.worldUI.ToggleExplorePanel();
        if (Input.GetMouseButtonDown(0))
            if (UIController.Instance.IsContentInteractionAllowed() == true && UIController.Instance.worldUI.explorePanelActive == true)
                if (ContainerController.Instance.hoveringLocationContainer == false)
                    UIController.Instance.worldUI.CloseExplorePanel();

        if (UIController.Instance.worldUI.explorePanelActive == true)
        {
            if (Input.GetKeyDown("up"))
                ShiftUp();
            if (Input.GetKeyDown("right"))
                ShiftRight();
            if (Input.GetKeyDown("down"))
                ShiftDown();
            if (Input.GetKeyDown("left"))
                ShiftLeft();
        }
    }

    public void SwitchLocation()
    {
        overviewClickedText.text = "";
        // Reset the Clicked Text, then check for loading an element/ecoblock into an equivalent to which was selected for the previous location
        Location selectedLoc = LocationController.Instance.GetSelectedLocation();   
        
        if (activeToggle == 1)
        {
            if (clickedType == ClickedTypes.Ruler && RulerController.Instance.GetSelectedRuler().isLocalRuler == true)
            {
                RulerController.Instance.SetSelectedRuler(selectedLoc.localRuler);
                exploreTextSetter.SetClickedRuler(RulerController.Instance.GetSelectedRuler());
            }
            else if (clickedType == ClickedTypes.Territory)
            {
                TerritoryController.Instance.SetSelectedTerritory(selectedLoc.locationTerritory);
                exploreTextSetter.SetClickedTerritory(TerritoryController.Instance.GetSelectedTerritory());
            }
            else if (clickedType == ClickedTypes.Population)
            {
                // Check if new Location has an equivalent population. If so, select it.
                Population selectedPop = PopulationController.Instance.GetSelectedPopulation();
                bool popMatchFound = false;
                foreach (Population pop in selectedLoc.populationList)
                {
                    if (pop.classType == selectedPop.classType && pop.laborType == selectedPop.laborType)
                    {
                        popMatchFound = true;
                        PopulationController.Instance.SetSelectedPopulation(pop);
                    }
                }
                if (popMatchFound == true)
                    exploreTextSetter.SetClickedPopulation(PopulationController.Instance.GetSelectedPopulation());
                else
                    clickedType = ClickedTypes.Location;
            }
            else
                clickedType = ClickedTypes.Location;
        } 
    }

    public void RefreshUI()
    {
        if (LocationController.Instance.GetSelectedLocation() != null)
        {
            Location selectedLoc = LocationController.Instance.GetSelectedLocation();

            //Clearing previous content
            TileMapController.Instance.DestroySchematicTileMaps();
            TileMapController.Instance.DestroyTemplateTileMaps();

            // General content
            nameText.text = selectedLoc.elementID;
            typeText.text = "" + VariableValueStore.Instance.factionAdjectiveValues[selectedLoc.currentFaction] + " " + selectedLoc.GetLocationSubType();
            overviewSchematicHeaderText.text = "Schematic";

            // Overview content
            overviewDescriptionText.text = selectedLoc.description;

            // Layout content
            layoutTemplateText.text = "";
            layoutClickedText.text = "";

            // Exchange content
            exchangeClickedText.text = "";

            if (activeToggle == 1)
            {
                if (clickedType == ClickedTypes.Location)
                    exploreTextSetter.SetClickedLocation(selectedLoc);
                else if (clickedType == ClickedTypes.Character)
                    exploreTextSetter.SetClickedCharacter(CharacterController.Instance.GetSelectedCharacter());
                else if (clickedType == ClickedTypes.Item)
                    exploreTextSetter.SetClickedItem(ItemController.Instance.GetSelectedItem());
                else if (clickedType == ClickedTypes.Creature)
                    exploreTextSetter.SetClickedCreature(CreatureController.Instance.GetSelectedCreature());
                else if (clickedType == ClickedTypes.Ruler)
                    exploreTextSetter.SetClickedRuler(RulerController.Instance.GetSelectedRuler());
                else if (clickedType == ClickedTypes.Warband)
                    exploreTextSetter.SetClickedWarband(WarbandController.Instance.GetSelectedWarband());
                else if (clickedType == ClickedTypes.Population)
                    exploreTextSetter.SetClickedPopulation(PopulationController.Instance.GetSelectedPopulation());
                else if (clickedType == ClickedTypes.Territory)
                    exploreTextSetter.SetClickedTerritory(TerritoryController.Instance.GetSelectedTerritory());
                else if (clickedType == ClickedTypes.Market)
                    exploreTextSetter.SetClickedMarketText( MarketController.Instance.GetSelectedMarket());
                else if (clickedType == ClickedTypes.Participant)
                    exploreTextSetter.SetClickedParticipantText(MarketController.Instance.GetSelectedParticipant());
            }
            else if (activeToggle == 2)
            {
                exploreTextSetter.SetDefaultLayoutContentText();
            }
            else if (activeToggle == 3)
            {
                exploreTextSetter.SetDefaultExchangeContentText();
            }

     

            RefreshExploreMaps();

            // Refreshing content specific to the selected location's LocationType
            //if (selectedLoc.GetLocationType() == Location.LocationType.Settled)
            //    RefreshSettled(selectedLoc);
            //else if (selectedLoc.GetLocationType() == Location.LocationType.Productive)
            //    RefreshProductive(selectedLoc);
            //else if (selectedLoc.GetLocationType() == Location.LocationType.Defensive)
            //    RefreshDefensive(selectedLoc);
            //else if (selectedLoc.GetLocationType() == Location.LocationType.Cultural)
            //    RefreshCultural(selectedLoc);
            //else if (selectedLoc.GetLocationType() == Location.LocationType.Natural)
            //    RefreshNatural(selectedLoc);
        }      
    } 
    //void RefreshSettled(Location selectedLoc)
    //{ 
    //}
    //void RefreshProductive(Location selectedLoc)
    //{ 
    //}
    //void RefreshDefensive(Location selectedLoc)
    //{ 
    //}
    //void RefreshCultural(Location selectedLoc)
    //{ 
    //}
    //void RefreshNatural(Location selectedLoc)
    //{ 
    //}
    void RefreshExploreMaps()
    {
        Location selectedLoc = LocationController.Instance.GetSelectedLocation();

        if (activeToggle == 1) // OVERVIEW
        {
            TileMapController.Instance.CreateTileMap("Elements", TileMap.TileMapCategory.Schematic);
            TileMapController.Instance.CreateTileMap("Rule", TileMap.TileMapCategory.Schematic);
        }
        else if (activeToggle == 2) // LAYOUT
        {
            if (TileMapController.Instance.HasSpecificTemplate(selectedLoc) == false && TileMapController.Instance.HasGeneralTemplate(selectedLoc) == false)
                layoutTemplateText.text = "No Template Found";
            else if (TileMapController.Instance.HasSpecificTemplate(selectedLoc) == true)
                layoutTemplateText.text = "Location-Specific Template";
            else if (TileMapController.Instance.HasGeneralTemplate(selectedLoc) == true)
                layoutTemplateText.text = "SubType Template";

            TileMapController.Instance.CreateTemplateTileMap(selectedLoc);
        }
        else if (activeToggle == 3) // EXCHANGE
        {
            TileMapController.Instance.CreateTileMap("Exchanges", TileMap.TileMapCategory.Schematic);
        }
    }
    public void RefreshClose()
    {
        TileMapController.Instance.DestroySchematicTileMaps();
        TileMapController.Instance.DestroyTemplateTileMaps();
    } 
    public void ToggleContent1(bool checkmarked)
    {
        if (checkmarked == true)
            activeToggle = 1;
        Content1.SetActive(checkmarked); 
        RefreshUI();
    }
    public void ToggleContent2(bool checkmarked)
    {
        if (checkmarked == true)
            activeToggle = 2;
        Content2.SetActive(checkmarked); 
        RefreshUI();
    }
    public void ToggleContent3(bool checkmarked)
    {
        if (checkmarked == true)
            activeToggle = 3;
        Content3.SetActive(checkmarked); 
        RefreshUI();
        exploreTextSetter.SetDefaultExchangeContentText();
    } 

    public void ShiftRight()
    {
        bool hasSelected = false;
        float absDistance;
        float closestDistance = 9999f;
        LocationContainer selectedCont = ContainerController.Instance.GetContainerFromLocation(LocationController.Instance.GetSelectedLocation());
        LocationContainer newSelectedCont = null;
        foreach (LocationContainer listedContainer in ContainerController.Instance.locationContainerList)
        {
            absDistance = Vector3.Distance(selectedCont.transform.localPosition, listedContainer.transform.localPosition);
            if (listedContainer.transform.localPosition.x > selectedCont.transform.localPosition.x && absDistance < closestDistance)
            {
                closestDistance = absDistance;
                newSelectedCont = listedContainer;
                hasSelected = true;
            }
        }

        if (hasSelected == true)
        { 
            ContainerController.Instance.ShiftSelectedContainer(newSelectedCont);
        }
    }

    public void ShiftLeft()
    {
        bool hasSelected = false;
        float absDistance;
        float closestDistance = 9999f;
        LocationContainer selectedCont = ContainerController.Instance.GetContainerFromLocation(LocationController.Instance.GetSelectedLocation());
        LocationContainer newSelectedCont = null;
        foreach (LocationContainer listedContainer in ContainerController.Instance.locationContainerList)
        {
            absDistance = Vector3.Distance(selectedCont.transform.localPosition, listedContainer.transform.localPosition);
            if (listedContainer.transform.localPosition.x < selectedCont.transform.localPosition.x && absDistance < closestDistance)
            {
                closestDistance = absDistance;
                newSelectedCont = listedContainer;
                hasSelected = true;
            }
        }

        if (hasSelected == true)
        { 
            ContainerController.Instance.ShiftSelectedContainer(newSelectedCont);
        }
    }
    public void ShiftDown()
    {
        bool hasSelected = false;
        float absDistance;
        float closestDistance = 9999f;
        LocationContainer selectedCont = ContainerController.Instance.GetContainerFromLocation(LocationController.Instance.GetSelectedLocation());
        LocationContainer newSelectedCont = null;
        foreach (LocationContainer listedContainer in ContainerController.Instance.locationContainerList)
        {
            absDistance = Vector3.Distance(selectedCont.transform.localPosition, listedContainer.transform.localPosition);
            if (listedContainer.transform.localPosition.y < selectedCont.transform.localPosition.y && absDistance < closestDistance)
            { 
                closestDistance = absDistance;
                newSelectedCont = listedContainer;
                hasSelected = true;
            }
        }

        if (hasSelected == true)
        { 
            ContainerController.Instance.ShiftSelectedContainer(newSelectedCont);
        }
    }
    public void ShiftUp()
    {
        bool hasSelected = false;
        float absDistance;
        float closestDistance = 9999f;
        LocationContainer selectedCont = ContainerController.Instance.GetContainerFromLocation(LocationController.Instance.GetSelectedLocation());
        LocationContainer newSelectedCont = null;
        foreach (LocationContainer listedContainer in ContainerController.Instance.locationContainerList)
        {
            absDistance = Vector3.Distance(selectedCont.transform.localPosition, listedContainer.transform.localPosition);
            if (listedContainer.transform.localPosition.y > selectedCont.transform.localPosition.y && absDistance < closestDistance)
            {
                closestDistance = absDistance;
                newSelectedCont = listedContainer;
                hasSelected = true;
            }
        }

        if (hasSelected == true)
        { 
            ContainerController.Instance.ShiftSelectedContainer(newSelectedCont);
        }
    }

}
