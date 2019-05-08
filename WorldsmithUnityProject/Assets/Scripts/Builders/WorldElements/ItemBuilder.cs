using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
     public ItemImports imports;

    public void CreateItems(World activeWorld)
    { 
        foreach (ItemImportsData data in imports.dataArray)
        {
            if (data.Name != "")
            {
                Item created = new Item(data.Name );
                created.description = data.Description;
                activeWorld.itemList.Add(created);
                created.SetLocation(data.Location);
            } 
        } 
    }
}
