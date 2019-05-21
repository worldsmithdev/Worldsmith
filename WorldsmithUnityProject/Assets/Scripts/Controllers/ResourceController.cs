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
         
        resourceCompendium.Add(Resource.Type.Wheat, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Barley, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Olives, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Grapes, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Fruits, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Pulses, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Honey, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Wine, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Livestock, Resource.Category.Farmed );
        resourceCompendium.Add(Resource.Type.Timber, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Stone, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Limestone, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Marble, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Clay, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Salt, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Sulfur, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Amber, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Travertine, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Pork, Resource.Category.Natural);
        resourceCompendium.Add(Resource.Type.Fish, Resource.Category.Natural); 
        resourceCompendium.Add(Resource.Type.Wares, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.LocalWares, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.LocalGreekWares, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.GreekWares, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.Glass, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.Arms, Resource.Category.Manufactured );
        resourceCompendium.Add(Resource.Type.Silver, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Gold, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Bronze, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Iron, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Copper, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Lead, Resource.Category.Metal );
        resourceCompendium.Add(Resource.Type.Tin, Resource.Category.Metal );
     
    }

 

    public Resource.Category GetResourceCategory(Resource res)
    {
        return resourceCompendium[res.type];
    }

 

}
