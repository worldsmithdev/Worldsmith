using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; protected set; }


    public Dictionary<Resource.Type, Resource.Category > resourceCompendium = new Dictionary<Resource.Type, Resource.Category>();

    private void Awake()
    {
        Instance = this;
         
        resourceCompendium.Add(Resource.Type.Wheat, Resource.Category.Food );
        resourceCompendium.Add(Resource.Type.Wares, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.Silver, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Timber, Resource.Category.Natural ); 
        resourceCompendium.Add(Resource.Type.Stone, Resource.Category.Natural ); 
        resourceCompendium.Add(Resource.Type.Salt, Resource.Category.Natural); 
        resourceCompendium.Add(Resource.Type.Marble, Resource.Category.Natural ); 
    }
     


    public Resource.Category GetResourceCategory(Resource res)
    {
        return resourceCompendium[res.type];
    }

 

}
