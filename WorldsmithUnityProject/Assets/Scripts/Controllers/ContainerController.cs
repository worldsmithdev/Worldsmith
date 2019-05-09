using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContainerController : MonoBehaviour
{
    // Handles loading and setting for Location Containers, and functionality relating to visual toggling of the containers

    public static ContainerController Instance { get; protected set; }

    public Slider scaleFullSlider;
    public Slider scaleSpritesSlider;
    public Slider scaleHighlightsSlider;
    public GameObject linePrefab;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipTitle;
    public TextMeshProUGUI tooltipText;

    float centralSpriteScale;
    float highlightSpriteScale;
    float selectionSpriteScale;
    float subSpriteScale;
    float lineStartSize = 0.08f;
    float lineEndSize = 0.03f;
    List<GameObject> lineList = new List<GameObject>(); 

    LocationContainer hoveredContainer;
    LocationContainer selectedContainer;

    [HideInInspector]
    public List<LocationContainer> locationContainerList = new List<LocationContainer>();

    [HideInInspector]
    public bool hoveringLocationContainer = false;
    bool showTooltipOnHover = false;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadContainersIntoList();
    }

    // Reads content from all Background WorldMap objects, so you don't have to manually add LCs to a list.
    public void LoadContainersIntoList()
    {
        foreach (GameObject mapobj in MapController.Instance.mapHolderList) 
            foreach (Transform categoryholder in mapobj.transform) 
                foreach (Transform lc in categoryholder.gameObject.transform) 
                    locationContainerList.Add(lc.gameObject.GetComponent<LocationContainer>()); 
    }

    // Loads initial scale of LC's child objects to a value that is used when scaling LCs with the scale sliders.
    void SetScalePresets(LocationContainer container)
    {
        centralSpriteScale = container.centralSprite.gameObject.transform.localScale.x;
        highlightSpriteScale = container.highlightSprite.gameObject.transform.localScale.x;
        selectionSpriteScale = container.selectionSprite.gameObject.transform.localScale.x;
        subSpriteScale = container.subIconSprite.gameObject.transform.localScale.x;
    }

    // Called once to link each LC with respective imported Location.
    public void MatchLocationsToContainers()
    {
        // Call this once to link LC's with their imported Location
        foreach (LocationContainer container in locationContainerList)
            if (container != null)
                container.SetContainedLocation();

        // Call this once to set their initial sprite
        foreach (LocationContainer container in locationContainerList)
            if (container != null)
                container.SetContainerSprite();

        // Check the first container to learn its scale values. And for the slider to match its scale for its initial value.
        foreach (LocationContainer container in locationContainerList)
            if (container != null)
            {
                SetScalePresets(container);
                scaleFullSlider.value = container.gameObject.transform.localScale.x;
                break;
            }

    }
   
    // Called from LC's box collider on enter and exit
    public void SetHoveredContainer(LocationContainer cont)
    {
        hoveredContainer = cont;
        if (cont == null) // exit hover
        {
            hoveringLocationContainer = false;
            HideTooltip();
        }
        else
        {
            hoveringLocationContainer = true;
            if (showTooltipOnHover == true)
                ShowTooltip(cont);
        }  
    }

    public void ClickContainer(LocationContainer cont)
    {
        if (UIController.Instance.IsContentInteractionAllowed() == true)
        {
            if (cont == selectedContainer && UIController.Instance.worldUI.explorePanelActive == true)
            {
                UIController.Instance.worldUI.CloseExplorePanel(); 
            }
            else
            {
                if (selectedContainer != null)
                    selectedContainer.RemoveSelectHighlight();
                selectedContainer = cont;
                selectedContainer.SelectHighlight();
                LocationController.Instance.SetSelectedLocation(selectedContainer.GetContainedLocation());
                UIController.Instance.worldUI.OpenExplorePanel();
            } 
        } 
    }
    
    // Selecting the nearest LC in an N,E,S or W direction from the ExploreScreen
    public void ShiftSelectedContainer(LocationContainer cont)
    { 
            if (selectedContainer != null)
                selectedContainer.RemoveSelectHighlight();
            selectedContainer = cont;
            selectedContainer.SelectHighlight();
            LocationController.Instance.SetSelectedLocation(selectedContainer.GetContainedLocation());
            UIController.Instance.RefreshUI(); 
    }

    //To enable or disable tooltips
    public void ToggleTooltips(bool checkmarked)
    {
        showTooltipOnHover = checkmarked;
    }
    void ShowTooltip(LocationContainer cont)
    {
        tooltipPanel.transform.localPosition = new Vector3(cont.gameObject.transform.position.x + 5.5f, cont.gameObject.transform.position.y - 1.6f, 0);
        tooltipPanel.SetActive(true);
        tooltipTitle.text = cont.GetContainedLocation().elementID;
        tooltipText.text = cont.GetContainedLocation().description;
    }
    void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    public LocationContainer GetSelectedContainer()
    {
        return selectedContainer;
    }
    public LocationContainer GetContainerFromLocation(Location loc)
    {
        foreach (LocationContainer container in locationContainerList)
            if (container.GetXCoord() == loc.GetPositionVector().x && container.GetYCoord() == loc.GetPositionVector().y)
                return container;
        Debug.Log("Could not find container for location: " + loc.elementID);
        return null;
    }

    // Scale LC GameObject with children, called from the full scale slider
    public void ScaleLocationContainersFull()
    {
        foreach (LocationContainer container in locationContainerList)
            container.gameObject.transform.localScale = new Vector3(scaleFullSlider.value * 1.0f, scaleFullSlider.value * 1.0f, 1);
    }

    // Scale only the icon objects, called from the icon scale slider
    public void ScaleLocationContainersSprites()
    {
        foreach (LocationContainer container in locationContainerList)
        {
            container.centralSprite.gameObject.transform.localScale = new Vector3(scaleSpritesSlider.value * centralSpriteScale, scaleSpritesSlider.value * centralSpriteScale, 1);
            container.highlightSprite.gameObject.transform.localScale = new Vector3(scaleSpritesSlider.value * highlightSpriteScale, scaleSpritesSlider.value * highlightSpriteScale, 1);
            container.selectionSprite.gameObject.transform.localScale = new Vector3(scaleSpritesSlider.value * selectionSpriteScale, scaleSpritesSlider.value * selectionSpriteScale, 1);
            container.subIconSprite.gameObject.transform.localScale = new Vector3(scaleSpritesSlider.value * subSpriteScale, scaleSpritesSlider.value * subSpriteScale, 1);
            container.subIconSprite.gameObject.transform.localPosition = new Vector3(0, -0.65f - scaleSpritesSlider.value / 2f, 1);
            container.GetComponent<CircleCollider2D>().radius = scaleSpritesSlider.value * 0.5f;
        }
    }


    //These toggle functions should probably get a better spot outside this class. Called from LeftPanel's various toggles.
    public void ToggleCentralIcons0()
    {
        foreach (LocationContainer container in locationContainerList)
            container.centralSprite.GetComponent<SpriteRenderer>().sprite = null;
    }
    public void ToggleCentralIcons1()
    { 
        foreach (LocationContainer container in locationContainerList)
            container.SetContainerSprite();
    }
    public void ToggleCentralIcons2()
    {
        foreach (LocationContainer container in locationContainerList)
        {
            Location loc = container.GetContainedLocation();
            if (loc.politicalType == 0)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = null;
            else if (loc.politicalType == 1)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite1;
            else if (loc.politicalType == 2)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite2;
            else if (loc.politicalType == 3)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite3;
            else if (loc.politicalType == 4)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite4;
            else if (loc.politicalType == 5)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite5;
        }
    }
    public void ToggleCentralIcons3()
    {
        foreach (LocationContainer container in locationContainerList)
        {
            Location loc = container.GetContainedLocation();
            if (loc.economicType == 0)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = null;
            else if (loc.economicType == 1)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite1;
            else if (loc.economicType == 2)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite2;
            else if (loc.economicType == 3)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite3;
            else if (loc.economicType == 4)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite4;
            else if (loc.economicType == 5)
                container.centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite5;
        }
    }

    public void ToggleSubIcons0()
    { 
        foreach (LocationContainer container in locationContainerList)
        {
            container.subIconSprite.SetActive(false);
            container.subIconSprite.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    public void ToggleSubIcons1()
    {
        foreach (LocationContainer container in locationContainerList)
        {
            container.subIconSprite.SetActive(true);
            Location loc = container.GetContainedLocation();
            if (loc.politicalType == 0)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = null;
            else if (loc.politicalType == 1)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite1;
            else if (loc.politicalType == 2)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite2;
            else if (loc.politicalType == 3)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite3;
            else if (loc.politicalType == 4)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite4;
            else if (loc.politicalType == 5)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.politicalSprite5;
        }
    }
    public void ToggleSubIcons2()
    {
        foreach (LocationContainer container in locationContainerList)
        {
            container.subIconSprite.SetActive(true);
            Location loc = container.GetContainedLocation();
            if (loc.economicType == 0)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = null;
            else if (loc.economicType == 1)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite1;
            else if (loc.economicType == 2)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite2;
            else if (loc.economicType == 3)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite3;
            else if (loc.economicType == 4)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite4;
            else if (loc.economicType == 5)
                container.subIconSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.economicSprite5;
        }
    }

    public void ScaleHighlights()
    {
        foreach (LocationContainer container in locationContainerList)
            container.bubbleSprite.transform.localScale = new Vector3(scaleHighlightsSlider.value * 1.0f, scaleHighlightsSlider.value * 1.0f, 1);
    }

    public void ToggleHighlights0()
    {
   
       foreach (LocationContainer container in locationContainerList)
        {
            container.bubbleSprite.SetActive(false);
            container.bubbleSprite.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void ToggleHighlights1()
    {
        foreach (LocationContainer container in locationContainerList)
        {
            container.bubbleSprite.SetActive(true);
            Location loc = container.GetContainedLocation();
            if (loc.culturalType == 1)
                container.bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteBlue;
            else if (loc.culturalType == 2)
                container.bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteRed;
            else if (loc.culturalType == 3)
                container.bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteGreen;
            else if (loc.culturalType == 4)
                container.bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteYellow;
            else if (loc.culturalType == 5)
                container.bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteOrange;
        }
    }
    public void ToggleLines0()
    {
        DestroyLines();
    }
    public void ToggleLines1()
    {
        DestroyLines(); 

        foreach (LocationContainer container in locationContainerList)
        {
            Location loc = container.GetContainedLocation();
            Ruler ruler = loc.localRuler;
            if (ruler != null)            
                if (ruler.rulerHierarchy == Ruler.Hierarchy.Dominating)
                    InstantiateDominationLines(ruler); 
        }
    }
    public void DestroyLines()
    {
        foreach (GameObject obj in lineList)
            if (obj != null)
                Destroy(obj);
        lineList = new List<GameObject>();
    }

    // Probably want to build a proper, dedicated line-drawing class if you start using this function extensively. 
    public void InstantiateDominationLines(Ruler ruler)
    {
        Location loc = ruler.GetHomeLocation();
        List<Location> lineLocations = new List<Location>(); 

        foreach (Location location in ruler.GetControlledLocations())
            lineLocations.Add(location); 

        foreach (Location lineloc in lineLocations)
        {
            GameObject lineObj = Instantiate(linePrefab);
            LineRenderer line = lineObj.GetComponent<LineRenderer>(); 
            // Set false to not scale to map holder and go offpos. Alternatively set localscale to 1,1,1 at bottom.
            lineObj.transform.SetParent(MapController.Instance.GetActiveMapHolder().transform, false);  
            lineList.Add(lineObj);
            line.startWidth = lineStartSize;
            line.endWidth = lineEndSize;
            lineObj.transform.localPosition = new Vector3(0, 0, 0); 
            line.SetPosition(0, ruler.GetPositionVector());
            line.SetPosition(1, lineloc.GetPositionVector());
            line.startColor = Color.blue;
            line.endColor = Color.red;   
        }

    }
    public void HighlightLocationOrange(Location loc)
    {
        GetContainerFromLocation(loc).bubbleSprite.SetActive(true);
        GetContainerFromLocation(loc).bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteOrange;
    }
    public void HighlightLocationBlue(Location loc)
    {
        GetContainerFromLocation(loc).bubbleSprite.SetActive(true);
        GetContainerFromLocation(loc).bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteBlue;
    }
    public void HighlightLocationRed(Location loc)
    {
        GetContainerFromLocation(loc).bubbleSprite.SetActive(true);
        GetContainerFromLocation(loc).bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteRed;
    }
    public void HighlightLocationGreen(Location loc)
    {
        GetContainerFromLocation(loc).bubbleSprite.SetActive(true);
        GetContainerFromLocation(loc).bubbleSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.colorBallSpriteGreen;
    }


}
