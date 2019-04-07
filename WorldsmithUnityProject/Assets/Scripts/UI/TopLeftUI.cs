using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopLeftUI : MonoBehaviour
{
    // Any text and UI functionality relating the Top Left panel

    public TextMeshProUGUI headerText;
    public TextMeshProUGUI selectedText; 


    public void RefreshUI()
    {  
        if (UIController.Instance.currentSection == UIController.Section.World)
        {
            headerText.text = "World";
            string mapselect = "Map: ";
            string locationselect = "Location: ";
            string charselect = "Character: ";
            if (MapController.Instance.inGreaterMapView)
                mapselect += "Greater";
            else if (MapController.Instance.activeSubMap != null)
                mapselect += MapController.Instance.activeSubMap.transform.name;
            else
                mapselect += "Default";
            if (LocationController.Instance.GetSelectedLocation() != null)
                locationselect += LocationController.Instance.GetSelectedLocation().elementID;
            if (CharacterController.Instance.GetSelectedCharacter() != null)
                charselect += CharacterController.Instance.GetSelectedCharacter().elementID;
            selectedText.text = mapselect + "\n" + locationselect + "\n" + charselect;
        }
        else if (UIController.Instance.currentSection == UIController.Section.Location)
        {
            if (LocationController.Instance.GetSelectedLocation() != null)
                headerText.text = LocationController.Instance.GetSelectedLocation().elementID;
            else
                headerText.text = "Location";
            if (LocationController.Instance.GetSelectedBuildingType() == Building.BuildingType.Unassigned)
                selectedText.text = "Building: ";
            else
                selectedText.text = "Building: " + LocationController.Instance.GetSelectedBuildingType();
        }
        else if (UIController.Instance.currentSection == UIController.Section.Character)
        {
            if (CharacterController.Instance.GetSelectedCharacter() != null)
                headerText.text = CharacterController.Instance.GetSelectedCharacter().elementID;
            else
                headerText.text = "Character";
            selectedText.text = "";
        }  
    }
}
