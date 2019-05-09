﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Warband : EcoBlock
{
    public Dictionary<Resource.Type, Resource> resourcePortfolio;

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
    }

}
