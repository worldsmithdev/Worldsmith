using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance { get; protected set; }


    Character selectedCharacter;

    private void Awake()
    {
        Instance = this;
    }


    public List<Character> GetCharactersAtLocation(Location loc)
    {
        List<Character> returnList = new List<Character>();
        foreach (Character character in WorldController.Instance.GetWorld().characterList)        
            if (character != null)            
                if (character.GetPositionVector().x == loc.GetPositionVector().x && character.GetPositionVector().y == loc.GetPositionVector().y)
                    returnList.Add(character);    
        return returnList;
    }

    public void SetSelectedCharacter (Character character)
    {
        selectedCharacter = character;
        UIController.Instance.RefreshUI();
    }
    public Character GetSelectedCharacter( )
    {
        return selectedCharacter;
    }
}
