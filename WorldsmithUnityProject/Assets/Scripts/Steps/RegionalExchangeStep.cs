using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalExchangeStep : Step
{
    public override void ConfigureStep(EcoBlock ecoblock)
    {
        if (ecoblock.blockType == EcoBlock.BlockType.Population)
        {
            Population population = (Population)ecoblock; 

        }
        else if (ecoblock.blockType == EcoBlock.BlockType.Ruler)
        {
            Ruler ruler = (Ruler)ecoblock; 

        }

    }

    public override void CycleStep(EcoBlock ecoblock)
    {
        if (ecoblock.blockType == EcoBlock.BlockType.Population)
        {
            Population population = (Population)ecoblock; 

        }
        else if (ecoblock.blockType == EcoBlock.BlockType.Ruler)
        {
            Ruler ruler = (Ruler)ecoblock; 

        }
    }

    public override void ResolveStep()
    {


    }
}
