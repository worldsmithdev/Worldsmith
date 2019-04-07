using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuilder : MonoBehaviour
{
    
    public CharacterImports imports;

    public void CreateCharacters(World activeWorld)
    { 
        foreach (CharacterImportsData data in imports.dataArray)
        {
            if (data.Name != "")
            {
                Character newChar = new Character(data.Name);
                activeWorld.characterList.Add(newChar);
                newChar.description = data.Description;
                newChar.age = data.Age;
                newChar.SetLocation(data.Location);
            } 
        } 
    }

}
