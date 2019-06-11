using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsController : MonoBehaviour
{
    // Drives world simulation through a divison of eight steps, each of which can be filled with custom fucntionality

    public static StepsController Instance { get; protected set; }

    public GenerationStep generationStep;
    public LocalPoliticsStep localPoliticsStep;
    public PopulationStep populationStep;    
    public IndustryStep industryStep;    
    public ConstructionStep constructionStep;
    public RegionalPoliticsStep regionalPoliticsStep;
    public ExchangeStep exchangeStep;
    public DestructionStep destructionStep;

    private void Awake()
    {
        Instance = this;
    }

    public void FullCycle()
    {
        CycleSteps();
        CycleEvents(); 
    }

    public void CycleSteps()
    { 

        // Generation
        foreach (Territory ecoblock in EconomyController.Instance.territoryDictionary.Keys)
            generationStep.ConfigureStep(ecoblock);
        foreach (Territory ecoblock in EconomyController.Instance.territoryDictionary.Keys)
            generationStep.CycleStep(ecoblock);
        foreach (Territory ecoblock in EconomyController.Instance.territoryDictionary.Keys)
            generationStep.ResolveStep(ecoblock);

        // Industry
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            industryStep.ConfigureStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            industryStep.CycleStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            industryStep.ResolveStep(ecoblock);

        // LocalPolitics
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            localPoliticsStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            localPoliticsStep.CycleStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            localPoliticsStep.ResolveStep(ecoblock);

        // Population
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            populationStep.ConfigureStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            populationStep.CycleStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            populationStep.ResolveStep(ecoblock); 

        // Construction
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            constructionStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            constructionStep.CycleStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            constructionStep.ResolveStep(ecoblock);

        // RegionalPolitics
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.CycleStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.ResolveStep(ecoblock);

        // Exchange
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            exchangeStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            exchangeStep.CycleStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            exchangeStep.ResolveStep(ecoblock);

        // Destruction
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.ConfigureStep(ecoblock);
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.CycleStep(ecoblock);
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.ResolveStep(ecoblock);
    }

    public void CycleEvents()
    {
        // Additional events that are not dictated by EcoBlock cycling eg. natural disaster


    }


}
