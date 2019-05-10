using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    public static PopulationController Instance { get; protected set; }

    Population selectedPopulation;

    private void Awake()
    {
        Instance = this;
    }


    public List<Population> GetPopulationsAtLocation(Location loc)
    {
        List<Population> returnList = new List<Population>();
        foreach (Population population in EconomyController.Instance.populationDictionary.Keys)
            if (population != null)
            {
                if (population.xLocation == loc.GetPositionVector().x && population.yLocation == loc.GetPositionVector().y)
                    returnList.Add(population);
            }


        return returnList;
    }

    public void SetSelectedPopulation(Population population)
    {
        selectedPopulation = population;
    }

    public Population GetSelectedPopulation()
    {
        return selectedPopulation;
    }
}

