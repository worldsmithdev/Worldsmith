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

    }
    void LevyGeneration(Ruler ruler)
    {

    }
    void ClearStepVariables(Ruler ruler)
    { 

    }
}
