using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Population : EcoBlock
{
    public enum ClassType { Unassigned}
    public enum LaborType { Unassigned}

    public ClassType classType;
    public LaborType laborType;

    public int amount;

    public Dictionary<Resource.Type, Resource> resourcePortfolio = new Dictionary<Resource.Type, Resource>();

    public Population(Location loc)
    { 
        blockType = BlockType.Population;
        xLocation = loc.GetPositionVector().x;
        yLocation = loc.GetPositionVector().y;

        int nr = 1;
        blockID = "PopulationOf" + loc.elementID;
        blockID += nr;

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = "PopulationOf" + loc.elementID;
            blockID += nr;
        }
        foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)
            resourcePortfolio.Add(restype, new Resource(restype, 0));
    }
    public Location GetHomeLocation()
    {
        foreach (Location location in WorldController.Instance.GetWorld().locationList)
            if (location.populationList.Contains(this))
                return location;

        Debug.LogWarning("Trying to find Location for Population " + this.blockID + " - no Location found!");
        return null;
    }
}
