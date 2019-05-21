using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; protected set; }


    public Dictionary<Resource.Type, Resource.Category> resourceCompendium = new Dictionary<Resource.Type, Resource.Category>();

    private void Awake()
    {
        Instance = this;
    }

    public Resource.Category GetResourceCategory(Resource res)
    {
        return resourceCompendium[res.type];
    }

}
