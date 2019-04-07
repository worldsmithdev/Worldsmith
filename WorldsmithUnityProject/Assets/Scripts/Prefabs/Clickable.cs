using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clickable : MonoBehaviour 
{
    // Prefab script for versatile container that can be hovered and clicked for different purposes (currently only used for SubMap selection)

    public enum ClickableType { Unassigned, SubMapField}
    public ClickableType clickableType;
    public TextMeshProUGUI text;
    public GameObject mainSpriteObject;
    public GameObject triggerSpriteObject;
    public bool activated = false;


    private void OnMouseOver()
    {
        if (activated == true)
        {
            triggerSpriteObject.SetActive(true);
            if (clickableType == ClickableType.SubMapField)
                MapController.Instance.HoverClickable(text.text);
        }
    }
    private void OnMouseExit()
    {
        if (activated == true)
        {
            triggerSpriteObject.SetActive(false);
            if (clickableType == ClickableType.SubMapField)
                MapController.Instance.ExitHoverClickable();
        } 
    }
    private void OnMouseDown()
    {
        if (clickableType == ClickableType.SubMapField)
        {
            triggerSpriteObject.SetActive(false);
            MapController.Instance.ClickMapClickable(text.text);
        }
    }
}
