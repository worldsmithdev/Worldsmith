using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Handles selecting and fetching items. Probably more when items do more

    public static ItemController Instance { get; protected set; }


    Item clickedItem;

    private void Awake()
    {
        Instance = this;
    }

    public List<Item> GetItemsAtLocation(Location loc)
    {
        List<Item> returnList = new List<Item>();
        foreach (Item item in WorldController.Instance.GetWorld().itemList)
            if (item != null)
                if (item.GetPositionVector().x == loc.GetPositionVector().x && item.GetPositionVector().y == loc.GetPositionVector().y)
                    returnList.Add(item);
        return returnList;
    }


    public void SetClickedItem (Item item)
    {
        clickedItem = item;
    }
}
