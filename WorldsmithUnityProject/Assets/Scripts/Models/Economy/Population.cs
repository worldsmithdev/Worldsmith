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

    public Dictionary<Resource.Type, int> resourceBuyPriority = new Dictionary<Resource.Type, int >();
    public Dictionary<Resource.Type, int> resourceSellPriority = new Dictionary<Resource.Type, int>();



    // INDUSTRY Step
    public float cycleWaresManpower;
    public Dictionary<Resource.Type, float> cycleGeneratedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> cycleCreatedResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> cycleAvailableResources = new Dictionary<Resource.Type, float>();
    public List<Resource> cyclePaidLevyResources = new List<Resource>();

    // POPULATION Step
    public float cycleDesiredFoodConsumption;
    public float cycleActualFoodConsumption;
    public float cycleDesiredComfortConsumption;
    public float cycleActualComfortConsumption;
    public int successiveFoodConsumptionShortage = 0;
    public List<float> foodConsumptionShortageList = new List<float>();
    public int successiveComfortConsumptionShortage = 0;
    public List<float> comfortConsumptionShortageList = new List<float>();

    // LOCALEXCHANGE Step
    public Dictionary<Resource.Type, float> cycleLocalSurplusResources = new Dictionary<Resource.Type, float>();
    public Dictionary<Resource.Type, float> cycleLocalWantedResources = new Dictionary<Resource.Type, float>();


    public Population(ClassType givenclasstype, LaborType givenlabortype, float proportion, Location loc)
    {
        classType = givenclasstype;
        laborType = givenlabortype;
        blockType = BlockType.Population;
        xLocation = loc.GetPositionVector().x;
        yLocation = loc.GetPositionVector().y;

        ResourceController.Instance.SetResourceBuyPriority(this);
        ResourceController.Instance.SetResourceSellPriority(this);

        int nr = 1;
        blockID = classType.ToString().Substring(0, 1) + laborType.ToString().Substring(0, 1) + "Pop"   +loc.elementID + nr;
        blockID += nr;
         
        amount = (int) (WorldConstants.GetPopulationAmount(loc) * proportion);
        loc.populationList.Add(this);

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = classType.ToString() + laborType.ToString() + "Pop" + loc.elementID + nr;
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
        if (level > 100)
            level = 100;
        return level;
    }
    public float GetAverageLuxuryShortage()
    {
        float totalshortage = 0f;
        foreach (float entry in this.comfortConsumptionShortageList)
            totalshortage += entry;
        float avg = totalshortage / this.comfortConsumptionShortageList.Count;
        float level = (avg * 100) / this.cycleActualComfortConsumption;
        if (level > 100)
            level = 100;
        return level;
    }
}
