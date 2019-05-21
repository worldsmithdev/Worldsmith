using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Territory  : EcoBlock
{
    public enum Size { Unassigned, Core, Cluster, Valley, Urban, Chora }
    public enum SoilQuality { Unassigned, Poor, Average, Rich, Volcanic, Splendid }
    public enum Status { Unassigned, Destroyed, Neglected, Maintained, Optimised, Industrialized }

    public Size territorySize;
    public SoilQuality territorySoilQuality;
    public Status territoryStatus;


    // STEPS

    public float cycleStoredFood; 
    public Dictionary<Resource.Type, float> cycleGeneratedResources = new Dictionary<Resource.Type, float>();

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


    public void GenerateFoodResources()
    {
        float foodGenerated = this.cycleStoredFood * WorldConstants.RESOURCETYPEQUANTIFIER[Resource.Type.Wheat] * Random.Range(0.95f, 1f);

        cycleGeneratedResources.Add(Resource.Type.Wheat, foodGenerated);
    }

}
