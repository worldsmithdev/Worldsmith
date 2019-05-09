using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class World  
{

    public enum WorldElement { Unassigned, Location, God,Item,Character,Creature,Faction,Story,Law}
    string worldName;
    Economy worldEconomy;
    public int completedCycles = 0;

    public List<Character> characterList;
    public List<Location> locationList; 
    public List<God> godList;
    public List<Creature> creatureList;
    public List<Item> itemList; 
    public List<Faction> factionList;
    public List<Story> storyList;
    public List<Law> lawList;  
    

   
    public World()
    {

    }
    public World(string name)
    {
        worldName = name;
        characterList = new List<Character>();
        godList = new List<God>();
        creatureList = new List<Creature>();
        itemList = new List<Item>();
        factionList = new List<Faction>();
        storyList = new List<Story>();
        locationList = new List<Location>();
        lawList = new List<Law>();
     
    }


    public void SyncDataToWorld()
    {
        worldEconomy.SaveEconomy();
    }

    public void SyncDataFromWorld()
    {
        worldEconomy.LoadEconomy();
    }




    // SET functions
    public void SetEconomy(Economy givenEconomy)
    {
        worldEconomy = givenEconomy;
    }
    public void SetName(string str)
    {
        worldName = str;
    } 
    //GET-functions
    public string GetName()
    {
        return worldName;
    }
    public Economy GetEconomy()
    {
        return worldEconomy;
    }




}
