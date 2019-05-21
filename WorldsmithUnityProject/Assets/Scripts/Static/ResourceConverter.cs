using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceConverter : MonoBehaviour
{






    public static Resource GetResourceForManpower(Resource.Type foodtype, float manpower)
    {
        return new Resource(Resource.Type.Marble, 55f); 

    }




}
