using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource : EcoBlock
{

 //   public enum Type { Unassigned, Wheat, Wares, Silver, Timber, Stone, Salt, Marble, Pork, Amber }
    public enum Category { Unassigned, Farmed, Natural, Manufactured, Metal }

    public enum Type
    {
        Unassigned, Wheat, Barley, Olives, Grapes, Fruits, Pulses, Honey, Wine, Livestock,    // FARMED 
        Timber, Stone, Limestone, Marble, Clay, Salt, Sulfur, Amber, Travertine, Pork, Fish,   // NATURAL
        Glass, Arms, Wares, LocalWares, LocalGreekWares, GreekWares,                    // MANUFACTURED
        Silver, Gold, Bronze, Iron, Copper, Lead, Tin                          // METALS
    }

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
