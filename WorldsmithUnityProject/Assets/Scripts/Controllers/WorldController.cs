using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    // Creates Worlds and their content, and accesses the active World. 
    // Sets things in motion on Play through its Start function.

    public static WorldController Instance { get; protected set; }
   
    World activeWorld;  

    public LocationBuilder locationBuilder;
    public GodBuilder godBuilder;
    public ItemBuilder itemBuilder;
    public CharacterBuilder characterBuilder;
    public CreatureBuilder creatureBuilder;
    public FactionBuilder factionBuilder;
    public StoryBuilder storyBuilder;
    public LawBuilder lawBuilder;



    public bool loadExistingWorld = false;
    public string existingWorldName;

    private void Awake()
    {
        Instance = this;
        WorldConstants.SetDictionaries();
        Converter.SetDictionaries();
    }

    private void Start()
    {
        if (loadExistingWorld == false)
            AutoCreateWorld();
        else
            LoadExistingWorld(existingWorldName);

        LocationController.Instance.SetPreSelectedLocation();
        UIController.Instance.RefreshUI();
        EconomyController.Instance.ForwardCycle();
    }    
    void AutoCreateWorld()
    {
        World newWorld = new World("AutoWorld");
        activeWorld = newWorld;

        CreateWorldElements();

        newWorld.SetEconomy(new Economy());
        EconomyController.Instance.BuildEconomy();  
    } 
    
    void LoadExistingWorld (string worldname)
    {
        WorldSaver.Instance.LoadWorldFromProgramStart(worldname);
    }
    public void LoadExistingWorld(World world)
    {
        activeWorld = world; 
        ContainerController.Instance.MatchLocationsToContainers();
        LocationController.Instance.SetPreSelectedLocation();
    }

    public World CreateNewWorld(string worldname)
    {
        World newWorld = new World(worldname);
        activeWorld = newWorld;

        CreateWorldElements();

        activeWorld.SetEconomy(new Economy());
        EconomyController.Instance.BuildEconomy();
        Debug.Log("Created World: " + worldname);

        return newWorld;
    }


    public World GetWorld()
    {
        return activeWorld;
    }

    void CreateWorldElements()
    {
        CreateLocations();
        ContainerController.Instance.MatchLocationsToContainers();
        CreateGods();
        CreateItems();
        CreateCharacters();
        CreateCreatures();
        CreateFactions();
        CreateLaws();
        CreateStories();
    }
    void CreateLocations()
    {
        locationBuilder.CreateLocations( activeWorld); 
    }
    void CreateGods()
    {
       godBuilder.CreateGods(activeWorld);
    }

    void CreateItems()
    {
       itemBuilder.CreateItems(activeWorld);
    }

    void CreateCharacters()
    {
       characterBuilder.CreateCharacters(activeWorld);
    }

    void CreateCreatures()
    {
      creatureBuilder.CreateCreatures(activeWorld);
    }
    void CreateFactions()
    {
       factionBuilder.CreateFactions(activeWorld);
    }
    void CreateStories()
    {
       storyBuilder.CreateStories(activeWorld);
    }
    void CreateLaws()
    {
      lawBuilder.CreateLaws(activeWorld);
    }

    public bool ElementIDExists(string id, WorldElement.ElementType type)
    {
        if (type == WorldElement.ElementType.Location)
        {
            foreach (Location element in activeWorld.locationList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.God)
        {
            foreach (God element in activeWorld.godList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.Character)
        {
            foreach (Character element in activeWorld.characterList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.Item)
        {
            foreach (Item element in activeWorld.itemList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.Creature)
        {
            foreach (Creature element in activeWorld.creatureList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.Faction)
        {
            foreach (Faction element in activeWorld.factionList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.Story)
        {
            foreach (Story element in activeWorld.storyList)
                if (element.elementID == id)
                    return true;
        }
        else if (type == WorldElement.ElementType.Law)
        {
            foreach (Law element in activeWorld.lawList)
                if (element.elementID == id)
                    return true;
        } 
        return false;
    }

}
