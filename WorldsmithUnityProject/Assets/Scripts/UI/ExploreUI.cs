using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using TMPro;

public class ExploreUI : MonoBehaviour
{
    // Any text and UI (and some additional) functionality relating the Explore panel

    public GameObject Content1;
    public GameObject Content2;
    public GameObject Content3; 

    int activeToggle = 1;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;

    public TextMeshProUGUI overviewDescriptionText; 
    public TextMeshProUGUI overviewSubText;
    public TextMeshProUGUI overviewClickedText;
    public TextMeshProUGUI overviewHoveredElementText; 
    public TextMeshProUGUI overviewHoveredRulerText; 
    public TextMeshProUGUI layoutTemplateText; 
    public TextMeshProUGUI layoutHoveredText; 
    public TextMeshProUGUI layoutClickedText;

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
            typeText.text = "" + selectedLoc.GetLocationType() + " - " + selectedLoc.GetLocationSubType();            

            // Overview content
            overviewDescriptionText.text = selectedLoc.description;
            overviewHoveredElementText.text = "World Elements";
            overviewHoveredRulerText.text = "Economy Blocks";
            overviewClickedText.text = "";
            RefreshExploreMaps();

            // Layout content
            layoutTemplateText.text = "";
            layoutHoveredText.text = "";
            layoutClickedText.text = "";

            // Refreshing content specific to the selected location's LocationType
            if (selectedLoc.GetLocationType() == Location.LocationType.Settled)
                RefreshSettled(selectedLoc);
            else if (selectedLoc.GetLocationType() == Location.LocationType.Productive)
                RefreshProductive(selectedLoc);
            else if (selectedLoc.GetLocationType() == Location.LocationType.Defensive)
                RefreshDefensive(selectedLoc);
            else if (selectedLoc.GetLocationType() == Location.LocationType.Cultural)
                RefreshCultural(selectedLoc);
            else if (selectedLoc.GetLocationType() == Location.LocationType.Natural)
                RefreshNatural(selectedLoc);
        }      
    } 
    void RefreshSettled(Location selectedLoc)
    {
        overviewSubText.text = "xLocation: " + selectedLoc.GetPositionVector().x.ToString("F2") + "\nyLocation: " + selectedLoc.GetPositionVector().y.ToString("F2");
    }
    void RefreshProductive(Location selectedLoc)
    {
        overviewSubText.text = "";
    }
    void RefreshDefensive(Location selectedLoc)
    {
        overviewSubText.text = "";
    }
    void RefreshCultural(Location selectedLoc)
    {
        overviewSubText.text = "";
    }
    void RefreshNatural(Location selectedLoc)
    {
        overviewSubText.text = "";
    }
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
                layoutTemplateText.text = "No Specific or General Template Found";
            else if (TileMapController.Instance.HasSpecificTemplate(selectedLoc) == true)
                layoutTemplateText.text = "Specific Template";
            else if (TileMapController.Instance.HasGeneralTemplate(selectedLoc) == true)
                layoutTemplateText.text = "General Template";

            TileMapController.Instance.CreateTemplateTileMap(selectedLoc);
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
            LocationController.Instance.SetSelectedLocation(newSelectedCont.GetContainedLocation());
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
            LocationController.Instance.SetSelectedLocation(newSelectedCont.GetContainedLocation());
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
            LocationController.Instance.SetSelectedLocation(newSelectedCont.GetContainedLocation());
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
            LocationController.Instance.SetSelectedLocation(newSelectedCont.GetContainedLocation());
            ContainerController.Instance.ShiftSelectedContainer(newSelectedCont);
        }
    }


    public void SetClickedCharacterText(Character character)
    {
        string line1 = "Character: " + character.elementID + "\n";
        string line2 = "Description: " + character.description + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }

    public void SetClickedItemText(Item item)
    {
        string line1 = "Item: " + item.elementID + "\n";
        string line2 = "Description: " + item.description + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedCreatureText(Creature creature)
    {
        string line1 = "Creature: " + creature.elementID + "\n";
        string line2 = "Description: " + creature.description + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedRuler (Ruler ruler)
    {
        string line1 = "Ruler: " + ruler.blockID + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedWarband (Warband warband)
    {
        string line1 = "Warband: " + warband.blockID + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedPopulation (Population population)
    {
        string line1 = "Population: " + population.blockID + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedTerritory (Territory territory)
    {
        string line1 = "Territory: " + territory.blockID + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        overviewClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
    public void SetClickedBuildingText(Building.BuildingType type)
    {
        string line1 = "Building Type: " + type + "\n";
        string line2 = "" + "\n";
        string line3 = "" + "\n";
        string line4 = "" + "\n";
        string line5 = "" + "\n";
        string line6 = "" + "\n";
        string line7 = "" + "\n";
        layoutClickedText.text = line1 + line2 + line3 + line4 + line5 + line6 + line7;
    }
}
