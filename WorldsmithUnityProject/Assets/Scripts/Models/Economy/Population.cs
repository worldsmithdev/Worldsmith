using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Population : EcoBlock
{

    public Population(Location loc)
    {
        blockID = "PopulationOf" + loc.elementID;
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
    }
}
