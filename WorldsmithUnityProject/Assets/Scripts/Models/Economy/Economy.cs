using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Economy 
{

     
    Dictionary<Ruler, Ruler> rulerDictionary;    
    Dictionary<Warband, Ruler> warbandDictionary;    
    Dictionary<Territory, Ruler> territoryDictionary;
    Dictionary<Population, Ruler> populationDictionary;     
     


    public void SaveEconomy()
    { 
        // Saving here means storing the dictionaries from the Controllers into this Economy class, so that it can be serialized and stored on disk by the WorldSaver
        rulerDictionary = EconomyController.Instance.rulerDictionary;
        populationDictionary = EconomyController.Instance.populationDictionary;
        territoryDictionary = EconomyController.Instance.territoryDictionary;
        warbandDictionary = EconomyController.Instance.warbandDictionary; 
    }
    public void LoadEconomy()
    { 
        // Loading here means taking this Economy's dictionaries and assigning them to the Controllers, which provide access to the data as the program runs
        EconomyController.Instance.rulerDictionary = rulerDictionary;
        EconomyController.Instance.warbandDictionary = warbandDictionary;
        EconomyController.Instance.populationDictionary = populationDictionary;
        EconomyController.Instance.territoryDictionary = territoryDictionary; 
 
    }
}
