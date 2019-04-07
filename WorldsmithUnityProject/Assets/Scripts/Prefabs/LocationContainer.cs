using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationContainer : MonoBehaviour
{
    // Prefab script for Location Containers. Initializes them and handles hovers/clicks

    Location containedLocation;
    float xCoord;
    float yCoord;
     
    public Location.LocationType locationType; 
    public TextMeshProUGUI nameText;
    public GameObject centralSprite;
    public GameObject highlightSprite;
    public GameObject selectionSprite;
    public GameObject subIconSprite;
    public GameObject bubbleSprite;

    public bool hideIcon;
    public bool hideName;

    private void Start()
    { 
        highlightSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.containerHighlightSprite;
        selectionSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.containerSelectionSprite;

        // fix when the Z goes negative automatically (why does it?), hiding the sprite
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);

        if (hideIcon == true)
            centralSprite.SetActive(false);
        if (hideName == true)
            nameText.gameObject.SetActive(false);

    }
     

    void OnMouseEnter()
    {
        ContainerController.Instance.SetHoveredContainer(this); 
            HoverHighlight();
    }

    void OnMouseDown()
    {     
            ContainerController.Instance.ClickContainer(this);        
    }

    void OnMouseExit()
    {
        RemoveHoverHighlight();
        ContainerController.Instance.SetHoveredContainer(null);
    }


    public void HoverHighlight()
    {
        highlightSprite.SetActive(true); 
    }
    public void RemoveHoverHighlight()
    {
        highlightSprite.SetActive(false); 
    }
    public void SelectHighlight()
    {
        selectionSprite.SetActive(true);
    }
    public void RemoveSelectHighlight()
    {
        selectionSprite.SetActive(false);
    }

    public void SetContainedLocation()
    { 
        foreach (Location loc in WorldController.Instance.GetWorld().locationList) 
            if (loc.GetLocationType() == this.locationType && loc.elementID == this.transform.name) 
                containedLocation = loc;  
       
        if (containedLocation == null)
        {
            Debug.Log("Did not find a match in imports for container: " + this.transform.name + " with loctype: " + this.locationType + ". Please check that name and type matches with database.");
        }
        else
        {
            containedLocation.SetXLocation(GetXCoord());
            containedLocation.SetYLocation(GetYCoord());
            nameText.text = "" + containedLocation.elementID;
        }
        
    }

    public void SetContainerSprite()
    {
        if (containedLocation != null)
        {
            if (SpriteCollection.Instance.locationSubTypePairings.ContainsKey(containedLocation.GetLocationSubType()))
                centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.locationSubTypePairings[containedLocation.GetLocationSubType()];
            else
                centralSprite.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.locationTypePairings[containedLocation.GetLocationType()];
        }
    }



    public Location GetContainedLocation()
    {
        return containedLocation;
    }
    public float GetXCoord()
    {
        return gameObject.transform.localPosition.x;
    }
    public float GetYCoord()
    {
        return gameObject.transform.localPosition.y;
    }
}
