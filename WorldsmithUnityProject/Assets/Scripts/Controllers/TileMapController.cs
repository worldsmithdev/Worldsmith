using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapController : MonoBehaviour
{
    // Handles holding, finding, and creation of Tile Maps. 

    public static TileMapController Instance { get; protected set; }

    public TileMapBuilder tileMapBuilder;
    public TileMapPlacer tileMapPlacer;
    public TileMapHandler tileMapHandler;
    public TileMapSpecifics tileMapSpecifics;
    public List<TileMap> tileMapList = new List<TileMap>();

  
    [HideInInspector]
    public List<GameObject> activeTerrainTileMapBlocksList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> activeSchematicTileMapBlocksList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> activeDynamicTileMapBlocksList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> activeTemplateTileMapBlocksList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreateTemplateTileMap(Location loc)
    {
        //Check if a specific map for the location exists. If not, check if a template exists for the location subtype. If not, no map is made. 
        bool locationTileMapFound = false;  

        foreach (TileMap map in tileMapList)
            if (map.tileMapName == loc.elementID&& map.tileMapCategory == TileMap.TileMapCategory.Template)
            { 
                locationTileMapFound = true;
                CreateTileMap(loc.elementID, map.tileMapCategory);
            }

        if (locationTileMapFound == false)        
            foreach (TileMap map in tileMapList)
                if (map.tileMapName == loc.GetLocationSubType().ToString() && map.tileMapCategory == TileMap.TileMapCategory.Template)
                    CreateTileMap(loc.GetLocationSubType().ToString(), map.tileMapCategory);  
    }

    public bool HasTerrainTileMap()
    {
        // check if active sub map has corresponding terrain tile map
        if (MapController.Instance.activeSubMap == null)
            return false;
        
        foreach (TileMap map in tileMapList)
            if ( (MapController.Instance.activeSubMap.gameObject.transform.name ) == (map.tileMapName))
                return true;
        return false;
    }
    public void CreateTerrainTileMap()
    {
        string terrainMapName = (MapController.Instance.activeSubMap.gameObject.transform.name );
        foreach (TileMap map in tileMapList)
            if (terrainMapName == (map.tileMapName))
                CreateTileMap(terrainMapName, TileMap.TileMapCategory.Terrain);
    }


    public void CreateTileMap (string mapname, TileMap.TileMapCategory mapcategory )
    { 
            TileMap createdMap = GetTileMapFromList(mapname);
            createdMap.CreateEmptyTiles();
            tileMapPlacer.PlaceTileMap(createdMap);
            tileMapBuilder.FillMapTiles(createdMap);
            tileMapBuilder.CreateTileBlocks(createdMap); 
    }
     
      

    public TileMap GetTileMapFromList(string name)
    {
        foreach (TileMap map in tileMapList)        
            if (map.tileMapName == name) return map;

        Debug.Log("Did not find TileMap in list for: " + name);
        return null;
    }

    public void DestroyAllTileMaps()
    {
        DestroyTerrainTileMaps();
        DestroySchematicTileMaps();
        DestroyDynamicTileMaps();
        DestroyTemplateTileMaps();
    }
    public void DestroyTerrainTileMaps()
    {
        foreach (GameObject obj in activeTerrainTileMapBlocksList)
            if (obj != null)
                Destroy(obj);

        activeTerrainTileMapBlocksList = new List<GameObject>(); 

    }
    public void DestroySchematicTileMaps()
    {
        foreach (GameObject obj in activeSchematicTileMapBlocksList)
            if (obj != null)
                Destroy(obj);
        activeSchematicTileMapBlocksList = new List<GameObject>();
  
    }
    public void DestroyDynamicTileMaps()
    {
        foreach (GameObject obj in activeDynamicTileMapBlocksList)
            if (obj != null)
                Destroy(obj);
        activeDynamicTileMapBlocksList = new List<GameObject>(); 
    }
    public void DestroyTemplateTileMaps()
    {
        foreach (GameObject obj in activeTemplateTileMapBlocksList)
            if (obj != null)
                Destroy(obj);
        activeTemplateTileMapBlocksList = new List<GameObject>();
    }



    public bool HasLocationTileMap(Location loc)
    {
        foreach (TileMap map in tileMapList)
            if (map.tileMapName == loc.elementID && map.tileMapCategory == TileMap.TileMapCategory.Template)
                return true;
        return false;
    }
    public bool HasTemplateTileMap(Location loc)
    {
        foreach (TileMap map in tileMapList)
            if (map.tileMapName == loc.GetLocationSubType().ToString() && map.tileMapCategory == TileMap.TileMapCategory.Template)
                return true;
        return false;
    }

    public bool HasSpecificTemplate (Location loc)
    {
        foreach (TileMap map in tileMapList)
            if (map.tileMapName == loc.elementID && map.tileMapCategory == TileMap.TileMapCategory.Template) 
                return true;
        return false;
    }

    public bool HasGeneralTemplate (Location loc)
    { 
            foreach (TileMap map in tileMapList)
                if (map.tileMapName == loc.GetLocationSubType().ToString() && map.tileMapCategory == TileMap.TileMapCategory.Template)
                    return true;
        return false;
    }

}
