using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource : EcoBlock
{

    public enum Type { Unassigned }
    public enum Category { Unassigned }

    public Type type;
    public Category category;
    public float amount;

    public Resource(Type type, float amount)
    {
        this.type = type;
        this.amount = amount;
        this.category = ResourceController.Instance.GetResourceCategory(this);

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
    public Resource(Category category, Type type, float amount)
    {
        this.type = type;
        this.amount = amount;
        this.category = category;

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
