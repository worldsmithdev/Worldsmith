using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerBuilder : MonoBehaviour
{
    public void BuildRulers()
    {
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (loc.GetLocationType() == Location.LocationType.Settled)
                BuildSettledRulers(loc);

        OrganizeRulerHierarchy();
    }



    void BuildSettledRulers(Location loc)
    { 
        // Create Location's primary ruler
        Ruler localRuler = new Ruler(loc, true);
        EconomyController.Instance.rulerDictionary.Add(localRuler, null);


        // Assign Directly Translated Types     
        for (int i = 0; i < 10; i++)
        {
            if (loc.authorityPolitical == i)
                localRuler.authorityType = (Ruler.AuthorityType)i;
            if (loc.attitudePolitical == i)
                localRuler.attitude = (Ruler.Attitude)i;

        }
           
        

        if (loc.GetLocationSubType() == Location.LocationSubType.Polis) 
        {
            Ruler secondaryRuler = new Ruler(loc, false); 
            EconomyController.Instance.rulerDictionary.Add(secondaryRuler, null);
        }
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
            if (ruler.rulerHierarchy == Ruler.Hierarchy.Unassigned)
            {
                foreach (Ruler domruler in ruler.GetControlledRulers())
                    if (domruler != null)
                        if (domruler.isLocalRuler == true)
                            ruler.rulerHierarchy = Ruler.Hierarchy.Dominating;
            }

            if (ruler.rulerHierarchy == Ruler.Hierarchy.Unassigned)
            {
                if (EconomyController.Instance.rulerDictionary[ruler] == ruler || EconomyController.Instance.rulerDictionary[ruler] == null)
                    if (ruler.isLocalRuler == true)
                        ruler.rulerHierarchy = Ruler.Hierarchy.Independent;
                    else
                        ruler.rulerHierarchy = Ruler.Hierarchy.Secondary;
            }
            if (ruler.rulerHierarchy == Ruler.Hierarchy.Unassigned)
            {
                if (EconomyController.Instance.rulerDictionary[ruler] != ruler && EconomyController.Instance.rulerDictionary[ruler] != null)
                    ruler.rulerHierarchy = Ruler.Hierarchy.Dominated;
            }
        }
    }



}
