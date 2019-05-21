using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPoliticsStep : Step
{
    public override void ConfigureStep(EcoBlock ecoblock)
    {
        Ruler ruler = (Ruler)ecoblock;
        ClearStepVariables(ruler);

        if (ruler.isLocalRuler)
            DetermineGenerationLevy(ruler); 
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Ruler ruler = (Ruler)ecoblock;

        if (ruler.isLocalRuler)
            LevyGeneration(ruler);
    }

    public override void ResolveStep(EcoBlock ecoblock)
    {
        Ruler ruler = (Ruler)ecoblock;

    }

    void DetermineGenerationLevy (Ruler ruler)
    {
        if (ruler.attitude == Ruler.Attitude.Aggressive || ruler.attitude == Ruler.Attitude.Economic)
            ruler.cycleGenerationLevyPercentage = WorldConstants.GENERATION_LEVY_RATE_HI;
        else if (ruler.attitude == Ruler.Attitude.Independent || ruler.attitude == Ruler.Attitude.Temperate)
            ruler.cycleGenerationLevyPercentage = WorldConstants.GENERATION_LEVY_RATE_MID;
        else if (ruler.attitude == Ruler.Attitude.Submissive)
            ruler.cycleGenerationLevyPercentage = WorldConstants.GENERATION_LEVY_RATE_LO;
    }
    void LevyGeneration(Ruler ruler)
    {
        Territory terr = ruler.GetHomeLocation().locationTerritory;
        float leviedAmount;
        Dictionary<Resource.Type, float> leviedResources = new Dictionary<Resource.Type, float>();
        foreach (Resource.Type restype in terr.storedResources.Keys)        
            if (terr.storedResources[restype] > 0)
            {
                leviedAmount = terr.storedResources[restype] * ruler.cycleGenerationLevyPercentage;
                leviedResources.Add(restype, leviedAmount);
            }
        foreach (Resource.Type restype in leviedResources.Keys)
        {
            terr.storedResources[restype] -= leviedResources[restype];
            ruler.resourcePortfolio[restype].amount += leviedResources[restype];
        }


    }
    void ClearStepVariables(Ruler ruler)
    { 

    }
}
