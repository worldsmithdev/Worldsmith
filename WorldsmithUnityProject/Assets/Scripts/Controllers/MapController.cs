using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapController : MonoBehaviour
{
    // Handles background maps: initial loading & switching between sub maps and the greater map

    public static MapController Instance { get; protected set; }


    bool initialized = false;

    public GameObject greaterMap;

    public GameObject mapsHolder;
    [HideInInspector]
    public List<GameObject> mapHolderList = new List<GameObject>();
    [HideInInspector]
    public List<SubMap> subMapList = new List<SubMap>();
    public List<Clickable> mapClickablesList = new List<Clickable>();

    public SubMap initialSubMap;

    //  [HideInInspector]
    public SubMap activeSubMap;
    [HideInInspector]
    public bool inGreaterMapView = false;


    // Checking these UI panels for the background toggles as an ugly workaround for a Unity 2019 bug:
    // https://issuetracker.unity3d.com/issues/re-enabling-game-object-with-toggle-group-loses-information-about-previously-checked-toggle 

    public GameObject backgroundToggleParentSub;
     
    public GameObject backgroundToggleParentTop;

    private void Awake()
    {
        Instance = this;
        activeSubMap = initialSubMap;
        greaterMap.SetActive(false);
        LoadMapHoldersIntoList();
        LoadSubMapsIntoList();         

    }
    private void Start()
    {
        SyncMapClickables();
        ActivateInitialBackgroundMap();
        SetSubMapsSprite1();
        initialized = true;
    }

    // Add each of the Background WorldMap gameobjects into a List.
    void LoadMapHoldersIntoList()
    {
        foreach (Transform holder in mapsHolder.transform)
            mapHolderList.Add(holder.gameObject);
    }
    // Add each of the SubMAp gameobjects into a List.
    void LoadSubMapsIntoList()
    {
        foreach (Transform holder in greaterMap.transform)
            subMapList.Add(holder.gameObject.GetComponent<SubMap>());
    }
    //Load the Background Worldmap's default image as its SubMap's Sprite1
    void SetSubMapsSprite1()
    {
        foreach (SubMap subMap in subMapList)        
            foreach (GameObject mapobj in mapHolderList)            
                if (mapobj.name == subMap.name)                
                    subMap.fullMap1Sprite = mapobj.GetComponent<SpriteRenderer>().sprite;
    }
    // Link the clickable text-fields in the LeftPanel's Greater section with the same-named SubMaps
    void SyncMapClickables()
    {
        foreach (GameObject mapobj in mapHolderList)
            if (mapobj != null)
                foreach (Clickable clickable in mapClickablesList)
                    if (clickable.activated == false)
                    {
                        clickable.gameObject.SetActive(true);
                        clickable.clickableType = Clickable.ClickableType.SubMapField;
                        clickable.transform.name = mapobj.transform.name;
                        clickable.text.text = mapobj.transform.name;
                        clickable.mainSpriteObject.SetActive(false);
                        clickable.triggerSpriteObject.GetComponent<Image>().sprite = SpriteCollection.Instance.clickableBackgroundSprite;
                        clickable.activated = true;
                        break;
                    }
    }
    // Find the Background WorldMap paired with the SubMap that is in the initial submap field
    void ActivateInitialBackgroundMap()
    {
        foreach (GameObject mapobj in mapHolderList)
            if (mapobj != null)
            {
                mapobj.SetActive(false);
                if (mapobj.transform.name == initialSubMap.transform.name)
                    mapobj.SetActive(true);
            }
    }
    // Called when switching to Greater in LeftPanel
    public void EnableGreaterMapView()
    {
        inGreaterMapView = true;
        greaterMap.SetActive(true);
        mapsHolder.SetActive(false);
        activeSubMap = null;
        foreach (GameObject mapobj in mapHolderList)
            if (mapobj != null)
                mapobj.SetActive(false);
        CameraController.Instance.SetWorldSectionView();
        UIController.Instance.worldUI.ClosePanels();
        UIController.Instance.RefreshUI();
    }
    public void DisableGreaterMapView()
    {
        if (inGreaterMapView == true)
        {
            inGreaterMapView = false;
            greaterMap.SetActive(false);
            mapsHolder.SetActive(true);
            UIController.Instance.RefreshUI();
            if (activeSubMap == null)
                activeSubMap = initialSubMap;
            foreach (GameObject mapobj in mapHolderList)
                if (mapobj != null)
                    if (mapobj.transform.name == activeSubMap.transform.name)
                        mapobj.SetActive(true);
        }
    }

    public void HoverClickable(string name)
    {
        foreach (Clickable clickable in mapClickablesList)
            if (clickable.transform.name == name)
                clickable.triggerSpriteObject.SetActive(true);

        foreach (SubMap subMap in subMapList)
            if (subMap.transform.name == name)
                subMap.highlightObject.SetActive(true);
    }
    public void ExitHoverClickable()
    {
        foreach (Clickable clickable in mapClickablesList)
            clickable.triggerSpriteObject.SetActive(false);

        foreach (SubMap subMap in subMapList)
            subMap.highlightObject.SetActive(false);
    }
    public void ClickMapClickable(string name)
    {
        foreach (Clickable clickable in mapClickablesList)
            clickable.triggerSpriteObject.SetActive(false);
        foreach (SubMap subMap in subMapList)
            if (subMap.transform.name == name)
                ClickSubMap(subMap);
    }

    // Activating a SubMap, either by clicking it or its paired Clickable in LeftPanel, will open that map's background
    public void ClickSubMap(SubMap submap)
    {
        bool foundMatch = false;
        foreach (GameObject mapobj in mapHolderList)
        {
            if (mapobj != null)
                if (mapobj.transform.name == submap.transform.name)
                {
                    submap.highlightObject.SetActive(false);
                    UIController.Instance.leftUI.DestroyTerrainDataTileMap();
                    foundMatch = true;
                    activeSubMap = submap;
                    mapobj.SetActive(true);
                    DisableGreaterMapView();
                    CameraController.Instance.SetWorldSectionView();
                    UIController.Instance.tabsSwitcher.toggleBottom1.isOn = true;
                    UIController.Instance.RefreshUI();
                    foreach (Clickable clickable in mapClickablesList)
                        clickable.triggerSpriteObject.SetActive(false);
                }
        }
        if (foundMatch == false)
            Debug.Log("Tried to open submap but did not find gameobject match: " + submap.transform.name);
    }




    public GameObject GetActiveMapHolder()
    {
        foreach (GameObject mapobj in mapHolderList)
            if (mapobj != null)
                if (mapobj.transform.name == activeSubMap.transform.name)
                    return mapobj;

        Debug.Log("Trying to get active map holder - but no match for submap: " + activeSubMap.transform.name);
        return null;
    }


    // Called from Maps - Background (World 1 Sub 1)
    // Extravagant check for each as temp fix for Unity bug described at start of this class
    public void ToggleBackgroundNone(bool checkmarked)
    {
        if (checkmarked == true && initialized == true && backgroundToggleParentTop.activeSelf == true && UIController.Instance.tabsSwitcher.bottomLeftWorld.activeSelf == true && backgroundToggleParentSub.activeSelf == true)
        { 
            foreach (GameObject mapobj in mapHolderList)
                if (mapobj != null)
                    if (mapobj.transform.name == activeSubMap.transform.name)
                        mapobj.GetComponent<SpriteRenderer>().sprite = null;
        }

    }
    public void ToggleBackground1(bool checkmarked)
    { 
        if (checkmarked == true && initialized == true && backgroundToggleParentTop.activeSelf == true && UIController.Instance.tabsSwitcher.bottomLeftWorld.activeSelf == true && backgroundToggleParentSub.activeSelf == true)
        { 
            foreach (GameObject mapobj in mapHolderList)
                if (mapobj != null)
                    if (mapobj.transform.name == activeSubMap.transform.name)
                        mapobj.GetComponent<SpriteRenderer>().sprite = activeSubMap.fullMap1Sprite;
        }

    }
    public void ToggleBackground2(bool checkmarked)
    {
        
        if (checkmarked == true && initialized == true && backgroundToggleParentTop.activeSelf == true && UIController.Instance.tabsSwitcher.bottomLeftWorld.activeSelf == true && backgroundToggleParentSub.activeSelf == true)
        { 
            foreach (GameObject mapobj in mapHolderList)
                if (mapobj != null)
                    if (mapobj.transform.name == activeSubMap.transform.name)
                        mapobj.GetComponent<SpriteRenderer>().sprite = activeSubMap.fullMap2Sprite;
        }

    }
    public void ToggleBackground3(bool checkmarked)
    { 
        if (checkmarked == true && initialized == true && backgroundToggleParentTop.activeSelf == true && UIController.Instance.tabsSwitcher.bottomLeftWorld.activeSelf == true && backgroundToggleParentSub.activeSelf == true)
        { 
            foreach (GameObject mapobj in mapHolderList)
                if (mapobj != null)
                    if (mapobj.transform.name == activeSubMap.transform.name)
                        mapobj.GetComponent<SpriteRenderer>().sprite = activeSubMap.fullMap3Sprite;
        }
    }

}
