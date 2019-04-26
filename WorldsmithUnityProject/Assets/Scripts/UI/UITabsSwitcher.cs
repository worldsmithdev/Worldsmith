using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITabsSwitcher : MonoBehaviour
{
    // Toggles texts for the 3 toggles on TopLeft and Left panels; (de)activates corresponding holders for Left panel; adds in some additional calls

    public GameObject topLeftWorld;
    public GameObject topLeftCity;
    public GameObject topLeftCharacter;
    public GameObject bottomLeftWorld;
    public GameObject bottomLeftCity;
    public GameObject bottomLeftCharacter; 

    public Toggle toggleTop1;
    public Toggle toggleTop2;
    public Toggle toggleTop3;
    public Toggle toggleBottom1;
    public Toggle toggleBottom2;
    public Toggle toggleBottom3;
    List<Toggle> bottomTogglesList = new List<Toggle>();
    List<Toggle> togglesList = new List<Toggle>();

    public GameObject world1;
    public GameObject world1sub1;
    public GameObject world1sub2;
    public GameObject world1sub3;
    public GameObject world2;
    public GameObject world2sub1;
    public GameObject world2sub2;
    public GameObject world2sub3;
    public GameObject world3;
    public GameObject world3sub1;
    public GameObject world3sub2;
    public GameObject world3sub3;
    public GameObject location1;
    public GameObject location1sub1;
    public GameObject location1sub2;
    public GameObject location1sub3;
    public GameObject location2;
    public GameObject location2sub1;
    public GameObject location2sub2;
    public GameObject location2sub3;
    public GameObject location3;
    public GameObject location3sub1;
    public GameObject location3sub2;
    public GameObject location3sub3;
    public GameObject character1;
    public GameObject character1sub1;
    public GameObject character1sub2;
    public GameObject character1sub3;
    public GameObject character2;
    public GameObject character2sub1;
    public GameObject character2sub2;
    public GameObject character2sub3;
    public GameObject character3;
    public GameObject character3sub1;
    public GameObject character3sub2;
    public GameObject character3sub3;
    public List<GameObject> bottomPanelsList;
    public List<GameObject> basePanelsList;

    string stringWorldTop1 = "Maps";
    string stringWorldTop2 = "Places";
    string stringWorldTop3 = "";
    string stringWorldBottom11 = "Image";
    string stringWorldBottom12 = "Data";
    string stringWorlddBottom13 = "Greater";
    string stringWorldBottom21 = "Sprites";
    string stringWorldBottom22 = "Highlights";
    string stringWorldBottom23 = "Lookup";
    string stringWorldBottom31 = "";
    string stringWorldBottom32 = "";
    string stringWorldBottom33 = "";
    string stringLocationTop1 = "Navigate";
    string stringLocationTop2 = "";
    string stringLocationTop3 = "";
    string stringLocationBottom11 = "Landing";
    string stringLocationBottom12 = "Building";
    string stringLocationBottom13 = ""; 
    string stringLocationBottom21 = "";
    string stringLocationBottom22 = "";
    string stringLocationBottom23 = "";
    string stringLocationBottom31 = "";
    string stringLocationBottom32 = "";
    string stringLocationBottom33 = "";
    string stringCharacterTop1 = "";
    string stringCharacterTop2 = "";
    string stringCharacterTop3 = "";
    string stringCharacterBottom11 = "";
    string stringCharacterBottom12 = "";
    string stringCharacterBottom13 = "";
    string stringCharacterBottom21 = "";
    string stringCharacterBottom22 = "";
    string stringCharacterBottom23 = "";
    string stringCharacterBottom31 = "";
    string stringCharacterBottom32 = "";
    string stringCharacterBottom33 = "";

    int currentTopToggle = 1;

    void Awake()
    {
        togglesList.Add(toggleTop1);
        togglesList.Add(toggleTop2);
        togglesList.Add(toggleTop3);
        togglesList.Add(toggleBottom1);
        togglesList.Add(toggleBottom2);
        togglesList.Add(toggleBottom3);
        bottomTogglesList.Add(toggleBottom1);
        bottomTogglesList.Add(toggleBottom2);
        bottomTogglesList.Add(toggleBottom3);
        DisableBasePanels();
        DisableBottomPanels();
    }

    public void ResetToggles()
    {
        toggleTop1.gameObject.SetActive(true);
        toggleTop2.gameObject.SetActive(true);
        toggleTop3.gameObject.SetActive(true);
        foreach (Toggle toggle in togglesList)
        {
            toggle.isOn = false;
        }
        toggleTop1.isOn = true;
        toggleBottom1.isOn = true;
    }
    public void ResetBottomToggles()
    {
        foreach (Toggle toggle in bottomTogglesList)
        {
            toggle.isOn = false;
        }
        toggleBottom1.isOn = true;
    }
    public void ToggleWorldUI()
    {
        topLeftWorld.SetActive(true);
        bottomLeftWorld.SetActive(true);
        topLeftCity.SetActive(false);
        bottomLeftCity.SetActive(false);
        topLeftCharacter.SetActive(false);
        bottomLeftCharacter.SetActive(false);
        ResetToggles();
        toggleTop1.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldTop1;
        toggleTop2.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldTop2;
        toggleTop3.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldTop3;

    }
    public void ToggleLocationUI()
    {
        topLeftWorld.SetActive(false);
        bottomLeftWorld.SetActive(false);
        topLeftCity.SetActive(true);
        bottomLeftCity.SetActive(true);
        topLeftCharacter.SetActive(false);
        bottomLeftCharacter.SetActive(false);
        ResetToggles();
        toggleTop1.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationTop1;
        toggleTop2.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationTop2;
        toggleTop3.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationTop3;
    }
    public void ToggleCharacterUI()
    {
        topLeftWorld.SetActive(false);
        bottomLeftWorld.SetActive(false);
        topLeftCity.SetActive(false);
        bottomLeftCity.SetActive(false);
        topLeftCharacter.SetActive(true);
        bottomLeftCharacter.SetActive(true);
        ResetToggles();
        toggleTop1.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterTop1;
        toggleTop2.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterTop2;
        toggleTop3.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterTop3;
    }

    public void ToggleTop1(bool checkmarked)
    {
        if (checkmarked == true)
        {
            UIController.Instance.RefreshUI();
            currentTopToggle = 1;
            DisableBasePanels();
            ResetBottomToggles();
            if (UIController.Instance.currentSection == UIController.Section.World)
            {
                world1.SetActive(true);
                world1sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom11;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom12;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringWorlddBottom13;
            }
            if (UIController.Instance.currentSection == UIController.Section.Location)
            {  
                UIController.Instance.locationUI.ClearSectionContent();
                location1.SetActive(true);
                location1sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom11;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom12;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom13; 
            }
            if (UIController.Instance.currentSection == UIController.Section.Character)
            {
                character1.SetActive(true);
                character1sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom11;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom12;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom13;
            }
        }

    }
    public void ToggleTop2(bool checkmarked)
    {
        if (checkmarked == true)
        { 
            UIController.Instance.RefreshUI();
            currentTopToggle = 2;
            DisableBasePanels();
            ResetBottomToggles();
            if (UIController.Instance.currentSection == UIController.Section.World)
            {
                world2.SetActive(true);
                world2sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom21;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom22;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom23;
            }
            if (UIController.Instance.currentSection == UIController.Section.Location)
            { 
                UIController.Instance.locationUI.ClearSectionContent();
                location2.SetActive(true);
                location2sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom21;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom22;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom23; 
            }
            if (UIController.Instance.currentSection == UIController.Section.Character)
            {
                character2.SetActive(true);
                character2sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom21;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom22;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom23;
            }
        }

    }
    public void ToggleTop3(bool checkmarked)
    {
        if (checkmarked == true)
        { 
            UIController.Instance.RefreshUI();
            currentTopToggle = 3;
            DisableBasePanels();
            ResetBottomToggles();
            if (UIController.Instance.currentSection == UIController.Section.World)
            {
                world3.SetActive(true);
                world3sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom31;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom32;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringWorldBottom33;
            }
            if (UIController.Instance.currentSection == UIController.Section.Location)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location3.SetActive(true);
                location3sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom31;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom32;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringLocationBottom33;
            }
            if (UIController.Instance.currentSection == UIController.Section.Character)
            {
                character3.SetActive(true);
                character3sub1.SetActive(true);
                toggleBottom1.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom31;
                toggleBottom2.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom32;
                toggleBottom3.GetComponentInChildren<TextMeshProUGUI>().text = stringCharacterBottom33;
            }
        }

    }


    public void ToggleBottom1(bool checkmarked)
    {
        if (checkmarked == true)
        {
            DisableBottomPanels();
            UIController.Instance.RefreshUI();
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 1)
            {
                world1sub1.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 2)
            {
                world2sub1.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 3)
            {
                world3sub1.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 1)
            { 
                UIController.Instance.locationUI.OpenSection();
                location1sub1.SetActive(true); 
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 2)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location2sub1.SetActive(true); 
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 3)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location3sub1.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 1)
            {
                character1sub1.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 2)
            {
                character2sub1.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 3)
            {
                character3sub1.SetActive(true);
            }
        }
    }
    public void ToggleBottom2(bool checkmarked)
    {
        if (checkmarked == true)
        {
            DisableBottomPanels();
            UIController.Instance.RefreshUI();
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 1)
            {
                world1sub2.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 2)
            {
                world2sub2.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 3)
            {
                world3sub2.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 1)
            {
                // Switch to building tab
                UIController.Instance.locationUI.ClearSectionContent();
                UIController.Instance.locationUI.OpenBuildingTab();
                location1sub2.SetActive(true); 
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 2)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location2sub2.SetActive(true); 
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 3)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location3sub2.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 1)
            {
                character1sub2.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 2)
            {
                character2sub2.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 3)
            {
                character3sub2.SetActive(true);
            }
        }

    }
    public void ToggleBottom3(bool checkmarked)
    {
        if (checkmarked == true)
        {
            DisableBottomPanels();
            UIController.Instance.RefreshUI();
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 1)
            {
                world1sub3.SetActive(true);
                // set to greate map mode
                MapController.Instance.EnableGreaterMapView();
            }
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 2)
            {
                world2sub3.SetActive(true);
                MapController.Instance.DisableGreaterMapView();
            }
            if (UIController.Instance.currentSection == UIController.Section.World && currentTopToggle == 3)
            {
                world3sub3.SetActive(true);
                MapController.Instance.DisableGreaterMapView();
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 1)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location1sub3.SetActive(true); 
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 2)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location2sub3.SetActive(true); 
            }
            if (UIController.Instance.currentSection == UIController.Section.Location && currentTopToggle == 3)
            {
                UIController.Instance.locationUI.ClearSectionContent();
                location3sub3.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 1)
            {
                character1sub3.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 2)
            {
                character2sub3.SetActive(true);
            }
            if (UIController.Instance.currentSection == UIController.Section.Character && currentTopToggle == 3)
            {
                character3sub3.SetActive(true);
            }
        }

    }
    void DisableBottomPanels()
    {
        MapController.Instance.DisableGreaterMapView();
        foreach (GameObject obj in bottomPanelsList)
        {
            obj.SetActive(false);
        }
    }
    void DisableBasePanels()
    {
        if (MapController.Instance != null)
            MapController.Instance.DisableGreaterMapView();

        foreach (GameObject obj in basePanelsList)
        {
            obj.SetActive(false);
        }
    }
}
