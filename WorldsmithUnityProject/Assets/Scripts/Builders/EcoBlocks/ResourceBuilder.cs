using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilder : MonoBehaviour
{
    float citizenMultiplier = 1.6f;
    Dictionary<int, float> territoryStatusFoodModifier = new Dictionary<int, float>(); // Amount of consumption cycles stored
    Dictionary<int, float> territoryStatusLuxuryModifier = new Dictionary<int, float>(); // Multiplier for other resources

    void Awake()
    {
        territoryStatusFoodModifier.Add( 1 , 2f);
        territoryStatusFoodModifier.Add( 2 , 4f);
        territoryStatusFoodModifier.Add( 3 , 6f);
        territoryStatusFoodModifier.Add( 4 , 8f);
        territoryStatusFoodModifier.Add( 5 , 12f);
        territoryStatusLuxuryModifier.Add( 1 , 0.4f);
        territoryStatusLuxuryModifier.Add( 2 , 0.8f);
        territoryStatusLuxuryModifier.Add( 3 , 1.0f);
        territoryStatusLuxuryModifier.Add( 4 , 1.2f);
        territoryStatusLuxuryModifier.Add( 5 , 1.6f);

    }
    public void BuildResources()
    {  
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
        {
            foreach (Population population in loc.populationList)
            {
                StepsController.Instance.populationStep.DetermineFoodConsumption(population);
                StepsController.Instance.populationStep.DetermineLuxuryConsumption(population);

                if (population.laborType == Population.LaborType.Leisure)
                    AssignLeisureClassResources(population);
                else if (population.laborType == Population.LaborType.Housekeeper)
                    AssignHousekeeperClassResources(population);
                else if (population.laborType == Population.LaborType.Artisan)
                    AssignArtisanClassResources(population);
            }
            AssignLocalRulerResources(loc.localRuler);
            foreach ( Ruler ruler in loc.secondaryRulers)            
                if (ruler != null)
                    AssignSecondaryRulerResources(ruler);
            
        }
    
    }
    void AssignLocalRulerResources (Ruler ruler)
    {

    }
    void AssignSecondaryRulerResources(Ruler ruler)
    {

    }
    void AssignLeisureClassResources (Population population)
    {
        population.resourcePortfolio[Resource.Type.Wheat].amount += (population.cycleDesiredFoodConsumption * territoryStatusFoodModifier[population.GetHomeLocation().territoryStatus]) * 2;
        population.resourcePortfolio[Resource.Type.Wares].amount += (population.cycleDesiredLuxuryConsumption * territoryStatusLuxuryModifier[population.GetHomeLocation().territoryStatus]) * 2;
        population.resourcePortfolio[Resource.Type.Silver].amount += (population.amount * 5f);
    }
    void AssignHousekeeperClassResources(Population population)
    {
        population.resourcePortfolio[Resource.Type.Wheat].amount += (population.cycleDesiredFoodConsumption * territoryStatusFoodModifier[population.GetHomeLocation().territoryStatus] ) * 2;
        population.resourcePortfolio[Resource.Type.Wares].amount += (population.cycleDesiredLuxuryConsumption * territoryStatusLuxuryModifier[population.GetHomeLocation().territoryStatus] ) /2 ;
        if (population.classType == Population.ClassType.Citizen)
        {
            population.resourcePortfolio[Resource.Type.Wheat].amount *= citizenMultiplier;
            population.resourcePortfolio[Resource.Type.Wares].amount *= citizenMultiplier;
        }
    }
    void AssignArtisanClassResources(Population population)
    {
        population.resourcePortfolio[Resource.Type.Wheat].amount += (population.cycleDesiredFoodConsumption * territoryStatusFoodModifier[population.GetHomeLocation().territoryStatus]) / 2 ;
        population.resourcePortfolio[Resource.Type.Wares].amount += (population.cycleDesiredLuxuryConsumption * territoryStatusLuxuryModifier[population.GetHomeLocation().territoryStatus]) * 2;
    }

}
