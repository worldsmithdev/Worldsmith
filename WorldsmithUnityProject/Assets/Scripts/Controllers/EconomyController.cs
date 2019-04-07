using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    // Creates content for the World's Economy, holds dictionaries that contain all of the Economy's data

    public static EconomyController Instance { get; protected set; }

    public EconomyBuilder economyBuilder;

    public Dictionary<Ruler, Ruler> rulerDictionary = new Dictionary<Ruler, Ruler>();
    public Dictionary<Warband, Ruler> warbandDictionary = new Dictionary<Warband, Ruler>(); 
    public Dictionary<Population, Ruler> populationDictionary = new Dictionary<Population, Ruler>(); 
    public Dictionary<Territory, Ruler> territoryDictionary = new Dictionary<Territory, Ruler>();  

    private void Awake()
    {
        Instance = this;
    }


    public void BuildEconomy()
    {
        economyBuilder.BuildRulers(); 
        economyBuilder.BuildWarbands();
        economyBuilder.BuildTerritories();
        economyBuilder.BuildPopulations();
        economyBuilder.BuildResources();
    }

    public  Ruler GetLocationRuler(Location loc)
    {
        return loc.localRuler;
    }
    public List<Ruler> GetLocationRulers(Location loc)
    {
        List<Ruler> returnList = new List<Ruler>();

        foreach (Ruler ruler in loc.secondaryRulers)        
            if (ruler != null)
                returnList.Add(ruler);        

        returnList.Add(loc.localRuler);
        return returnList;
    }

  
   
    public Ruler GetRulerFromID (string id)
    {
        foreach (Ruler ruler in rulerDictionary.Keys)        
            if (ruler.blockID == id)
                return ruler;        
        Debug.Log("Tried to find Ruler but no id match for "+ id);
        return null;
    }



    public void ClearDictionaries()
    { 
    rulerDictionary = new Dictionary<Ruler, Ruler>();
    warbandDictionary = new Dictionary<Warband, Ruler>();
    populationDictionary = new Dictionary<Population, Ruler>();
    territoryDictionary = new Dictionary<Territory, Ruler>(); 
    }

    public bool BlockIDExists(string id, EcoBlock.BlockType type)
    {
        if (type == EcoBlock.BlockType.Ruler)
        {
            foreach (Ruler ruler in rulerDictionary.Keys)
                if (ruler.blockID == id)
                    return true;
        }          
        else if (type == EcoBlock.BlockType.Warband)
        {
            foreach (Warband warband in warbandDictionary.Keys)
                if (warband.blockID == id)
                    return true;
        }
        else if (type == EcoBlock.BlockType.Territory)
        {
            foreach (Territory terr in territoryDictionary.Keys)
                if (terr.blockID == id)
                    return true;
        }
        else if (type == EcoBlock.BlockType.Population)
        {
            foreach (Population pop in populationDictionary.Keys)
                if (pop.blockID == id)
                    return true;
        } 
        return false;
    }
}
