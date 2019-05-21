using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Population : EcoBlock
{
    public enum ClassType { Unassigned, Citizen, Habitant, Slave }
    public enum LaborType { Unassigned, Leisure, Artisan, Housekeeper, Builder }

    public ClassType classType;
    public LaborType laborType;

    public int peopleAmount;

    public Dictionary<Resource.Type, Resource> resourcePortfolio = new Dictionary<Resource.Type, Resource>();

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
         
        peopleAmount = (int) (WorldConstants.GetPopulationAmount(loc) * proportion);
        loc.populationList.Add(this);

        while (EconomyController.Instance.BlockIDExists(this.blockID, this.blockType) == true)
        {
            nr++;
            blockID = classType.ToString() + laborType.ToString() + "PopulationOf" + loc.elementID;
            blockID += nr;
        } 
        foreach (Resource.Type restype in ResourceController.Instance.resourceCompendium.Keys)
            resourcePortfolio.Add(restype, new Resource(restype, 0));
    }
}
