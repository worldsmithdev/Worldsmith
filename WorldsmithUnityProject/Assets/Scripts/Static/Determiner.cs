using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Determiner : MonoBehaviour
{


    public static float DetermineSilverReservation(EcoBlock ecoBlock)
    {
        Ruler ruler;
        Population population;
        Warband warband;

        if (ecoBlock.blockType == EcoBlock.BlockType.Ruler)
        {
            ruler = (Ruler)ecoBlock;
            return 500f;
        }
        else if (ecoBlock.blockType == EcoBlock.BlockType.Population)
        {
            population = (Population)ecoBlock;
            return 2f * population.amount;
        }
        else if (ecoBlock.blockType == EcoBlock.BlockType.Warband)
        {
            warband = (Warband)ecoBlock;
            return 200f;
        }

        return 0f;
    }
}
