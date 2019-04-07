using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableValueStore : MonoBehaviour
{
    // Reference file for descriptions of each value of each variable in the database, as well as images. 
     

    public static VariableValueStore Instance { get; protected set; }

    public Dictionary<int, Sprite> politicalTypeSpritePairings = new Dictionary<int, Sprite>(); 

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        politicalTypeSpritePairings.Add(1, SpriteCollection.Instance.politicalSprite1);
        politicalTypeSpritePairings.Add(2, SpriteCollection.Instance.politicalSprite2);
        politicalTypeSpritePairings.Add(3, SpriteCollection.Instance.politicalSprite3);
        politicalTypeSpritePairings.Add(4, SpriteCollection.Instance.politicalSprite4);
        politicalTypeSpritePairings.Add(5, SpriteCollection.Instance.politicalSprite5); 
    }

    public string GetVariableRecord(WorldElement worldElement, string variableName, int variableValue)
    {
        if (worldElement.elementType == WorldElement.ElementType.Location)
        {
            if (variableName == "PoliticalType")
            {
                if (variableValue == 1)
                    return "PoliticalType 1";
                if (variableValue == 2)
                    return "PoliticalType 2";
                if (variableValue == 3)
                    return "PoliticalType 3";
                if (variableValue == 4)
                    return "PoliticalType 4";
                if (variableValue == 5)
                    return "PoliticalType 5";
            } 
        }
        else if (worldElement.elementType == WorldElement.ElementType.God)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Item)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Character)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Creature)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Faction)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Law)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Story)
        {

        }

        return ("Variable " + variableName + " of WorldElementType " + worldElement.elementType + " has no text record for value: " + variableValue);
    }

    public Sprite GetVariableSprite(WorldElement worldElement, string variableName, int variableValue)
    {

        if (worldElement.elementType == WorldElement.ElementType.Location)
        {
            if (variableName == "PoliticalType")
                if (politicalTypeSpritePairings.ContainsKey(variableValue))
                    return politicalTypeSpritePairings[variableValue]; 

        }
        else if (worldElement.elementType == WorldElement.ElementType.God)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Item)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Character)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Creature)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Faction)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Law)
        {

        }
        else if (worldElement.elementType == WorldElement.ElementType.Story)
        {

        }

        return null;
    }
}

