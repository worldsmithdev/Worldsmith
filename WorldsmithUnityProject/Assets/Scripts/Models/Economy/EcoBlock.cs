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

    public Dictionary<Resource.Type, Resource> resourcePortfolio = new Dictionary<Resource.Type, Resource>();

    // Steps Nonsense
    public List<Resource.Type> localProfitResourceTypes = new List<Resource.Type>();
    public Dictionary<Resource.Type, float> cycleRegionalCommittedResources = new Dictionary<Resource.Type, float>();

    public Vector3 GetPositionVector()
    {
        return new Vector3(xLocation, yLocation, 0);
    }


}
