using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapHandler : MonoBehaviour
{
    // All clicks and hovers on TileBlock prefabs get sent here

    TileMap activeMap;


    public void HandleTileHover (Tile tile)
    {
        activeMap = TileMapController.Instance.GetTileMapFromList(tile.originalTileMapName);
        if (tile.tileWorldElementType == World.WorldElement.Character)
            UIController.Instance.exploreUI.overviewHoveredElementText.text = "World Element: " + tile.linkedCharacter.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Creature)
            UIController.Instance.exploreUI.overviewHoveredElementText.text = "World Element: " + tile.linkedCreature.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Item)
            UIController.Instance.exploreUI.overviewHoveredElementText.text = "World Element: " + tile.linkedItem.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Location)
            UIController.Instance.exploreUI.overviewHoveredElementText.text = "World Element: " + tile.linkedLocation.elementID;
        else if (tile.tileWorldElementType == World.WorldElement.Unassigned)
            UIController.Instance.exploreUI.overviewHoveredElementText.text = "World Elements" ;

        if (tile.tileEcoBlockType == EcoBlock.BlockType.Ruler)
            UIController.Instance.exploreUI.overviewHoveredRulerText.text = "Economy Block: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Warband)
            UIController.Instance.exploreUI.overviewHoveredRulerText.text = "Economy Block: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Population)
            UIController.Instance.exploreUI.overviewHoveredRulerText.text = "Economy Block: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Territory)
            UIController.Instance.exploreUI.overviewHoveredRulerText.text = "Economy Block: " + tile.linkedEcoBlock.blockID;
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Unassigned)
            UIController.Instance.exploreUI.overviewHoveredRulerText.text = "Economy Blocks" ;

        //TODO: more elegant way to check which map is active: explore layout, Locationsection layout, perhaps other future option
        if (UIController.Instance.currentSection == UIController.Section.World)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.exploreUI.layoutHoveredText.text = "" + tile.tileBuildingType;
            else
                UIController.Instance.exploreUI.layoutHoveredText.text = "";
        }
        else if (UIController.Instance.currentSection == UIController.Section.Location)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.locationUI.hoveredText.text = "Hovered: " + tile.tileBuildingType;
            else
                UIController.Instance.locationUI.hoveredText.text = "Hovered: ";
        }


    }
    public void HandleTileClick(Tile tile)
    {
        activeMap = TileMapController.Instance.GetTileMapFromList(tile.originalTileMapName);

        if (tile.tileWorldElementType == World.WorldElement.Character)
        {
            CharacterController.Instance.SetSelectedCharacter(tile.linkedCharacter);
            UIController.Instance.exploreUI.SetClickedCharacter(tile.linkedCharacter);
        }
        else if (tile.tileWorldElementType == World.WorldElement.Creature)
        {
            CreatureController.Instance.SetSelectedCreature(tile.linkedCreature);
            UIController.Instance.exploreUI.SetClickedCreature(tile.linkedCreature);
        }
        else if (tile.tileWorldElementType == World.WorldElement.Item)
        {
            ItemController.Instance.SetSelectedItem(tile.linkedItem);
            UIController.Instance.exploreUI.SetClickedItem(tile.linkedItem);
        }
        else if (tile.tileWorldElementType == World.WorldElement.Location)
        {
           ContainerController.Instance.ShiftSelectedContainer( ContainerController.Instance.GetContainerFromLocation (tile.linkedLocation)); 
        }

        if (tile.tileEcoBlockType == EcoBlock.BlockType.Ruler)
        {
            RulerController.Instance.SetSelectedRuler((Ruler) tile.linkedEcoBlock);
            UIController.Instance.exploreUI.SetClickedRuler((Ruler)tile.linkedEcoBlock);
        }
       
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Warband)
        {
            WarbandController.Instance.SetSelectedWarband((Warband)tile.linkedEcoBlock); 
            UIController.Instance.exploreUI.SetClickedWarband((Warband)tile.linkedEcoBlock);
        }
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Population)
        {
            PopulationController.Instance.SetSelectedPopulation ((Population)tile.linkedEcoBlock);
            UIController.Instance.exploreUI.SetClickedPopulation((Population)tile.linkedEcoBlock);
        }
        else if (tile.tileEcoBlockType == EcoBlock.BlockType.Territory)
        {
            TerritoryController.Instance.SetSelectedTerritory((Territory)tile.linkedEcoBlock);
            UIController.Instance.exploreUI.SetClickedTerritory((Territory)tile.linkedEcoBlock);
        }

        if (UIController.Instance.currentSection == UIController.Section.World)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.exploreUI.SetClickedBuildingText(tile.tileBuildingType);  
        }
        else if (UIController.Instance.currentSection == UIController.Section.Location)
        {
            if (tile.tileBuildingType != Building.BuildingType.Unassigned)
                UIController.Instance.locationUI.ClickLayoutBuilding(tile.tileBuildingType);
        }

    }
}
