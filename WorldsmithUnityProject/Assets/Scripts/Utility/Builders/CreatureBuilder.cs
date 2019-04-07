using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBuilder : MonoBehaviour
{
    
   public CreatureImports imports;

    public void CreateCreatures(World activeWorld)
    { 
          foreach (CreatureImportsData data in imports.dataArray)
          {
            if (data.Name != "")
            { 
                Creature created = new Creature(data.Name );
                created.description = data.Description;
                activeWorld.creatureList.Add(created);
                created.SetLocation(data.Location);

            }
        }
    }
}
