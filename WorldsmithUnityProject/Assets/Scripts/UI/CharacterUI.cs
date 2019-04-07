using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    // Links UI components and functions relating to UI - specific to Character Section

    Character selectedCharacter;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public Image characterPicture;

    public void OpenSection()
    {
        if (CharacterController.Instance.GetSelectedCharacter() != null)
        {
            selectedCharacter = CharacterController.Instance.GetSelectedCharacter();
            nameText.text = "" + selectedCharacter.elementID;
            infoText.text = selectedCharacter.age + " Years Old\n" + selectedCharacter.description;            
            string path = "Images/Characters/" + selectedCharacter.elementID;
            Sprite charSprite = Resources.Load<Sprite>(path);
            if (charSprite != null)
            { 
                characterPicture.gameObject.SetActive(true); 
                characterPicture.sprite = charSprite;
                characterPicture.SetNativeSize();
            } 
            else
                characterPicture.gameObject.SetActive(false); 
        }
        else
        {
            nameText.text = "Name";
            infoText.text = "Info";
            characterPicture.gameObject.SetActive(false);
        } 
    }
    public void CloseSection()
    {

    }

}
