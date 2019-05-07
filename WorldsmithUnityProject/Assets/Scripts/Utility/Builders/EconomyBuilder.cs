using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyBuilder : MonoBehaviour
{
  

    public void BuildRulers()
    {  
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)        
            if (loc.GetLocationType() == Location.LocationType.Settled)
                  BuildSettledRulers(loc);

        OrganizeRulerHierarchy(); 
    }
    void OrganizeRulerHierarchy()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
        {
            List<Location> domList = new List<Location>();
            if (loc.dominateStrings != null)
            {
                foreach (string str in loc.dominateStrings)
                    domList.Add(LocationController.Instance.GetSpecificLocation(str));

                foreach (Location domloc in domList)
                {
                    if (domloc.localRuler != null)
                        EconomyController.Instance.rulerDictionary[domloc.localRuler] = loc.localRuler;
                    else
                        domloc.dominatingRuler = loc.localRuler;
                }   
            } 
        } 
        foreach (Ruler ruler in EconomyController.Instance.rulerDictionary.Keys)
        {
            if (ruler.rulerHierarchy ==Ruler.Hierarchy.Unassigned)
            {
                foreach (Ruler domruler in ruler.GetControlledRulers())
                    if (domruler != null)
                        if (domruler.isLocalRuler == true) 
                            ruler.rulerHierarchy = Ruler.Hierarchy.Dominating;  
            }

            if (ruler.rulerHierarchy ==Ruler.Hierarchy.Unassigned)
            {
                if (EconomyController.Instance.rulerDictionary[ruler] == ruler || EconomyController.Instance.rulerDictionary[ruler] == null)
                    if (ruler.isLocalRuler == true)
                        ruler.rulerHierarchy =Ruler.Hierarchy.Independent;
                    else
                        ruler.rulerHierarchy =Ruler.Hierarchy.Secondary;
            }
            if (ruler.rulerHierarchy ==Ruler.Hierarchy.Unassigned)
            {
                if (EconomyController.Instance.rulerDictionary[ruler] != ruler && EconomyController.Instance.rulerDictionary[ruler] != null)
                    ruler.rulerHierarchy =Ruler.Hierarchy.Dominated;
            } 
        } 
    }
    public void BuildWarbands()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled) 
                    BuildSettledWarbands(loc);
    }
    public void BuildTerritories()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled) 
                    BuildSettledTerritory(loc);
    }

    public void BuildPopulations()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled) 
                    BuildSettledPopulations(loc);
    }
    public void BuildResources()
    { 
    }


    void BuildSettledRulers (Location loc)
    {          
        Ruler ruler = new Ruler(loc, true); 
        EconomyController.Instance.rulerDictionary.Add(ruler, null);
        if (loc.GetLocationSubType() == Location.LocationSubType.Polis)
        {
            Ruler secondaryRuler = new Ruler(loc, false);
            EconomyController.Instance.rulerDictionary.Add(secondaryRuler, null);
        }
    }
    void BuildSettledWarbands(Location loc)
    {
        Warband  warband = new Warband(loc);
        EconomyController.Instance.warbandDictionary.Add(warband, null);
    }
    void BuildSettledTerritory(Location loc)
    {
        Territory terr = new Territory(loc);
        EconomyController.Instance.territoryDictionary.Add(terr, null);
    }
    void BuildSettledPopulations(Location loc)
    { 
        Population pop = new Population(loc);
        EconomyController.Instance.populationDictionary.Add(pop, null);
    }
}
