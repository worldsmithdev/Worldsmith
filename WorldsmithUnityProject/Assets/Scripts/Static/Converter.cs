using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter  
{

    public static Dictionary<Resource.Type, float> RESOURCE_PER_MANPOWER_INDEX = new Dictionary<Resource.Type, float>();
    public static Dictionary<Resource.Type, float> unitSilverValues = new Dictionary<Resource.Type, float>();

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

        unitSilverValues.Add(Resource.Type.Silver, 1f);
        unitSilverValues.Add(Resource.Type.Wheat, 50f);
        unitSilverValues.Add(Resource.Type.Wares, 2f);
        unitSilverValues.Add(Resource.Type.Timber, 10f);
        unitSilverValues.Add(Resource.Type.Stone, 10f);
        unitSilverValues.Add(Resource.Type.Salt, 20f);
        unitSilverValues.Add(Resource.Type.Marble, 20f);
        unitSilverValues.Add(Resource.Type.Pork, 2f);
        unitSilverValues.Add(Resource.Type.Clay, 15f);

    }


    public static Resource GetResourceForManpower(Resource.Type restype, float manpower)
    {
        if (RESOURCE_PER_MANPOWER_INDEX.ContainsKey(restype))
             return new Resource(restype, (manpower * RESOURCE_PER_MANPOWER_INDEX[restype]) );

        Debug.LogWarning("Trying to get resource of type " + restype + " - but it is not found as a key.");
        return null;
    }

    public static float GetSilverEquivalent (Resource resource)
    {
        if (unitSilverValues.ContainsKey(resource.type))
        return unitSilverValues[resource.type] * resource.amount;
        else
        {
            Debug.Log("Cannot get silver equivalent, entry missing for: " + resource.type);
            return 0f;
        }
    }
    public static Resource GetResourceEquivalent(Resource.Type restype, float silverAmount)
    {
    //    Debug.Log("For " + silverAmount + "value get " + (silverAmount / unitSilverValues[restype]) + " of type " + restype); 
        return  new Resource(restype, silverAmount/ unitSilverValues[restype]) ;
    }

    public static Resource GetResourceEquivalent(Participant part, Resource.Type restype, float silverAmount)
    {
     //   Debug.Log("For " + silverAmount + "value " + part.participantName +" gets " + (silverAmount / unitSilverValues[restype]) + " of type " + restype);
        return new Resource(restype, silverAmount / unitSilverValues[restype]);
    }
}
