using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Population : EcoBlock
{
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
}
