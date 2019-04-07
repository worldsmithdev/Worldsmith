using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionBuilder : MonoBehaviour
{
   
     public FactionImports imports;

    public void CreateFactions(World activeWorld)
    { 
       foreach (FactionImportsData data in imports.dataArray)
       {
            if (data.Name != "")
            {
                Faction created = new Faction(data.Name);
                created.description = data.Description;
                activeWorld.factionList.Add(created);
            } 
       } 
    }

}
