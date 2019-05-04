using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsController : MonoBehaviour
{
    public static StepsController Instance { get; protected set; }
 

    private void Awake()
    {
        Instance = this;
    }
}
