using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Population : EcoBlock
{
    public enum ClassType { Unassigned, Citizen, Habitant, Slave }
    public enum LaborType { Unassigned, Leisure, Artisan, Housekeeper }

    public ClassType classType;
    public LaborType laborType;

    public int amount;

    public Dictionary<Resource.Type, Resource> resourcePortfolio = new Dictionary<Resource.Type, Resource>();

    // INDUSTRY Step
    public float cycleWaresManpower;
    public Dictionary<Resource.Type, float> cycleGeneratedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> cycleCreatedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> cycleAvailableResources = new Dictionary<Resource.Type, float>();
    public List<Resource> cyclePaidLevyResources = new List<Resource>();

    // POPULATION Step
    public float cycleDesiredFoodConsumption;
    public float cycleActualFoodConsumption;
    public float cycleDesiredLuxuryConsumption;
    public float cycleActualLuxuryConsumption;
    public int successiveFoodConsumptionShortage = 0;
    public List<float> foodConsumptionShortageList = new List<float>();
    public int successiveLuxuryConsumptionShortage = 0;
    public List<float> luxuryConsumptionShortageList = new List<float>();

    // LOCALEXCHANGE Step
    public Dictionary<Resource.Type, float> cycleLocalSurplusResources = new Dictionary<Resource.Type, float>();


    public Population(ClassType givenclasstype, LaborType givenlabortype, float proportion, Location loc)
    {
        classType = givenclasstype;
        laborType = givenlabortype;
        blockType = BlockType.Population;
        xLocation = loc.GetPositionVector().x;
        yLocation = loc.GetPositionVector().y;

        int nr = 1;
        blockID = classType.ToString() +laborType.ToString() + "PopulationOf" + loc.elementID;
        blockID += nr;
         
        amount = (int) (WorldConstants.GetPopulationAmount(loc) * proportion);
        loc.populationList.Add(this);

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = classType.ToString() + laborType.ToString() + "PopulationOf" + loc.elementID;
            blockID += nr;
        } 
        foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)
        {
            resourcePortfolio.Add(restype, new Resource(restype, 0));
            cycleAvailableResources.Add(restype, 0);
        }
    }
    public void ShiftLabor (ClassType classtype, LaborType fromLabor, LaborType toLabor)
    {

    }
    public Location GetHomeLocation()
    {
        foreach (Location location in WorldController.Instance.GetWorld().locationList)        
            if (location.populationList.Contains(this))
                return location;

        Debug.LogWarning("Trying to find Location for Population " + this.blockID + " - no Location found!");
        return null;        
    }

    public float GetAverageFoodShortage()
    { 
        float totalshortage = 0f;
        foreach (float entry in this.foodConsumptionShortageList)
            totalshortage += entry;
        float avg = totalshortage / this.foodConsumptionShortageList.Count;
        float level = (avg * 100) / this.cycleDesiredFoodConsumption;

        return level;
    }
    public float GetAverageLuxuryShortage()
    {
        float totalshortage = 0f;
        foreach (float entry in this.luxuryConsumptionShortageList)
            totalshortage += entry;
        float avg = totalshortage / this.luxuryConsumptionShortageList.Count;
        float level = (avg * 100) / this.cycleActualLuxuryConsumption;

        return level;
    }
}
