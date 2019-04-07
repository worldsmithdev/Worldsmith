using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile  
{
    // Data container for TileMaps that kind of messily is able to hold very different datatypes. 

    public string tileName;
    public int xCoord;
    public int yCoord;

     
    public enum TileTerrainType {  Sea, Land, Mountain, Forest}
     
    public string originalTileMapName;

    public TileTerrainType tileTerrainType;
    public Building.BuildingType tileBuildingType;
    public World.WorldElement tileWorldElementType;
    public EcoBlock.BlockType tileEcoBlockType;

    public EcoBlock linkedEcoBlock;
    public Location linkedLocation;
    public Character linkedCharacter;
    public Creature linkedCreature;
    public Item linkedItem;
     
    public Tile(int xloc, int yloc, string mapname)
    {
        originalTileMapName = mapname;
        xCoord = xloc;
        yCoord = yloc;
        tileName = "Tile" + "X" + xCoord + "Y" + yCoord;
    }


    public void SetTileEcoBlockType(EcoBlock.BlockType type)
    {
        tileEcoBlockType = type;
    }
    public void SetTileWorldElementType(World.WorldElement type)
    {
        tileWorldElementType = type;
    }
    public void SetTileTerrainType(TileTerrainType type)
    { 
        tileTerrainType = type;
    }
    public void SetTileBuildingType(Building.BuildingType type)
    {
        tileBuildingType = type;
    }




    public void SetLinkedEcoBlock(EcoBlock ecoBlock)
    {
        tileEcoBlockType = ecoBlock.blockType;
        linkedEcoBlock = ecoBlock;  
    }
    public void SetLinkedLocation (Location loc)
    {
        tileWorldElementType = World.WorldElement.Location;
        linkedLocation = loc;
    }
    public void SetLinkedCharacter(Character character)
    {
        tileWorldElementType = World.WorldElement.Character;
        linkedCharacter = character;
    }
    public void SetLinkedCreature(Creature creature)
    {
        tileWorldElementType = World.WorldElement.Creature;
        linkedCreature = creature;
    }
    public void SetLinkedItem(Item item)
    {
        tileWorldElementType = World.WorldElement.Item;
        linkedItem = item;
    }
 

}
