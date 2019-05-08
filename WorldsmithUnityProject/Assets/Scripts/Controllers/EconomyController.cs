using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    // Creates content for the World's Economy, holds dictionaries that contain all of the Economy's data

    public static EconomyController Instance { get; protected set; }
     
    public RulerBuilder rulerBuilder;
    public WarbandBuilder warbandBuilder;
    public TerritoryBuilder territoryBuilder;
    public PopulationBuilder populationBuilder;
    public ResourceBuilder resourceBuilder;

    // Eventually probably move these dictionaries to RulerController, etc. 
    public Dictionary<Ruler, Ruler> rulerDictionary = new Dictionary<Ruler, Ruler>();
    public Dictionary<Warband, Ruler> warbandDictionary = new Dictionary<Warband, Ruler>(); 
    public Dictionary<Population, Ruler> populationDictionary = new Dictionary<Population, Ruler>(); 
    public Dictionary<Territory, Ruler> territoryDictionary = new Dictionary<Territory, Ruler>();  

    private void Awake()
    {
        Instance = this;
    }

    // Split these later into dedicated scripts. And call it create > build for consistency with WorldController.
    public void BuildEconomy()
    {
        rulerBuilder.BuildRulers();
        warbandBuilder.BuildWarbands();
        territoryBuilder.BuildTerritories();
        populationBuilder.BuildPopulations();
        resourceBuilder.BuildResources();
    } 


    public void ClearDictionaries()
    { 
    rulerDictionary = new Dictionary<Ruler, Ruler>();
    warbandDictionary = new Dictionary<Warband, Ruler>();
    populationDictionary = new Dictionary<Population, Ruler>();
    territoryDictionary = new Dictionary<Territory, Ruler>(); 
    }

    // To check if a name already exists when creating an EcoBlock
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
