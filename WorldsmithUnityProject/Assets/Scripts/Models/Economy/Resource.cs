using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource : EcoBlock
{

    public enum Type { Wheat, Wares, Silver, Timber, Stone, Salt, Marble }
    public enum Category { Unassigned, Food, Natural, Manufactured, Metal }

    public Type type;
    public Category category;

    public Resource()
    {
        int nr = 1;
        blockID = "Resource" + type;
        blockID += nr;

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = "Resource" + type;
            blockID += nr;
        }
    }
}
