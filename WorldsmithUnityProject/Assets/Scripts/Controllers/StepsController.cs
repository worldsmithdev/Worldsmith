using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsController : MonoBehaviour
{
    // Drives world simulation through a divison of ten steps, each of which can be filled with custom fucntionality

    public static StepsController Instance { get; protected set; }

    public GenerationStep generationStep;
    public IndustryStep industryStep;
    public LocalPoliticsStep localPoliticsStep;
    public LocalExchangeStep localExchangeStep;
    public PopulationStep populationStep;
    public ConstructionStep constructionStep;
    public RegionalPoliticsStep regionalPoliticsStep;
    public RegionalExchangeStep regionalExchangeStep;
    public DestructionStep destructionStep;
    public GlobalExchangeStep globalExchangeStep;

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

        // TEMPORARY STIPEND
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            if (ecoblock.isLocalRuler)
                ecoblock.resourcePortfolio[Resource.Type.Silver].amount += 52000f;

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



        // LocalExchange
        localExchangeStep.PrepareStep();
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            localExchangeStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            localExchangeStep.ConfigureStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            localExchangeStep.CycleStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys) 
            localExchangeStep.CycleStep(ecoblock);  
        localExchangeStep.ResolveStep(); 
  

        // RegionalPolitics
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalPoliticsStep.CycleStep(ecoblock);
       
            regionalPoliticsStep.ResolveStep();

        // RegionalExchange

        // wares, then food, then silver if profitable
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            regionalExchangeStep.ConfigureStep(ecoblock);
        foreach (Population ecoblock in EconomyController.Instance.populationDictionary.Keys)
            regionalExchangeStep.CycleStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalExchangeStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            regionalExchangeStep.CycleStep(ecoblock);

        regionalExchangeStep.ResolveStep();
         

        // Destruction
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.ConfigureStep(ecoblock);
        foreach (Warband ecoblock in EconomyController.Instance.warbandDictionary.Keys)
            destructionStep.CycleStep(ecoblock);
       
            destructionStep.ResolveStep();

        // GlobalExchange
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            globalExchangeStep.ConfigureStep(ecoblock);
        foreach (Ruler ecoblock in EconomyController.Instance.rulerDictionary.Keys)
            globalExchangeStep.CycleStep(ecoblock);

        globalExchangeStep.ResolveStep();



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
