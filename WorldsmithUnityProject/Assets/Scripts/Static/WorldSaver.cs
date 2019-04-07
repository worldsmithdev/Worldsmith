using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class WorldSaver : MonoBehaviour
{
    // Handles storage and loading of worlds, through the SavePanel

    public static WorldSaver Instance { get; protected set; }

    public TextMeshProUGUI activeWorldText;
    public Button createButton;
    public Button saveButton;
    public Button clearButton;
    public Button loadButton;
    public TMP_InputField createInputField;
    public TMP_InputField saveInputField;
    public TMP_Dropdown worldsDropdown;

    List<Location> locationsList = new List<Location>();
    string saveFileName = "/SavedWorlds.ws";
    List<World> savedWorldsList = new List<World>();
    List<string> savedWorldsNamesList;

    private void Awake()
    {
        Instance = this;
        createInputField.onValueChanged.AddListener(delegate { CreateInputFieldValueCheck(); });
    }


    public void CreateWorldFromButton()
    {
        EconomyController.Instance.ClearDictionaries();
        WorldController.Instance.CreateNewWorld(createInputField.text); 
        LocationController.Instance.SetPreSelectedLocation();
        createInputField.text = "";
        UIController.Instance.RefreshUI(); 
    }

    public void SaveWorldFromButton()
    {
        if (saveInputField.text != "")
        {
            SaveAsNewWorld(WorldController.Instance.GetWorld(), saveInputField.text);
            saveInputField.text = "";
        }
        else        
            SaveExistingWorld(WorldController.Instance.GetWorld());        

        RefreshSavedWorldsDropdown();
        RefreshSaveUI();
    }

    public void SaveExistingWorld(World world)
    {
        if (HasWorldBeenSavedAlready(world))
        {
            RemoveWorldFromSaves(world.GetName());
            Debug.Log("Saving World (overwrite): " + world.GetName());
        }
        else
            Debug.Log("Saved World: " + world.GetName());

        world.SyncDataToWorld();
        savedWorldsList.Add(world); 

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFileName);  
        bf.Serialize(file, savedWorldsList);
        file.Position = 0;
        file.Close();
    }
    public void SaveAsNewWorld(World world, string worldname)
    {
        World newWorld = WorldController.Instance.CreateNewWorld(worldname);  
        newWorld.SyncDataToWorld();
        savedWorldsList.Add(newWorld);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFileName); 
        bf.Serialize(file, savedWorldsList);
        file.Position = 0;
        file.Close();
        Debug.Log("Saved World: " + newWorld.GetName());
    }
     

    public void LoadWorldFromButton()
    {
        LoadWorldFromName(worldsDropdown.options[worldsDropdown.value].text);
        UIController.Instance.RefreshUI(); 
    }
    public void LoadWorldFromName(string loadedworldname)
    { 
        bool matches = false;
        foreach (World world in savedWorldsList)
        {                                   
                if (loadedworldname == WorldController.Instance.GetWorld().GetName())
                {
                    matches = true;
            //        Debug.Log("World is already active!");
                }
                else if (world.GetName() == loadedworldname)
                {
                    WorldController.Instance.LoadExistingWorld(world);
                    world.SyncDataFromWorld(); 
                    matches = true;
                    Debug.Log("Loaded World: " + world.GetName());
                }
        }
        if (matches == false)                                
            Debug.Log("Selected world is not in list: " + loadedworldname); 
    }
    public void LoadWorldFromProgramStart(string loadedworldname)
    {
        LoadSavedWorldsList();   
            bool matches = false;
            foreach (World world in savedWorldsList)
            {              
                if (world.GetName() == loadedworldname)
                {
                    WorldController.Instance.LoadExistingWorld(world);
                    world.SyncDataFromWorld();
                    matches = true;
                    Debug.Log("Loaded World: " + world.GetName());
                }
            }
            if (matches == false)
                Debug.Log("Selected world is not in list: " + loadedworldname);        
    } 
    public void ClearSaveWorldsList()
    {
        savedWorldsList = new List<World>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFileName); 
        bf.Serialize(file, savedWorldsList);
        file.Position = 0;
        file.Close();
        Debug.Log("Cleared all saved worlds");
        RefreshSavedWorldsDropdown();
    }
    public void LoadSavedWorldsList()
    { 
        if (File.Exists(Application.persistentDataPath + saveFileName ))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open);
            savedWorldsList = (List<World>)bf.Deserialize(file);
            file.Position = 0;
            file.Close();
        }
        else
            Debug.Log("Trying to load world list from invalid path!");
    }


    public bool HasWorldBeenSavedAlready(World world)
    {
        bool prevSaved = false;
        foreach (World g in savedWorldsList)        
            if (g.GetName() == world.GetName()) prevSaved = true;
        return prevSaved;
    }
    public void RemoveWorldFromSaves(string removename)
    {
        World worldToRemove = new World();
        foreach (World world in savedWorldsList)        
            if (world.GetName() == removename)            
                worldToRemove = world;        
        savedWorldsList.Remove(worldToRemove);
    }

    public void RefreshSaveUI()
    { 
        activeWorldText.text ="Active World: " +  WorldController.Instance.GetWorld().GetName();
        RefreshSavedWorldsDropdown();
    }

    public void RefreshSavedWorldsDropdown()
    {
        LoadSavedWorldsList();       
        savedWorldsNamesList = new List<string>();
        foreach (World world in savedWorldsList)
            savedWorldsNamesList.Add(world.GetName());
        worldsDropdown.ClearOptions();
        worldsDropdown.AddOptions(savedWorldsNamesList);        
    }
 
    void CreateInputFieldValueCheck()
    {
        if (createInputField.text == "")
            createButton.interactable = false;
        else
            createButton.interactable = true;
    }


}
