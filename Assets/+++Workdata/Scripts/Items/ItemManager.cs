using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    [SerializeField] Item[] items;

    public Item[] Items => items;

    private void Awake()
    {
        Instance = this;
    }

    public Item GetItemById(string itemId)
    {
        foreach (var item in items)
        {
            if (item.itemId == itemId)
            {
                return item;
            }
        }

        return null;
    }

    public Item[] GetAllItems()
    {
        return items;
    }
}
