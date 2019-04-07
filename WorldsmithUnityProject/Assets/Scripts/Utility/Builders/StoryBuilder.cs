using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBuilder : MonoBehaviour
{
     public StoryImports imports;

    public void CreateStories(World activeWorld)
    { 
        foreach (StoryImportsData data in imports.dataArray)
        {
            if (data.Name != "")
            {
                Story created = new Story(data.Name);
                created.fullText = data.Description;
                activeWorld.storyList.Add(created);
            } 
        } 
    }
}
