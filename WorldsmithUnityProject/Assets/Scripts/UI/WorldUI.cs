using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldUI : MonoBehaviour
{
    // Links UI components and functions relating to UI - specific to World Section

    public GameObject savePanel;
    public GameObject explorePanel;
     
    public Button saveButton;

    [HideInInspector]
    public bool savePanelActive = false;
    [HideInInspector]
    public bool explorePanelActive = false;


    private void Start()
    {
        savePanel.SetActive(false);
        explorePanel.SetActive(false);

    }
    public void OpenSection()
    { 
        saveButton.gameObject.SetActive(true);
    }
    public void CloseSection()
    { 
        saveButton.gameObject.SetActive(false);
        ClosePanels();
    }

    public void ToggleSaveMenu()
    {
        if (savePanelActive == true)
        {
            savePanel.SetActive(false);
            savePanelActive = false;
        }
        else
        {
            WorldSaver.Instance.RefreshSaveUI();
            savePanel.SetActive(true);
            savePanelActive = true;
        }

    }

    public void ToggleExplorePanel()
    {
        if (explorePanelActive == true)
        {
            UIController.Instance.exploreUI.RefreshClose();
            explorePanel.SetActive(false);
            explorePanelActive = false;
        }
        else
        {
            UIController.Instance.exploreUI.RefreshUI();
            explorePanel.SetActive(true);
            explorePanelActive = true;
        }
    }
    public void OpenExplorePanel()
    {
        UIController.Instance.exploreUI.RefreshUI();
        explorePanel.SetActive(true);
        explorePanelActive = true;
    }
    public void CloseExplorePanel()
    {
        UIController.Instance.exploreUI.RefreshClose();
        explorePanel.SetActive(false);
        explorePanelActive = false;
    }
  
    public void ClosePanels()
    { 
        if (explorePanelActive == true)
              UIController.Instance.exploreUI.RefreshClose();
        explorePanel.SetActive(false);
        explorePanelActive = false;
        savePanel.SetActive(false);
        savePanelActive = false;
    }

}
