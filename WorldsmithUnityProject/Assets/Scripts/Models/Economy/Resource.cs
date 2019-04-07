using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource : EcoBlock
{

    public enum ResourceType {Unassigned }

    public ResourceType resourceType;

    public Resource()
    {
        int nr = 1;
        blockID = "Resource" + resourceType;
        blockID += nr;

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = "Resource" + resourceType;
            blockID += nr;
        }
    }
}
