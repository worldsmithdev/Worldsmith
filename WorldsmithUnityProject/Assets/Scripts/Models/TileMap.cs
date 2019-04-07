using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New TileMap", menuName = "TileMap")]

public class TileMap : ScriptableObject
{

    public enum TileMapCategory { Terrain, Schematic, Dynamic, Template  } 


    [SerializeField]
    public string tileMapName;
    [SerializeField]
    public TileMapCategory tileMapCategory;
    [SerializeField]
    public int xSize;
    [SerializeField]
    public int ySize; 
    [SerializeField]
    public Texture2D inputImage;
    [SerializeField]
    public bool onlyUsedInUICanvas;

    public Tile[,] tilesArray;


    

    public void CreateEmptyTiles()
    {
        tilesArray = new Tile[xSize, ySize];
        for (int x = 0; x < xSize; x++)        
            for (int y = 0; y < ySize; y++) 
                tilesArray[x, y] = new Tile(x, y, tileMapName);
    }


    public Tile GetTileAt(int x, int y)
    {
        return tilesArray[x, y];
    }

}
