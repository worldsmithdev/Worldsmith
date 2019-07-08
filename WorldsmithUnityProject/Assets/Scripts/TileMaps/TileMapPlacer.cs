using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapPlacer : MonoBehaviour
{
    // Places and scales TileMaps to their outlines and holders, which are preset here

    [HideInInspector]
    public GameObject activeOutline = null;
    [HideInInspector]
    public GameObject activeHolder = null;

    public Canvas mainCanvas;
    public GameObject terrainTileMapHolder;
    public GameObject exploreRuleTileMapOutline;
    public GameObject exploreRuleTileMapHolder;
    public GameObject exploreElementsTileMapOutline;
    public GameObject exploreElementsTileMapHolder;
    public GameObject exploreSquareLayoutTileMapOutline;
    public GameObject exploreFlatLayoutTileMapOutline;
    public GameObject exploreLayoutTileMapHolder; 
    public GameObject exploreLocalExchangeTileMapOutline; 
    public GameObject exploreLocalExchangeTileMapHolder;
    public GameObject exploreRegionalExchangeTileMapOutline;
    public GameObject exploreRegionalExchangeTileMapHolder;
    public GameObject exploreGlobalExchangeTileMapOutline;
    public GameObject exploreGlobalExchangeTileMapHolder;
    public GameObject locationSectionSquareLayoutOutline; 
    public GameObject locationSectionFlatLayoutOutline; 
    public GameObject locationSectionLayoutHolder;  

    float xBackgroundSize;
    float yBackgroundSize;
    float xStartPos;
    float yStartPos;
    float xMultiplier;
    float yMultiplier;
     

  

    public void PlaceTileMap (TileMap givenMap)
    {
        SetActiveGameobjects(givenMap);

        xBackgroundSize = activeOutline.GetComponent<SpriteRenderer>().bounds.size.x;
        yBackgroundSize = activeOutline.GetComponent<SpriteRenderer>().bounds.size.y;
        xStartPos = activeOutline.transform.position.x;
        yStartPos = activeOutline.transform.position.y;
        xMultiplier = xBackgroundSize / givenMap.xSize;
        yMultiplier = yBackgroundSize / givenMap.ySize;
      
        activeHolder.transform.localScale = Vector3.one;
        activeHolder.transform.localScale = new Vector3(activeHolder.transform.localScale.x * xMultiplier, activeHolder.transform.localScale.y * yMultiplier, 1);
        float xOffset = activeHolder.transform.localScale.x / 2;
        float yOffset = activeHolder.transform.localScale.y / 2;
        activeHolder.transform.position = new Vector3(xStartPos - (xBackgroundSize / 2) + xOffset, yStartPos - (yBackgroundSize / 2) + yOffset, 1); 
    }
    public void SetActiveGameobjects(TileMap givenMap)
    {
        if (givenMap.tileMapCategory == TileMap.TileMapCategory.Template)
        {
            if (UIController.Instance.currentSection == UIController.Section.World)
            {
                if (givenMap.xSize == givenMap.ySize)
                    activeOutline = exploreSquareLayoutTileMapOutline;
                else
                    activeOutline = exploreFlatLayoutTileMapOutline;
                activeHolder = exploreLayoutTileMapHolder;
            }
            else if (UIController.Instance.currentSection == UIController.Section.Location)
            {
                if (givenMap.xSize == givenMap.ySize)
                    activeOutline = locationSectionSquareLayoutOutline;
                else
                    activeOutline = locationSectionFlatLayoutOutline;
                activeHolder = locationSectionLayoutHolder;
            }
        }
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Terrain)
        {
            activeOutline = MapController.Instance.GetActiveMapHolder();
            activeHolder = terrainTileMapHolder;
        }
        else if (givenMap.tileMapName == "Rule")
        {
            activeOutline = exploreRuleTileMapOutline;
            activeHolder = exploreRuleTileMapHolder;
        }
        else if (givenMap.tileMapName == "Elements")
        {
            activeOutline = exploreElementsTileMapOutline;
            activeHolder = exploreElementsTileMapHolder;
        }
        else if (givenMap.tileMapName == "LocalExchanges")
        {
            activeOutline = exploreLocalExchangeTileMapOutline;
            activeHolder = exploreLocalExchangeTileMapHolder;
        }
        else if (givenMap.tileMapName == "RegionalExchanges")
        {
            activeOutline = exploreRegionalExchangeTileMapOutline;
            activeHolder = exploreRegionalExchangeTileMapHolder;
        }
        else if (givenMap.tileMapName == "GlobalExchanges")
        {
            activeOutline = exploreGlobalExchangeTileMapOutline;
            activeHolder = exploreGlobalExchangeTileMapHolder;
        }
    }
    public GameObject GetHolderForTileMap (TileMap givenMap)
    { 
        SetActiveGameobjects(givenMap);
        if (activeHolder != null)
            return activeHolder;
        else
            return activeOutline;
    }
}
