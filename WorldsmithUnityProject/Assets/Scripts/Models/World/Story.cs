using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Story : WorldElement
{ 
    public string fullText;
    public Location loc1;
    public Location loc2;
    public Character character1;
    public Character character2;
    public God god1;
    public God god2;
    public Faction faction1;
    public Faction faction2;
    public Creature creature1;
    public Creature creature2;
    public Item item1;
    public Item item2;

    public Story (string name)
    {
        elementType = ElementType.Story;
        elementID = name;

        int nr = 1;
        elementID = name;
        while (WorldController.Instance.ElementIDExists(this.elementID, this.elementType) == true)
        {
            nr++;
            elementID = name + nr;
        }
    }
}
