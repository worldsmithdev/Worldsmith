using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Handles selection of character for Section-view. Handles fetching of characters. 

    public static CharacterController Instance { get; protected set; }



    Character selectedCharacter;

    private void Awake()
    {
        Instance = this;
    }


    // Fetches characters based on x and ypos. Might change this method in the future. 
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
