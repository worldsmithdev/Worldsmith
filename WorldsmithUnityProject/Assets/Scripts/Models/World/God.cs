using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class God : WorldElement
{
      
    public God (string name)
    {
        elementType = ElementType.God;
        elementID = name;
    }
}
