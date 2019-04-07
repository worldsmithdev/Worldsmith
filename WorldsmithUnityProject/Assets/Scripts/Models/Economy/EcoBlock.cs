﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EcoBlock  
{
    // Parent class for economic elements: Ruler, Warband, Territory, Population, Resource

    public enum BlockType { Unassigned, Ruler, Warband, Population, Territory, Resource}

    public BlockType blockType;
    public string blockID;
     
    public float xLocation;
    public float yLocation;


    public Vector3 GetPositionVector()
    {
        return new Vector3(xLocation, yLocation, 0);
    }


}
