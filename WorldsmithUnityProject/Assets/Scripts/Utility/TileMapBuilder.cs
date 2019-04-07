using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapBuilder : MonoBehaviour
{
    // Handles all building either here or by calling TileMapSpecifics. Here: Creates Tiles for Terrain and Schematic types; creates TileBlocks for all TileMap types

    public Canvas mainCanvas;
     
    public TileBlock tileBlockPrefab;
     
    public Color32 seaColor = new Color32(); 
    public Color32 landColor = new Color32();
    public Color32 mountainColor = new Color32();
    public Color32 forestColor = new Color32();

    Color32 colorInputRed;
     Color32 colorInputYellow;
     Color32 colorInputOrange;
     Color32 colorInputBrown;
     Color32 colorInputLightgreen;
     Color32 colorInputDarkgreen;
     Color32 colorInputLightblue;
     Color32 colorInputDarkblue;
     Color32 colorInputPurple;
     Color32 colorInputBlack;
     Color32 colorInputWhite;
     Color32 colorInputGrey;
     Color32 colorInputDarkRed;

    Dictionary<World.WorldElement, Sprite> worldElementSpritePairings = new Dictionary<World.WorldElement, Sprite>();
    Dictionary<EcoBlock.BlockType, Sprite> ecoBlockSpritePairings = new Dictionary<EcoBlock.BlockType, Sprite>();
    Dictionary<Ruler.Hierarchy, Sprite> ecoBlockHierarchySpritePairings = new Dictionary<Ruler.Hierarchy, Sprite>();

    public Dictionary<Color32, Building.BuildingType> buildingTemplateInputColorPairing = new Dictionary<Color32, Building.BuildingType>();
    public Dictionary< Building.BuildingType, Sprite> buildingTemplateOutputSpritePairing = new Dictionary<Building.BuildingType, Sprite>();



    private void Start()
    { 

        colorInputRed = new Color32(255, 0, 0, 255);
        colorInputYellow = new Color32(255, 255, 0, 255);
        colorInputOrange = new Color32(243, 108, 79, 255);
        colorInputBrown = new Color32(96, 57, 19, 255);
        colorInputLightgreen = new Color32(0, 255, 0, 255);
        colorInputDarkgreen = new Color32(0, 166, 81, 255);
        colorInputLightblue = new Color32(0, 255, 255, 255);
        colorInputDarkblue = new Color32(0, 0, 255, 255);
        colorInputPurple = new Color32(255, 0, 255, 255);
        colorInputBlack = new Color32(0, 0, 0, 255);
        colorInputWhite = new Color32(255, 255, 255, 255);
        colorInputGrey = new Color32(149, 149, 149, 255);
        colorInputDarkRed = new Color32(121, 0, 0, 255);
         
        buildingTemplateInputColorPairing.Add(colorInputLightgreen, Building.BuildingType.Granary); 
         
        buildingTemplateOutputSpritePairing.Add(Building.BuildingType.Granary, SpriteCollection.Instance.colorTileYellow);  

        worldElementSpritePairings.Add(World.WorldElement.Unassigned, null);
        worldElementSpritePairings.Add(World.WorldElement.Character, SpriteCollection.Instance.schematicCharacterSprite);
        worldElementSpritePairings.Add(World.WorldElement.Creature, SpriteCollection.Instance.schematicCreatureSprite);
        worldElementSpritePairings.Add(World.WorldElement.Item, SpriteCollection.Instance.schematicItemSprite);
        worldElementSpritePairings.Add(World.WorldElement.Location, SpriteCollection.Instance.locationSubVillage);

        ecoBlockSpritePairings.Add(EcoBlock.BlockType.Unassigned, null);
        ecoBlockSpritePairings.Add(EcoBlock.BlockType.Ruler, SpriteCollection.Instance.schematicSecondarySprite);
        ecoBlockSpritePairings.Add(EcoBlock.BlockType.Population, SpriteCollection.Instance.schematicPopulationSprite);
        ecoBlockSpritePairings.Add(EcoBlock.BlockType.Territory, SpriteCollection.Instance.schematicTerritorySprite);
        ecoBlockSpritePairings.Add(EcoBlock.BlockType.Warband, SpriteCollection.Instance.schematicWarbandSprite);
        ecoBlockHierarchySpritePairings.Add(Ruler.Hierarchy.Dominating, SpriteCollection.Instance.schematicDominatingSprite);
        ecoBlockHierarchySpritePairings.Add(Ruler.Hierarchy.Independent, SpriteCollection.Instance.schematicIndependentSprite);
        ecoBlockHierarchySpritePairings.Add(Ruler.Hierarchy.Secondary, SpriteCollection.Instance.schematicSecondarySprite);
        ecoBlockHierarchySpritePairings.Add(Ruler.Hierarchy.Dominated, SpriteCollection.Instance.schematicDominatedSprite);
  
    }
    public void FillMapTiles (TileMap givenMap)
    { 

        if (givenMap.tileMapCategory == TileMap.TileMapCategory.Terrain)
            BuildTerrainMap(givenMap);
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Schematic)
        {
            if (givenMap.tileMapName == "Elements")
                TileMapController.Instance.tileMapSpecifics.BuildElementsMap(givenMap);
            else if (givenMap.tileMapName == "Rule")
                TileMapController.Instance.tileMapSpecifics.BuildRuleMap(givenMap);
        }
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Dynamic)
        {

        }           
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Template)
            BuildTemplateTileMap(givenMap); 

    }

    public void CreateTileBlocks (TileMap givenMap)
    {
        if (givenMap.tileMapCategory == TileMap.TileMapCategory.Terrain)
            CreateTerrainTileBlocks(givenMap);
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Schematic)
            CreateSchematicTileBlocks(givenMap);
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Dynamic)
            CreateDynamicTileBlocks(givenMap);
        else if (givenMap.tileMapCategory == TileMap.TileMapCategory.Template)
            CreateTemplateTileBlocks(givenMap);
    }


    void BuildTerrainMap(TileMap givenMap)
    {
        // Takes the TileMaps' InputImage and creates an array of colors from its pixels.
        // Loops through that array, adjusting for x and y position, and checks the color of the pixel against the color presets.
        // Checks each pixel; you can speed this up, perhaps losing accuracy, by increasing the for loops' variable increase. 
        Texture2D inputMap = givenMap.inputImage;
        Color32[] allPixels = inputMap.GetPixels32();

        for (int x = 0; x < inputMap.width; x += 1)
        {
            for (int y = 0; y < inputMap.height; y += 1)
            {
                Color32 col = allPixels[(y * inputMap.width) + x];
                if (col.Equals(landColor))
                    givenMap.GetTileAt(x / 10, y / 10).SetTileTerrainType(Tile.TileTerrainType.Land);
                else if (col.Equals(seaColor))
                    givenMap.GetTileAt(x / 10, y / 10).SetTileTerrainType(Tile.TileTerrainType.Sea);
                else if (col.Equals(mountainColor))
                    givenMap.GetTileAt(x / 10, y / 10).SetTileTerrainType(Tile.TileTerrainType.Mountain);
                else if (col.Equals(forestColor))
                    givenMap.GetTileAt(x / 10, y / 10).SetTileTerrainType(Tile.TileTerrainType.Forest);
            }
        }
    }

    void BuildTemplateTileMap(TileMap givenMap)
    {
        // Reads pixels like the BuildTerrainMap method.
        // Does some unwieldy math to account for being able to enter both square and rectangle ('flat')  maps. 
        Texture2D inputMap = givenMap.inputImage;
        Color32[] allPixels = inputMap.GetPixels32();

        bool flatMap;
        if (givenMap.xSize == givenMap.ySize)
            flatMap = false;
        else
            flatMap = true;  
        int sizeDividerX;
        int sizeDividerY;
        int stepsIncreaseX;
        int stepsIncreaseY;
        if (flatMap == false)
        {
            sizeDividerX = 1000 / givenMap.xSize;
            sizeDividerY = 1000 / givenMap.ySize;
            stepsIncreaseX = 10;
            stepsIncreaseY = 10;
        }
        else
        {
            sizeDividerX = 2000 / givenMap.xSize;
            sizeDividerY = 1300 / givenMap.ySize;
            stepsIncreaseX = 10;
            stepsIncreaseY = 10;
        }

        for (int x = 0; x < inputMap.width; x += stepsIncreaseX)
        {
            for (int y = 0; y < inputMap.height; y += stepsIncreaseY)
            {
                Color32 col = allPixels[(y * inputMap.width) + x];

                bool pairingFound = false;
                foreach (Color32 listedcol in  buildingTemplateInputColorPairing.Keys)
                {
                    if (listedcol.Equals(col))
                    {
                        givenMap.GetTileAt(x / sizeDividerX, y / sizeDividerY).SetTileBuildingType(buildingTemplateInputColorPairing[col]); 
                        pairingFound = true;
                    }
                }
                if (pairingFound == false)
                    givenMap.GetTileAt(x / sizeDividerX, y / sizeDividerY).SetTileBuildingType(Building.BuildingType.Unassigned);
            }
        }
    }


    public void CreateTerrainTileBlocks (TileMap givenMap)
    {
        for (int x = 0; x < givenMap.xSize; x++)
        {
            for (int y = 0; y < givenMap.ySize; y++)
            {
                Tile linkedTile = givenMap.GetTileAt(x, y);
                GameObject holder = TileMapController.Instance.tileMapPlacer.GetHolderForTileMap(givenMap);
                TileBlock tileBlock = Instantiate(tileBlockPrefab);
                GameObject blockObj = tileBlock.gameObject; 
                blockObj.transform.SetParent(holder.transform, false);
                tileBlock.xCoord = x;
                tileBlock.yCoord = y;
                tileBlock.linkedTile = linkedTile;
                blockObj.name = "TileBlock X" + x + " Y" + y;
                blockObj.GetComponent<SpriteRenderer>().sortingLayerName = "TileBlockBelowUI";         
                TileMapController.Instance.activeTerrainTileMapBlocksList.Add(blockObj); 
                blockObj.transform.localPosition = new Vector3(x  , y  , 0);

                if (linkedTile.tileTerrainType == Tile.TileTerrainType.Sea)
                    blockObj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.seaSprite;
                else if (linkedTile.tileTerrainType == Tile.TileTerrainType.Land)
                    blockObj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.landSprite;
                else if (linkedTile.tileTerrainType == Tile.TileTerrainType.Mountain)
                    blockObj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.mountainSprite;
                else if (linkedTile.tileTerrainType == Tile.TileTerrainType.Forest)
                    blockObj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.forestSprite;
            }
        }
    }
    public void CreateSchematicTileBlocks(TileMap givenMap)
    {
        for (int x = 0; x < givenMap.xSize; x++)
        {
            for (int y = 0; y < givenMap.ySize; y++)
            {
                Tile linkedTile = givenMap.GetTileAt(x, y);
                GameObject holder = TileMapController.Instance.tileMapPlacer.GetHolderForTileMap(givenMap);
                bool tileDrawn = false; 
                EcoBlock linkedEB = linkedTile.linkedEcoBlock;
                TileBlock tileBlock = Instantiate(tileBlockPrefab);
                GameObject blockObj = tileBlock.gameObject;                
                blockObj.transform.SetParent(holder.transform );
                tileBlock.xCoord = x;
                tileBlock.yCoord = y;
                tileBlock.linkedTile = linkedTile;
                blockObj.name = "TileBlock X" + x + " Y" + y;
                blockObj.GetComponent<SpriteRenderer>().sortingLayerName = "TileBlockAboveUI";    
                if (givenMap.onlyUsedInUICanvas == true)
                {
                    blockObj.transform.localPosition = new Vector3(x / mainCanvas.transform.localScale.x, y / mainCanvas.transform.localScale.y, 0);
                    blockObj.transform.localScale = new Vector3(20 / holder.transform.localScale.x, 20 / holder.transform.localScale.y, 0);
                }
                else 
                    blockObj.transform.localPosition = new Vector3(x, y, 0);

                TileMapController.Instance.activeSchematicTileMapBlocksList.Add(blockObj);
                 
                foreach (World.WorldElement type in worldElementSpritePairings.Keys)
                    if (type != World.WorldElement.Unassigned && tileDrawn == false && linkedTile.tileWorldElementType == type)
                    {
                        if (type == World.WorldElement.Location)
                        {
                            if (SpriteCollection.Instance.locationSubTypePairings.ContainsKey(linkedTile.linkedLocation.GetLocationSubType()) )
                                blockObj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.locationSubTypePairings[linkedTile.linkedLocation.GetLocationSubType()];
                            else
                                blockObj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.locationTypePairings[linkedTile.linkedLocation.GetLocationType()];

                            blockObj.transform.localScale *= 3;   
                        }
                        else
                            blockObj.GetComponent<SpriteRenderer>().sprite = worldElementSpritePairings[type];
                        tileDrawn = true;
                    }
                       
                foreach (EcoBlock.BlockType type in ecoBlockSpritePairings.Keys)
                    if (type != EcoBlock.BlockType.Unassigned && tileDrawn == false && linkedTile.tileEcoBlockType == type)
                    {
                        blockObj.GetComponent<SpriteRenderer>().sprite = ecoBlockSpritePairings[type];
                        tileDrawn = true;
                    }
                if (linkedTile.tileEcoBlockType == EcoBlock.BlockType.Ruler)                
                    blockObj.GetComponent<SpriteRenderer>().sprite = ecoBlockHierarchySpritePairings[RulerController.Instance.GetRulerFromID(linkedEB.blockID).rulerHierarchy];
             }
        }
    }
    public void CreateDynamicTileBlocks(TileMap givenMap)
    {
        for (int x = 0; x < givenMap.xSize; x++)
        {
            for (int y = 0; y < givenMap.ySize; y++)
            {

            }
        }
    }
    public void CreateTemplateTileBlocks(TileMap givenMap)
    {
        for (int x = 0; x < givenMap.xSize; x++)
        {
            for (int y = 0; y < givenMap.ySize; y++)
            {
                Tile linkedTile = givenMap.GetTileAt(x, y);
                GameObject holder = TileMapController.Instance.tileMapPlacer.GetHolderForTileMap(givenMap); 
                EcoBlock linkedEB = linkedTile.linkedEcoBlock;
                TileBlock tileBlock = Instantiate(tileBlockPrefab);
                GameObject blockObj = tileBlock.gameObject;
                blockObj.transform.SetParent(holder.transform);
                tileBlock.xCoord = x;
                tileBlock.yCoord = y;
                tileBlock.linkedTile = linkedTile;
                blockObj.name = "TileBlock X" + x + " Y" + y;
                blockObj.GetComponent<SpriteRenderer>().sortingLayerName = "TileBlockAboveUI";
                //TODO: More elegant way of UI scaling than below check
                if (UIController.Instance.currentSection == UIController.Section.World && UIController.Instance.worldUI.explorePanelActive == true)
                {
                    blockObj.transform.localPosition = new Vector3(x / mainCanvas.transform.localScale.x, y / mainCanvas.transform.localScale.y, 0);
                    blockObj.transform.localScale = new Vector3(10 / holder.transform.localScale.x, 10 / holder.transform.localScale.y, 0);
                }
                else
                    blockObj.transform.localPosition = new Vector3(x, y, 0);

                TileMapController.Instance.activeTemplateTileMapBlocksList.Add(blockObj);

                foreach (Building.BuildingType sub in buildingTemplateOutputSpritePairing.Keys)
                    if (givenMap.GetTileAt(x, y).tileBuildingType == sub)
                        blockObj.GetComponent<SpriteRenderer>().sprite = buildingTemplateOutputSpritePairing[sub]; 
            }
        }
    }
    




}
