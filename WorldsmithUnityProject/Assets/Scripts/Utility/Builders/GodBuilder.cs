using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBuilder : MonoBehaviour
{

    public GodImports imports;

    public void CreateGods(World activeWorld)
    { 
        foreach (GodImportsData data in imports.dataArray)
        {
            if (data.Name != "")
            {
                God created = new God(data.Name);
                created.description = data.Description;
                activeWorld.godList.Add(created); 
            } 
        } 
    }
}
