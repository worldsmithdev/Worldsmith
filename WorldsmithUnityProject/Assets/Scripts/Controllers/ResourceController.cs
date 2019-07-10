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

 
    public void InitiateResourceDictionary (Dictionary<Resource.Type, float> dict)
    {
       // dict = new Dictionary<Resource.Type, float>();
        foreach (Resource.Type restype in resourceCompendium.Keys)        
            dict.Add(restype, 0);        
    }


    public Resource.Category GetResourceCategory(Resource res)
    {
        return resourceCompendium[res.type];
    }


    public void SetResourceBuyPriority(Ruler ruler)
    {
        ruler.resourceBuyPriority = new Dictionary<Resource.Type, int>();
        ruler.resourceBuyPriority.Add(Resource.Type.Silver, 1);
        ruler.resourceBuyPriority.Add(Resource.Type.Wheat, 2);
        ruler.resourceBuyPriority.Add(Resource.Type.Wares, 3);
         
        foreach (Resource.Type restype in resourceCompendium.Keys)        
            if (ruler.resourceBuyPriority.ContainsKey(restype) == false)            
                ruler.resourceBuyPriority.Add(restype, 0);        


    }
    public void SetResourceBuyPriority(Warband warband)
    {
        warband.resourceBuyPriority = new Dictionary<Resource.Type, int>();
        warband.resourceBuyPriority.Add(Resource.Type.Silver, 1);
        warband.resourceBuyPriority.Add(Resource.Type.Wares, 2);
        warband.resourceBuyPriority.Add(Resource.Type.Wheat, 3);

        foreach (Resource.Type restype in resourceCompendium.Keys)
            if (warband.resourceBuyPriority.ContainsKey(restype) == false)
                warband.resourceBuyPriority.Add(restype, 0);
    }
    public void SetResourceBuyPriority(Population population)
    {
        population.resourceBuyPriority = new Dictionary<Resource.Type, int>();
        population.resourceBuyPriority.Add(Resource.Type.Wheat, 1);
        population.resourceBuyPriority.Add(Resource.Type.Silver, 2);
        population.resourceBuyPriority.Add(Resource.Type.Wares, 3);

        foreach (Resource.Type restype in resourceCompendium.Keys)
            if (population.resourceBuyPriority.ContainsKey(restype) == false)
                population.resourceBuyPriority.Add(restype, 0);
    }


    public void SetResourceSellPriority(Ruler ruler)
    {
        ruler.resourceSellPriority = new Dictionary<Resource.Type, int>(); 
        ruler.resourceSellPriority.Add(Resource.Type.Wheat, 1);
        ruler.resourceSellPriority.Add(Resource.Type.Wares, 2);

        foreach (Resource.Type restype in resourceCompendium.Keys)
            if (ruler.resourceSellPriority.ContainsKey(restype) == false)
                ruler.resourceSellPriority.Add(restype, 0);


    }
    public void SetResourceSellPriority(Warband warband)
    {
        warband.resourceSellPriority = new Dictionary<Resource.Type, int>();
        warband.resourceSellPriority.Add(Resource.Type.Wheat, 1);
        warband.resourceSellPriority.Add(Resource.Type.Wares, 2);

        foreach (Resource.Type restype in resourceCompendium.Keys)
            if (warband.resourceSellPriority.ContainsKey(restype) == false)
                warband.resourceSellPriority.Add(restype, 0);
    }
    public void SetResourceSellPriority(Population population)
    {
        population.resourceSellPriority = new Dictionary<Resource.Type, int>();
        population.resourceSellPriority.Add(Resource.Type.Wheat, 1);
        population.resourceSellPriority.Add(Resource.Type.Wares, 2);

        foreach (Resource.Type restype in resourceCompendium.Keys)
            if (population.resourceSellPriority.ContainsKey(restype) == false)
                population.resourceSellPriority.Add(restype, 0);
    }

}
