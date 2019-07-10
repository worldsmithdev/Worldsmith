using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    // Data container for TileMaps that kind of messily is able to hold very different datatypes. 

    public string tileName;
    public int xCoord;
    public int yCoord;

    public enum TileDataType { Unassigned, Blank, Terrain, Building, WorldElement, EcoBlock, Abstraction}
    public enum TileTerrainType { Sea, Land, Mountain, Forest }
    public enum TileAbstractionType { Unassigned, LocalMarket, Participant, LocalExchange, LocalExchangePassive, RegionalMarket, RegionalExchange, RegionalExchangePassive, RegionalBuyer,RegionalSeller, GlobalMarket, GlobalExchange }

    public string originalTileMapName;

    public TileDataType tileDataType;
    public TileTerrainType tileTerrainType;
    public TileAbstractionType tileAbstractionType;
    public Building.BuildingType tileBuildingType;
    public World.WorldElement tileWorldElementType;
    public EcoBlock.BlockType tileEcoBlockType;

    public EcoBlock linkedEcoBlock;
    public Location linkedLocation;
    public Character linkedCharacter;
    public Creature linkedCreature;
    public Item linkedItem;
    public LocalMarket linkedLocalMarket;
    public Participant linkedParticipant;
    public RegionalMarket linkedRegionalMarket;
    public Ruler linkedRegionalBuyer;
    public Ruler linkedRegionalSeller;
    public GlobalMarket linkedGlobalMarket;
    public Exchange linkedExchange;


    public Tile(int xloc, int yloc, string mapname)
    {
        originalTileMapName = mapname;
        xCoord = xloc;
        yCoord = yloc;
        tileName = "Tile" + "X" + xCoord + "Y" + yCoord;
    } 

    public void SetBlank()
    {
        tileDataType = TileDataType.Blank;
    }
    public void SetEmpty()
    {
        tileDataType = TileDataType.Unassigned;
    }
    public void SetTileEcoBlockType(EcoBlock.BlockType type)
    {
        tileDataType = TileDataType.EcoBlock;
        tileEcoBlockType = type;
    }
    public void SetTileWorldElementType(World.WorldElement type)
    {
        tileDataType = TileDataType.WorldElement;
        tileWorldElementType = type;
    }
    public void SetTileTerrainType(TileTerrainType type)
    {
        tileDataType = TileDataType.Terrain;
        tileTerrainType = type;
    }
    public void SetTileBuildingType(Building.BuildingType type)
    {
        tileDataType = TileDataType.Building;
        tileBuildingType = type;
    }

     
    public void SetLinkedEcoBlock(EcoBlock ecoBlock)
    {
        tileDataType = TileDataType.EcoBlock;
        tileEcoBlockType = ecoBlock.blockType;
        linkedEcoBlock = ecoBlock;
    }
    public void SetLinkedLocation(Location loc)
    {
        tileDataType = TileDataType.WorldElement;
        tileWorldElementType = World.WorldElement.Location;
        linkedLocation = loc;
    }
    public void SetLinkedCharacter(Character character)
    {
        tileDataType = TileDataType.WorldElement;
        tileWorldElementType = World.WorldElement.Character;
        linkedCharacter = character;
    }
    public void SetLinkedCreature(Creature creature)
    {
        tileDataType = TileDataType.WorldElement;
        tileWorldElementType = World.WorldElement.Creature;
        linkedCreature = creature;
    }
    public void SetLinkedItem(Item item)
    {
        tileDataType = TileDataType.WorldElement;
        tileWorldElementType = World.WorldElement.Item;
        linkedItem = item;
    }
    public void SetLinkedLocalMarket(LocalMarket market)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.LocalMarket;
        linkedLocalMarket = market;
    }
    public void SetLinkedParticipant(Participant participant)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.Participant;
        linkedParticipant = participant;
    }
    public void SetLinkedLocalExchange(Exchange exchange)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.LocalExchange;
        linkedExchange = exchange;
    }
    public void SetLinkedLocalExchangePassive(Exchange exchange)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.LocalExchangePassive;
        linkedExchange = exchange;
    }
    public void SetLinkedRegionalMarket(RegionalMarket market)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.RegionalMarket;
        linkedRegionalMarket = market;
    }
    public void SetLinkedRegionalExchange(Exchange exchange)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.RegionalExchange;
        linkedExchange = exchange;
    }
    public void SetLinkedRegionalExchangePassive(Exchange exchange)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.RegionalExchangePassive;
        linkedExchange = exchange;
    }
    public void SetLinkedRegionalBuyer(Ruler ruler)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.RegionalBuyer;
        linkedRegionalBuyer = ruler;
    }
    public void SetLinkedRegionalSeller(Ruler ruler)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.RegionalSeller;
        linkedRegionalSeller = ruler;
    }
    public void SetLinkedGlobalMarket(GlobalMarket market)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.GlobalMarket;
        linkedGlobalMarket = market;
    }
    public void SetLinkedGlobalExchange(Exchange exchange)
    {
        tileDataType = TileDataType.Abstraction;
        tileAbstractionType = TileAbstractionType.GlobalExchange;
        linkedExchange = exchange;
    }
}
