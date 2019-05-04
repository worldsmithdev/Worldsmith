using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftUI : MonoBehaviour
{
    // Any text and UI functionality relating to any of the content in any of the sub panels of the Left panel

    public Button terrainDataCreateButton;
    public Button terrainDataDestroyButton;
     

    bool terrainDataMapActive = false;
     
    public void RefreshUI()
    {
        RefreshWorld1Sub2();
    }
    void RefreshWorld1Sub2()
    {
        if (TileMapController.Instance.HasTerrainTileMap() == true )
        {
            terrainDataCreateButton.interactable = true;
            terrainDataCreateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        }
        if (TileMapController.Instance.HasTerrainTileMap() == false || terrainDataMapActive == true)
        {
            terrainDataCreateButton.interactable = false;
            terrainDataCreateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 85); 
        } 
        if (terrainDataMapActive == true)
        {
            terrainDataDestroyButton.interactable = true;
            terrainDataDestroyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        }
        else
        {
            terrainDataDestroyButton.interactable = false;
            terrainDataDestroyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 85);
        }
    }
     

    // World 1 Sub 2
    public void CreateTerrainDataTileMap()
    {
        terrainDataMapActive = true;
        TileMapController.Instance.CreateTerrainTileMap();
        RefreshUI();
    }
    public void DestroyTerrainDataTileMap()
    {
        terrainDataMapActive = false;
        TileMapController.Instance.DestroyTerrainTileMaps();
        RefreshUI();
    }

     

}
