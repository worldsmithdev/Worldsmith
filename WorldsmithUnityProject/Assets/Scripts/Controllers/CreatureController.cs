using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    // Everything creature related. Which is currently not much at all


    public static CreatureController Instance { get; protected set; }


    Creature selectedCreature;

    private void Awake()
    {
        Instance = this;
    }

    public List<Creature> GetCreaturesAtLocation(Location loc)
    {
        List<Creature> returnList = new List<Creature>();
        foreach (Creature creature in WorldController.Instance.GetWorld().creatureList)
            if (creature != null)
                if (creature.GetPositionVector().x == loc.GetPositionVector().x && creature.GetPositionVector().y == loc.GetPositionVector().y)
                    returnList.Add(creature);
        return returnList;
    }

    public void SetSelectedCreature(Creature creature)
    {
        selectedCreature = creature;
    }
    public Creature GetSelectedCreature()
    {
        return selectedCreature;
    }
}
