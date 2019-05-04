using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCollection : MonoBehaviour
{
    // Acts as general repository for sprites across the application. Holds dictionaries that pair sprites with data types .

    public static SpriteCollection Instance { get; protected set; }

    [HideInInspector]
    public Dictionary<Location.LocationType, Sprite> locationTypePairings = new Dictionary<Location.LocationType, Sprite>();
    [HideInInspector]
    public Dictionary<Location.LocationSubType, Sprite> locationSubTypePairings = new Dictionary<Location.LocationSubType, Sprite>();

    private void Awake()
    {
        Instance = this;
        locationTypePairings.Add(Location.LocationType.Settled, locationSettled);
        locationTypePairings.Add(Location.LocationType.Productive, locationProductive);
        locationTypePairings.Add(Location.LocationType.Defensive, locationDefensive);
        locationTypePairings.Add(Location.LocationType.Cultural, locationCultural);
        locationTypePairings.Add(Location.LocationType.Natural, locationNatural);

        locationSubTypePairings.Add(Location.LocationSubType.Village, locationSubVillage);

    }

    public Sprite containerHighlightSprite;
    public Sprite containerSelectionSprite;

    public Sprite politicalSprite1;
    public Sprite politicalSprite2;
    public Sprite politicalSprite3;
    public Sprite politicalSprite4;
    public Sprite politicalSprite5;
    public Sprite economicSprite1;
    public Sprite economicSprite2;
    public Sprite economicSprite3;
    public Sprite economicSprite4;
    public Sprite economicSprite5;
    public Sprite colorBallSpriteBlue;
    public Sprite colorBallSpriteRed;
    public Sprite colorBallSpriteGreen;
    public Sprite colorBallSpriteYellow;
    public Sprite colorBallSpriteOrange;

    public Sprite locationSettled;
    public Sprite locationProductive;
    public Sprite locationDefensive;
    public Sprite locationCultural;
    public Sprite locationNatural;

    public Sprite locationSubVillage;

    public Sprite seaSprite;
    public Sprite landSprite;
    public Sprite mountainSprite;
    public Sprite forestSprite;

    public Sprite schematicCharacterSprite;
    public Sprite schematicCreatureSprite;
    public Sprite schematicItemSprite;
    public Sprite schematicDominatingSprite;
    public Sprite schematicIndependentSprite;
    public Sprite schematicSecondarySprite;
    public Sprite schematicDominatedSprite;
    public Sprite schematicPopulationSprite;
    public Sprite schematicTerritorySprite;
    public Sprite schematicWarbandSprite;

    public Sprite colorTileDarkred;
    public Sprite colorTileRed;
    public Sprite colorTileYellow;
    public Sprite colorTileOrange;
    public Sprite colorTileBrown;
    public Sprite colorTileLightgreen;
    public Sprite colorTileDarkgreen;
    public Sprite colorTileLightblue;
    public Sprite colorTileDarkblue;
    public Sprite colorTilePurple;
    public Sprite colorTileWhite;
    public Sprite colorTileGrey;
    public Sprite colorTileBlack;
    public Sprite colorTileBlackDot;

    public Sprite clickableBackgroundSprite;




}
