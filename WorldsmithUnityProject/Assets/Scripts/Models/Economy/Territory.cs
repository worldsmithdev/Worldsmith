using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Territory  : EcoBlock
{
    public enum Size { Unassigned, Core, Cluster, Valley, Urban, Chora }
    public enum SoilQuality { Unassigned, Poor, Average, Rich, Volcanic, Splendid }
    public enum Status { Unassigned, Destroyed, Neglected, Maintained, Optimised, Industrialized }


    public Territory(Location loc)
    { 
        blockType = BlockType.Territory;
        xLocation = loc.GetPositionVector().x;
        yLocation = loc.GetPositionVector().y;

        int nr = 1;
        blockID = "TerritoryOf" + loc.elementID;
        blockID += nr;

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = "TerritoryOf" + loc.elementID;
            blockID += nr;
        }


    }

}
