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

    }
}
