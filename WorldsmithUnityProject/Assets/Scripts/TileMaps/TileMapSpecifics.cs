using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSpecifics : MonoBehaviour
{
    // Specific TileMap names, called from TileMapBuilder, are manually created here


    public void BuildRuleMap(TileMap givenMap)
    {
        Location loc = LocationController.Instance.GetSelectedLocation();
        Ruler localRuler = loc.localRuler;

        for (int x = 0; x < givenMap.xSize; x += 1)
            for (int y = 0; y < givenMap.ySize; y += 1)
                givenMap.GetTileAt(x, y).SetTileEcoBlockType(EcoBlock.BlockType.Unassigned);

        int xPos = 2;
        int yPos = 0;

        Territory terr = TerritoryController.Instance.GetTerritoryAtLocation(loc);
        if (terr != null)
        {
            givenMap.GetTileAt(xPos, yPos).SetTileEcoBlockType(EcoBlock.BlockType.Territory);
            givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(terr);
            xPos += 1;
        }

        foreach (Population pop in PopulationController.Instance.GetPopulationsAtLocation(loc))
            if (pop != null)
            {
                xPos += 1;
                if (xPos <= givenMap.xSize)
                {
                    givenMap.GetTileAt(xPos, yPos).SetTileEcoBlockType(EcoBlock.BlockType.Population);
                    givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(pop);
                }
                    
            }
        xPos = 5;
        yPos = 2;
        givenMap.GetTileAt(xPos, yPos).SetLinkedLocation(loc);
        xPos = 5;
        yPos = 4;


        // Add secondary rulers
        foreach (Ruler ruler in RulerController.Instance.GetLocationRulers(loc))
            if (ruler != null && ruler.isLocalRuler == false)
            {
                xPos += 2;
                givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(ruler);
            }

        // Add local ruler
        if (localRuler != null)
        {
            if (localRuler.rulerHierarchy == Ruler.Hierarchy.Dominating)
            {
                xPos = 5;
                yPos = 6;
                givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(localRuler);
                foreach (Location contloc in localRuler.GetControlledLocations())
                {
                    xPos += 2;
                    if (xPos <= givenMap.xSize)
                        givenMap.GetTileAt(xPos, yPos).SetLinkedLocation(contloc);
                }
            }
            else if (localRuler.rulerHierarchy == Ruler.Hierarchy.Independent)
            {
                xPos = 5;
                yPos = 4;
                givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(localRuler);
            }

            else  
            {
                xPos = 5;
                yPos = 4;
                givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(localRuler);
                yPos = 6;
                givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(localRuler.GetController());
                xPos += 2;
                givenMap.GetTileAt(xPos, yPos).SetLinkedLocation(localRuler.GetController().GetHomeLocation());
                foreach (Location contloc in localRuler.GetController().GetControlledLocations())
                {
                    xPos += 2;
                    if (xPos <= givenMap.xSize)
                        givenMap.GetTileAt(xPos, yPos).SetLinkedLocation(contloc);
                }
            }
        }
        else if (loc.dominatingRuler != null)
        {
            xPos = 5;
            yPos = 6;
            givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(loc.dominatingRuler);
            xPos += 2;
            givenMap.GetTileAt(xPos, yPos).SetLinkedLocation(loc.dominatingRuler.GetHomeLocation());
            foreach (Location contloc in loc.dominatingRuler.GetControlledLocations())
            {
                xPos += 2;
                if (xPos <= givenMap.xSize)
                    givenMap.GetTileAt(xPos, yPos).SetLinkedLocation(contloc);
            }
        }

        xPos = 5;
        yPos = 2;


        foreach (Warband warband in WarbandController.Instance.GetWarbandsAtLocation(loc))
            if (warband != null)
            {
                xPos -= 2;
                givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(warband);
            }
        xPos = 5;
        yPos = 4;
        if (localRuler != null)
        {
            foreach (Warband warband in localRuler.GetControlledWarbands())
                if (warband != null)
                {
                    xPos -= 2;
                    givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(warband);
                }
            xPos = 5;
            yPos = 6;
            if (localRuler.rulerHierarchy == Ruler.Hierarchy.Dominated)
                foreach (Warband warband in localRuler.GetController().GetControlledWarbands())
                    if (warband != null)
                    {
                        xPos -= 2;
                        givenMap.GetTileAt(xPos, yPos).SetLinkedEcoBlock(warband);
                    }
        }




    }


    public void BuildElementsMap(TileMap givenMap)
    {
        Location loc = LocationController.Instance.GetSelectedLocation();
        Ruler localRuler = loc.localRuler;
        for (int x = 0; x < givenMap.xSize; x += 1)
            for (int y = 0; y < givenMap.ySize; y += 1)
                givenMap.GetTileAt(x, y).SetTileWorldElementType(World.WorldElement.Unassigned);


        int xPos = -1;
        int yPos = givenMap.ySize - 1;

        foreach (Character character in CharacterController.Instance.GetCharactersAtLocation(loc))
            if (character != null)
            {
                xPos++;
                givenMap.GetTileAt(xPos, yPos).SetLinkedCharacter(character);
            }
        xPos += 1;
        foreach (Creature creature in CreatureController.Instance.GetCreaturesAtLocation(loc))
            if (creature != null)
            {
                xPos++;
                givenMap.GetTileAt(xPos, yPos).SetLinkedCreature(creature);
            }
        xPos += 1;
        if (xPos > 16)
        {
            xPos = 0;
            yPos--;
        }
        foreach (Item item in ItemController.Instance.GetItemsAtLocation(loc))
            if (item != null)
            {
                xPos++;
                givenMap.GetTileAt(xPos, yPos).SetLinkedItem(item);
            }
        xPos += 1;
        if (xPos > 16)
        {
            xPos = 0;
            yPos--;
        }
    }


    public void BuildLocalExchangesMap(TileMap givenMap)
    {
        Location selectedLoc = LocationController.Instance.GetSelectedLocation();
        for (int x = 0; x < givenMap.xSize; x += 1)
            for (int y = 0; y < givenMap.ySize; y += 1)
                givenMap.GetTileAt(x, y).SetEmpty();
         
        int xPos = givenMap.xSize ; 
        int yPos;

        int count = 0;

        // Most recent market on the right side always. Move to left.
        // 3 participants horizontally below it.
        // below each of those, the exchanges in which they were active

        MarketController.Instance.archivedLocalMarkets[selectedLoc].Reverse();

        foreach (LocalMarket locmarket in MarketController.Instance.archivedLocalMarkets[selectedLoc])
        {
            count++;
            if (count < 3)
            { 
                xPos -= 10;
                yPos = givenMap.ySize - 2;

                givenMap.GetTileAt(xPos, yPos).SetLinkedLocalMarket(locmarket); 

                xPos -= 2;
                foreach (Participant participant in locmarket.participantList)
                {
                    xPos += 1;
                    yPos = givenMap.ySize - 4; 
                     givenMap.GetTileAt(xPos, yPos).SetLinkedParticipant(participant);
                     
                    foreach (LocalExchange locexch in locmarket.localExchangesList)
                    {
                        if (locexch.activeParticipant == participant)
                        {
                            yPos -= 1;
                            givenMap.GetTileAt(xPos, yPos).SetLinkedLocalExchange(locexch);
                        }
                        if (locexch.passiveParticipant == participant)
                        {
                            yPos -= 1;
                            givenMap.GetTileAt(xPos, yPos).SetLinkedLocalExchangePassive(locexch);
                        }
                    }
                }
                xPos -= locmarket.participantList.Count;
            }  
        } 
        MarketController.Instance.archivedLocalMarkets[selectedLoc].Reverse(); 

    }
    public void BuildRegionalExchangesMap(TileMap givenMap)
    {

        Location selectedLoc = LocationController.Instance.GetSelectedLocation();
        for (int x = 0; x < givenMap.xSize; x += 1)
            for (int y = 0; y < givenMap.ySize; y += 1)
                givenMap.GetTileAt(x, y).SetEmpty();

        int xPos = givenMap.xSize;
        int yPos;

        int count = 0;
         
        MarketController.Instance.archivedRegionalMarkets[selectedLoc].Reverse();

        foreach (RegionalMarket market in MarketController.Instance.archivedRegionalMarkets[selectedLoc])
        {
            count++;
            if (count < 3)
            {
                xPos -= 10;
                yPos = givenMap.ySize - 2;

                givenMap.GetTileAt(xPos, yPos).SetLinkedRegionalMarket(market);

                xPos -= 3;

                givenMap.GetTileAt(xPos, yPos).SetLinkedRegionalSeller(market.seller);
                foreach (RegionalExchange exchange in ExchangeController.Instance.cycleRegionalExchanges)
                {
                //   Debug.Log("Active: " + exchange.activeParty.blockID + " passive: " + exchange.passiveParty.blockID + " seller: " + market.seller.blockID);
                    if (exchange.passiveParty == market.seller)
                    { 
                        yPos -= 1;
                        givenMap.GetTileAt(xPos, yPos).SetLinkedRegionalExchange(exchange);
                        if (yPos < 2)
                        {
                            yPos = givenMap.ySize - 2;
                            xPos += 1;
                        }

                    }
                }

                xPos += 1;
                yPos = givenMap.ySize - 2;
                foreach (Ruler buyer in market.buyersList)
                { 
                    xPos += 1;
                    yPos = givenMap.ySize - 4;
                    givenMap.GetTileAt(xPos, yPos).SetLinkedRegionalBuyer(buyer);

                    foreach (RegionalExchange exchange in market.regionalExchangesList)
                    {  
                        if (exchange.activeParty == market.seller && exchange.passiveParty == buyer)
                        { 
                            yPos -= 1;
                            givenMap.GetTileAt(xPos, yPos).SetLinkedRegionalExchange(exchange);
                        } 
                    }
                }
                xPos -= market.buyersList.Count;
            }
        }
        MarketController.Instance.archivedRegionalMarkets[selectedLoc].Reverse();

    }
    public void BuildGlobalExchangesMap(TileMap givenMap)
    {
        Location selectedLoc = LocationController.Instance.GetSelectedLocation();
        for (int x = 0; x < givenMap.xSize; x += 1)
            for (int y = 0; y < givenMap.ySize; y += 1)
                givenMap.GetTileAt(x, y).SetEmpty();

        int xPos = givenMap.xSize;
        int yPos;

        int count = 0;

        MarketController.Instance.archivedGlobalMarkets[selectedLoc].Reverse();

        foreach (GlobalMarket market in MarketController.Instance.archivedGlobalMarkets[selectedLoc])
        {
            count++;
            if (count < 3)
            {
                xPos -= 10;
                yPos = givenMap.ySize - 3; 
                givenMap.GetTileAt(xPos, yPos).SetLinkedGlobalMarket(market);
                foreach (GlobalExchange exchange in market.globalExchangesList)
                {
                    yPos -= 1;
                    givenMap.GetTileAt(xPos, yPos).SetLinkedGlobalExchange(exchange);
                }
            }
        }
        MarketController.Instance.archivedGlobalMarkets[selectedLoc].Reverse(); 
    }
}
