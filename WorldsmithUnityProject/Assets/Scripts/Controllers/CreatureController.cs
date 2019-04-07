using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public static CreatureController Instance { get; protected set; }


    Creature clickedCreature;

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

    public void SetClickedCreature(Creature creature)
    {
        clickedCreature = creature;
    }
}
