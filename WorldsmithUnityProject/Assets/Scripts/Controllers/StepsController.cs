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
    
            generationStep.ResolveStep();

        // Industry
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            industryStep.ConfigureStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            industryStep.CycleStep(ecoblock);
        
            industryStep.ResolveStep();

        // LocalPolitics
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            localPoliticsStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            localPoliticsStep.CycleStep(ecoblock);
        
            localPoliticsStep.ResolveStep();

        // Population
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            populationStep.ConfigureStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            populationStep.CycleStep(ecoblock);
        
            populationStep.ResolveStep();

        // Construction
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            constructionStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            constructionStep.CycleStep(ecoblock);
       
            constructionStep.ResolveStep();

        // RegionalPolitics
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.CycleStep(ecoblock);
       
            regionalPoliticsStep.ResolveStep();

        // Exchange
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            exchangeStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            exchangeStep.CycleStep(ecoblock);
        
            exchangeStep.ResolveStep();

        // Destruction
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.ConfigureStep(ecoblock);
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.CycleStep(ecoblock);
       
            destructionStep.ResolveStep();

        //// Each
        //foreach (Territory ecoblock in EconomyController.Instance.territoryDictionary.Keys)
        //    generationStep.ResolveStep(ecoblock);
        //foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
        //    industryStep.ResolveStep(ecoblock);
        //foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
        //    localPoliticsStep.ResolveStep(ecoblock);
        //foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
        //    populationStep.ResolveStep(ecoblock);
        //foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
        //    constructionStep.ResolveStep(ecoblock);
        //foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
        //    regionalPoliticsStep.ResolveStep(ecoblock);
        //foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
        //    exchangeStep.ResolveStep(ecoblock);
        //foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
        //    destructionStep.ResolveStep(ecoblock);
    }

    public void CycleEvents()
    {
        // Additional events that are not dictated by EcoBlock cycling eg. natural disaster


    }


}
