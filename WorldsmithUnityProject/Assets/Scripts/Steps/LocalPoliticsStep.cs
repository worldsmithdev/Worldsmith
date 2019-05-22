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
        {
            DetermineGenerationLevy(ruler);
            DetermineIndustryLevy(ruler);
        }
    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        Ruler ruler = (Ruler)ecoblock;

        if (ruler.isLocalRuler)
        {
            LevyGeneration(ruler);
            LevyIndustry(ruler);
        }
    }

    public override void ResolveStep()
    {
        StoreAvailableResources();

    }
    void StoreAvailableResources()
    { 
        foreach (Population population in EconomyController.Instance.populationDictionary.Keys)
        {
            foreach (Resource.Type restype in population.cycleAvailableResources.Keys)
                if (population.cycleAvailableResources[restype] > 0)
                { 
                    population.resourcePortfolio[restype].amount += population.cycleAvailableResources[restype]; 
                }
        }       
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
    void DetermineIndustryLevy(Ruler ruler)
    {
        if (ruler.attitude == Ruler.Attitude.Aggressive || ruler.attitude == Ruler.Attitude.Economic)
            ruler.cycleIndustryLevyPercentage = WorldConstants.INDUSTRY_LEVY_RATE_HI;
        else if (ruler.attitude == Ruler.Attitude.Independent || ruler.attitude == Ruler.Attitude.Temperate)
            ruler.cycleIndustryLevyPercentage = WorldConstants.INDUSTRY_LEVY_RATE_MID;
        else if (ruler.attitude == Ruler.Attitude.Submissive)
            ruler.cycleIndustryLevyPercentage = WorldConstants.INDUSTRY_LEVY_RATE_LO;
    }
    void LevyGeneration(Ruler ruler)
    {
        Location loc = ruler.GetHomeLocation();
        List<Population> housekeeperPopulations = new List<Population>();
        foreach (Population pop in loc.populationList)
            if (pop.laborType == Population.LaborType.Housekeeper)
                housekeeperPopulations.Add(pop);
        float leviedAmount;
        foreach (Population pop in housekeeperPopulations)
        {
            foreach (Resource.Type restype in pop.cycleGeneratedResources.Keys)
            { 
                if (pop.cycleGeneratedResources[restype] > 0)
                {
                    if (pop.classType == Population.ClassType.Slave)
                        leviedAmount = pop.cycleGeneratedResources[restype];
                    else
                         leviedAmount = pop.cycleGeneratedResources[restype] * ruler.cycleGenerationLevyPercentage;
                    ruler.cycleLeviedResources.Add(new Resource(restype, leviedAmount));
                    pop.cycleAvailableResources[restype] -= leviedAmount;
                    ruler.resourcePortfolio[restype].amount += leviedAmount; 
                }
            }
        } 
    }
    void LevyIndustry(Ruler ruler)
    {
        Location loc = ruler.GetHomeLocation();
        List<Population> artisanPopulations = new List<Population>();
        foreach (Population pop in loc.populationList)
            if (pop.laborType == Population.LaborType.Artisan)
                artisanPopulations.Add(pop);
        float leviedAmount = 0f;
        foreach (Population pop in artisanPopulations)
        {
            foreach (Resource.Type restype in pop.cycleCreatedResources.Keys)
            { 
                if (pop.cycleCreatedResources[restype] > 0)
                {
                    if (pop.classType == Population.ClassType.Slave)
                        leviedAmount = pop.cycleCreatedResources[restype];
                    else                     
                         leviedAmount = pop.cycleCreatedResources[restype] * ruler.cycleIndustryLevyPercentage;
                    ruler.cycleLeviedResources.Add(new Resource (restype, leviedAmount) );
                    pop.cycleAvailableResources[restype] -= leviedAmount;
                    ruler.resourcePortfolio[restype].amount += leviedAmount;
                }
            }           
        }
    }
    void ClearStepVariables(Ruler ruler)
    {
        ruler.cycleLeviedResources = new List<Resource>();
    }
}
