using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter  
{

    public static Dictionary<Resource.Type, float> RESOURCE_PER_MANPOWER_INDEX = new Dictionary<Resource.Type, float>();

    public static void SetDictionaries()
    {
        // NATURAL
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Wheat, 3.0f); //  HectoLiter
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Timber, 15.0f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Stone, 10.0f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Limestone, 10.0f); //  KG 
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Marble, 5.0f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Clay, 5.0f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Salt, 0.1f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Sulfur, 0.1f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Amber, 0.05f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Travertine, 5.0f); //  KG        
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Pork, 20.0f); //  KG
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Fish, 20.0f); //  KG        
        // MANUFACTURED
        RESOURCE_PER_MANPOWER_INDEX.Add(Resource.Type.Wares, 10.0f); //  piece        

    }


    public static Resource GetResourceForManpower(Resource.Type restype, float manpower)
    {
        if (RESOURCE_PER_MANPOWER_INDEX.ContainsKey(restype))
             return new Resource(restype, (manpower * RESOURCE_PER_MANPOWER_INDEX[restype]) );

        Debug.LogWarning("Trying to get resource of type " + restype + " - but it is not found as a key.");
        return null;
    }

}
