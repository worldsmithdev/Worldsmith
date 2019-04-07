using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlock : MonoBehaviour
{
    // Interactable object created for Tiles in a TileMap.
    public int xCoord;
    public int yCoord;
    public Tile linkedTile; 


    private void OnMouseOver()
    {
       TileMapController.Instance.tileMapHandler.HandleTileHover(linkedTile);

    }
    private void OnMouseExit()
    {

    }
    private void OnMouseUp()
    {
       TileMapController.Instance.tileMapHandler.HandleTileClick(linkedTile); 
    }
}
