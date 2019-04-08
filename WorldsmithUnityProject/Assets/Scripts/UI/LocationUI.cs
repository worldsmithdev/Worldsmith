using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationUI : MonoBehaviour
{
    // Links UI components and functions relating to UI - specific to Location Section

    Location selectedLoc;
    public Image locationBackground;
    public Toggle backgroundToggle;
    public Toggle layoutToggle;
    public TextMeshProUGUI hoveredText;
    public TextMeshProUGUI clickedText;
    public GameObject landingContent;
    public TextMeshProUGUI buildingText;
    public GameObject buildingContentsHolder;
    public List<GameObject> buildingContents = new List<GameObject>();

    void Start()
    {
        foreach (Transform content in buildingContentsHolder.transform)
            buildingContents.Add(content.gameObject);
    }
    public void OpenSection()
    {
        layoutToggle.isOn = false;
        hoveredText.text = "";
        clickedText.text = "";
        landingContent.SetActive(true);
        foreach (GameObject obj in buildingContents)
            obj.SetActive(false);

        if (LocationController.Instance.GetSelectedLocation() != null)
        {
            selectedLoc = LocationController.Instance.GetSelectedLocation();
            if (TileMapController.Instance.HasLocationTileMap(selectedLoc) == true || TileMapController.Instance.HasTemplateTileMap(selectedLoc) == true)
                layoutToggle.interactable = true;
            else
                layoutToggle.interactable = false;

            string path = "Images/Locations/" + selectedLoc.elementID;
            Sprite locSprite = Resources.Load<Sprite>(path);
            if (locSprite != null)
            {
                if (backgroundToggle.isOn == true)
                {
                    backgroundToggle.interactable = true;
                    locationBackground.gameObject.SetActive(true);
                    locationBackground.sprite = locSprite;
                }
                else
                {
                    backgroundToggle.interactable = true;
                }
            }
            else
            {
                locationBackground.gameObject.SetActive(false);
                backgroundToggle.interactable = false;
                backgroundToggle.isOn = false;
            }
        }
        else
        {
            locationBackground.gameObject.SetActive(false);
            backgroundToggle.interactable = false;
            backgroundToggle.isOn = false;
        }
    }
    public void CloseSection()
    {
        TileMapController.Instance.DestroyTemplateTileMaps();
    }



    public void ToggleBackground(bool checkmarked)
    {
        if (checkmarked == true)
        {
            if (LocationController.Instance.GetSelectedLocation() != null)
            {
                selectedLoc = LocationController.Instance.GetSelectedLocation();
                string path = "Images/Locations/" + selectedLoc.elementID;
                Sprite locSprite = Resources.Load<Sprite>(path);
                if (locSprite != null)
                {
                    locationBackground.gameObject.SetActive(true);
                    locationBackground.sprite = locSprite;
                }
                else
                    locationBackground.gameObject.SetActive(false);
            }
        }
        else
            locationBackground.gameObject.SetActive(false);
    }
    public void ToggleLayout(bool checkmarked)
    {
        if (checkmarked == true)
        {
            hoveredText.text = "Hovered: ";
            if (LocationController.Instance.GetSelectedBuildingType() == Building.BuildingType.Unassigned)
                clickedText.text = "Clicked: ";
            else
                clickedText.text = "Clicked: " + LocationController.Instance.GetSelectedBuildingType();
            TileMapController.Instance.CreateTemplateTileMap(selectedLoc);
        }
        else
        {
            hoveredText.text = "";
            clickedText.text = "";
            TileMapController.Instance.DestroyTemplateTileMaps();
        }
    }
    public void ClickLayoutBuilding(Building.BuildingType type)
    {
        clickedText.text = "Clicked: " + type;
        LocationController.Instance.SetSelectedBuildingType(type);
    }

    public void OpenBuildingTab()
    {
        bool foundContentMatch = false;
        if (LocationController.Instance.GetSelectedBuildingType() == Building.BuildingType.Unassigned)
            buildingText.text = "No building type selected";
        else
            foreach (GameObject obj in buildingContents)
                if (obj != null)
                    if (obj.transform.name == LocationController.Instance.GetSelectedBuildingType().ToString())
                    {
                        foundContentMatch = true;
                        obj.SetActive(true);
                        buildingText.text = "Showing content for type: " + LocationController.Instance.GetSelectedBuildingType();
                    }

        if (foundContentMatch == false)
            buildingText.text = "No content holder found for type: " + LocationController.Instance.GetSelectedBuildingType();
    }


    public void ClearSectionContent()
    {
        landingContent.SetActive(false);
        foreach (GameObject obj in buildingContents)
            obj.SetActive(false);
        TileMapController.Instance.DestroyTemplateTileMaps();
    }
}
