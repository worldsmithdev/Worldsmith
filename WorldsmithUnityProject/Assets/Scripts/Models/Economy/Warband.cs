using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Warband : EcoBlock
{ 

    public Warband(Location loc)
    { 
        
        blockType = BlockType.Warband;
        xLocation = loc.GetPositionVector().x;
        yLocation = loc.GetPositionVector().y;
         
        int nr = 1; 
        blockID = "WarbandOf" + loc.elementID;
        blockID += nr;

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++; 
            blockID = "WarbandOf" + loc.elementID;
            blockID += nr;
        } 
        foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)
            resourcePortfolio.Add(restype, new Resource(restype, 0));
    }

}
