using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; protected set; }


    public Dictionary<Resource.Category, Resource.Type> resourceCompendium = new Dictionary<Resource.Category, Resource.Type>();

    private void Awake()
    {
        Instance = this;
    }


   
}
