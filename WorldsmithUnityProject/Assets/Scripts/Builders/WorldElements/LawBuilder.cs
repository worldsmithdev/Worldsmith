using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawBuilder : MonoBehaviour
{
     public LawImports imports;

    public void CreateLaws(World activeWorld)
    { 
       foreach (LawImportsData data in imports.dataArray)
       {
            if (data.Name != "")
            {
                Law created = new Law(data.Name);
                activeWorld.lawList.Add(created);
                created.description = data.Description;
            } 
       } 
    }
}
