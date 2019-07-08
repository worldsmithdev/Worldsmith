using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapHandler : MonoBehaviour
{
    // All clicks and hovers on TileBlock prefabs get sent here

    TileMap activeMap;


    public void HandleTileHover (Tile tile)
    {
        bool noElementHovered = false; 
        activeMap = TileMapController.Instance.GetTileMapFromList(tile.originalTileMapName);
        if (tile.tileWorldElementType == World.WorldElement.Location)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Location: " + tile.linkedLocation.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Character)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Character: " + tile.linkedCharacter.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Creature)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Creature: " + tile.linkedCreature.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Item)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Item: " + tile.linkedItem.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Location)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Location: " + tile.linkedLocation.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Unassigned)
            noElementHovered = true;

        if (tile.tileEcoBlockType == EcoBlock.BlockType.Ruler)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Ruler: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Warband)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Warband: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Population)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Population: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Territory)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Territory: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Unassigned && noElementHovered == true)
            UIController.Instance.exploreUI.overviewSchematicHeaderText.text = "Schematic";

        //TODO: more elegant way to check which map is active: explore layout, Locationsection layout, perhaps other future option
        if (UIController.Instance.currentSection == UIController.Section.World)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.exploreUI.layoutSchematicHeaderText.text = "Building: " + tile.tileBuildingType;
            else
                UIController.Instance.exploreUI.layoutSchematicHeaderText.text = "Schematic";
        }
        else if (UIController.Instance.currentSection == UIController.Section.Location)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.locationUI.hoveredText.text = "" + tile.tileBuildingType;
            else
                UIController.Instance.locationUI.hoveredText.text = "";
        }

        if (tile.tileAbstractionType == Tile.TileAbstractionType.LocalMarket)
            UIController.Instance.exploreUI.localExchangeSchematicHeaderText.text = "LocalMarket: " + tile.linkedLocalMarket.marketName;
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.Participant)
            UIController.Instance.exploreUI.localExchangeSchematicHeaderText.text = "Participant: " + tile.linkedParticipant.participantName;
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.LocalExchange)
            UIController.Instance.exploreUI.localExchangeSchematicHeaderText.text = "LocalExchange: " + tile.linkedExchange.exchangeName; 
        else
            UIController.Instance.exploreUI.localExchangeSchematicHeaderText.text = "Schematic"; 

        if (tile.tileAbstractionType == Tile.TileAbstractionType.RegionalMarket)
            UIController.Instance.exploreUI.regionalExchangeSchematicHeaderText.text = "RegionalMarket: " + tile.linkedRegionalMarket.marketName;
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.RegionalExchange)
            UIController.Instance.exploreUI.regionalExchangeSchematicHeaderText.text = "RegionalExchange: " + tile.linkedExchange.exchangeName;
        else
            UIController.Instance.exploreUI.regionalExchangeSchematicHeaderText.text = "Schematic";

        if (tile.tileAbstractionType == Tile.TileAbstractionType.GlobalMarket)
            UIController.Instance.exploreUI.globalExchangeSchematicHeaderText.text = "GlobalMarket: " + tile.linkedGlobalMarket.marketName;
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.GlobalExchange)
            UIController.Instance.exploreUI.globalExchangeSchematicHeaderText.text = "GlobalExchange: " + tile.linkedExchange.exchangeName;
        else
            UIController.Instance.exploreUI.globalExchangeSchematicHeaderText.text = "Schematic";

    }
    public void HandleTileClick(Tile tile)
    {
        activeMap = TileMapController.Instance.GetTileMapFromList(tile.originalTileMapName);

        if (tile.tileWorldElementType == World.WorldElement.Character)
        {
            CharacterController.Instance.SetSelectedCharacter(tile.linkedCharacter);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedCharacter(tile.linkedCharacter);
        }
        else if (tile.tileWorldElementType == World.WorldElement.Creature)
        {
            CreatureController.Instance.SetSelectedCreature(tile.linkedCreature);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedCreature(tile.linkedCreature);
        }
        else if (tile.tileWorldElementType == World.WorldElement.Item)
        {
            ItemController.Instance.SetSelectedItem(tile.linkedItem);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedItem(tile.linkedItem);
        }
        else if (tile.tileWorldElementType == World.WorldElement.Location)
        {
           ContainerController.Instance.ShiftSelectedContainer( ContainerController.Instance.GetContainerFromLocation (tile.linkedLocation));
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedLocation(tile.linkedLocation);
        }

        if (tile.tileEcoBlockType == EcoBlock.BlockType.Ruler)
        {
            RulerController.Instance.SetSelectedRuler((Ruler) tile.linkedEcoBlock);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedRuler((Ruler)tile.linkedEcoBlock);
        }
       
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Warband)
        {
            WarbandController.Instance.SetSelectedWarband((Warband)tile.linkedEcoBlock); 
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedWarband((Warband)tile.linkedEcoBlock);
        }
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Population)
        {
            PopulationController.Instance.SetSelectedPopulation ((Population)tile.linkedEcoBlock);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedPopulation((Population)tile.linkedEcoBlock);
        }
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Territory)
        {
            TerritoryController.Instance.SetSelectedTerritory((Territory)tile.linkedEcoBlock);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedTerritory((Territory)tile.linkedEcoBlock);
        }

        if (UIController.Instance.currentSection == UIController.Section.World)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.exploreUI.exploreTextSetter.SetClickedBuildingText(tile.tileBuildingType);  
        }
        else if (UIController.Instance.currentSection == UIController.Section.Location)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.locationUI.ClickLayoutBuilding(tile.tileBuildingType);
        }

        if (tile.tileAbstractionType == Tile.TileAbstractionType.LocalMarket)
        {
            MarketController.Instance.SetSelectedLocalMarket(tile.linkedLocalMarket);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedLocalMarketText((LocalMarket)tile.linkedLocalMarket);
        }
         
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.Participant)
        {
            MarketController.Instance.SetSelectedParticipant(tile.linkedParticipant);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedLocalParticipantText(tile.linkedParticipant);
        }
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.LocalExchange)
        {
            ExchangeController.Instance.SetSelectedExchange(tile.linkedExchange);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedLocalExchangeText(tile.linkedExchange);
        }
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.RegionalMarket)
        {
            MarketController.Instance.SetSelectedRegionalMarket(tile.linkedRegionalMarket);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedRegionalMarketText(tile.linkedRegionalMarket);
        }
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.RegionalExchange)
        {
            ExchangeController.Instance.SetSelectedExchange(tile.linkedExchange);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedRegionalExchangeText(tile.linkedExchange);
        }
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.GlobalMarket)
        {
            MarketController.Instance.SetSelectedGlobalMarket(tile.linkedGlobalMarket);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedGlobalMarketText(tile.linkedGlobalMarket);
        }
        else if (tile.tileAbstractionType == Tile.TileAbstractionType.GlobalExchange)
        {
            ExchangeController.Instance.SetSelectedExchange(tile.linkedExchange);
            UIController.Instance.exploreUI.exploreTextSetter.SetClickedGlobalExchangeText(tile.linkedExchange);
        }
    }
}
