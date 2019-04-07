using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    // Links to all UI sub-scripts. Presets view and switches between sections. 

    public static UIController Instance { get; protected set; }

    public UITabsSwitcher tabsSwitcher;

    public enum Section { World, Location, Character} 

    public GameObject worldSection;
    public GameObject locationSection;
    public GameObject characterSection;

    public Toggle sectionToggle1;
    public Toggle sectionToggle2;
    public Toggle sectionToggle3;

    [HideInInspector]
    public Section currentSection; 

    public WorldUI worldUI; 
    public LocationUI locationUI; 
    public CharacterUI characterUI;  
    public TopLeftUI topLeftUI;
    public LeftUI leftUI; 
    public ExploreUI exploreUI; 
    public LookupUI lookopUI;

    public GameObject panelTopLeft;
    public GameObject panelLeft;
    public GameObject SavePanel;
    public GameObject ExplorePanel;

    public Button hideButton;
    public Button showButton; 

    public  Section initialSection =  Section.World;
    public int initialTopTab = 1;
    public int initialSubTab = 1;

    bool panelsHidden = false;

    void Awake()
    { 
        Instance = this;
    }
    private void Start()
    {  
        PresetView();
    }
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))        
        //    Debug.Log("X " + Input.mousePosition.x + " Y: " + Input.mousePosition.y); 
    }

    // Refreshes UI of all linked UI script that might need refreshing at general moments
    public void RefreshUI()
    {
        topLeftUI.RefreshUI();
        leftUI.RefreshUI(); 

        if (worldUI.savePanelActive == true)
            WorldSaver.Instance.RefreshSaveUI();
        if (worldUI.explorePanelActive == true)
            exploreUI.RefreshUI();
    } 

    public void ToggleWorldSection(bool checkmarked)
    { 
        worldSection.SetActive(checkmarked);
        if (checkmarked == true)
        {
            CameraController.Instance.SetWorldSectionView();
            currentSection = Section.World;
            tabsSwitcher.ToggleWorldUI();
            worldUI.OpenSection();
        }
        else        
            worldUI.CloseSection();        
        RefreshUI();  
    }

    public void ToggleLocationSection(bool checkmarked)
    {
        locationSection.SetActive(checkmarked);
        if (checkmarked == true)
        {
            CameraController.Instance.SetLocationSectionView();
            currentSection = Section.Location;
            tabsSwitcher.ToggleLocationUI();
            locationUI.OpenSection();
        }
        else
            locationUI.CloseSection(); 
        RefreshUI();
    }
    public void ToggleCharacterSection(bool checkmarked)
    {
        characterSection.SetActive(checkmarked);
        if (checkmarked == true)
        {
            CameraController.Instance.SetCharacterSectionView();
            currentSection = Section.Character;
            tabsSwitcher.ToggleCharacterUI();
            characterUI.OpenSection();
        }
        else
            characterUI.CloseSection(); 
        RefreshUI(); 
    } 

    // Use the public Initial this and that fields to offer specific sections and tabs on Play
    void PresetView()
    {
        ToggleWorldSection(false);
        ToggleLocationSection(false);
        ToggleCharacterSection(false); 
        if (initialSection == Section.World)
        {
            sectionToggle1.isOn = true;
            ToggleWorldSection(true);
        }
        else if (initialSection == Section.Location)
        {
            sectionToggle2.isOn = true;
            ToggleLocationSection(true);
        }
        else if (initialSection == Section.Character)
        {
            sectionToggle3.isOn = true;
            ToggleCharacterSection(true);
        } 
        if (initialTopTab == 1)
            tabsSwitcher.toggleTop1.isOn = true;
        else if (initialTopTab == 2)
            tabsSwitcher.toggleTop2.isOn = true;
        else if (initialTopTab == 1)
            tabsSwitcher.toggleTop2.isOn = true;
        if (initialSubTab == 1)
            tabsSwitcher.toggleBottom1.isOn = true;
        else if (initialSubTab == 2)
            tabsSwitcher.toggleBottom2.isOn = true;
        else if (initialSubTab == 3)
            tabsSwitcher.toggleBottom3.isOn = true;
    }
    public void HideLeftPanels()
    { 
        panelTopLeft.SetActive(false);
        panelLeft.SetActive(false);
        panelsHidden = true;
        hideButton.gameObject.SetActive(false);
        showButton.gameObject.SetActive(true);
    }
    public void ShowLeftPanels()
    {
        panelTopLeft.SetActive(true);
        panelLeft.SetActive(true);
        panelsHidden = false;
        showButton.gameObject.SetActive(false);
        hideButton.gameObject.SetActive(true);
    }

    public bool IsContentInteractionAllowed()
    {
        // Checks the click's position against current state of UI, primarily so that no LocationContainers get clicked behind UI Panels, for which I know no easier solution.

        // TODO: Either reposition the SavePanel, or make a clear visual indicating: no clicks allowed while Save Panel is open 
        if (worldUI.savePanelActive == true)
            return false;

        bool allowed = true;
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        // Left panel width is approx 17% of screen, height is 100%
        // Explore Panel width is from 18 % - 51% of screen width. . Height is approx 78%
        float leftWidth = Screen.width * 0.17f;
        float exploreStartWidth = Screen.width * 0.18f;
        float exploreEndWidth = Screen.width * 0.51f;
        float exploreEndHeight = Screen.height * 0.78f;

        if (panelsHidden == false)   // Left Panels     
            if (xPos < leftWidth)
                allowed = false;
        if (worldUI.explorePanelActive == true) // Explore Panel 
            if (xPos > exploreStartWidth && xPos < exploreEndWidth && yPos < exploreEndHeight)
                allowed = false;

        return allowed;
    }
}
