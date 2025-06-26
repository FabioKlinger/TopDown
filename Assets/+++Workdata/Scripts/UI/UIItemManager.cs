using System;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    [SerializeField] private GameObject uiItemPrefab;
    [SerializeField] private Transform itemListContainer;
                     //private ItemUIElement element;


    private void OnEnable()
    {
        GameState.VisualizeItemUIElement += AddItemElement;
    }

    private void OnDisable()
    {
        GameState.VisualizeItemUIElement -= AddItemElement;
    }

    public void AddItemElement(Item collectedItem, int value)
    {
        GameObject newItem = Instantiate(uiItemPrefab, itemListContainer);
        newItem.GetComponent<ItemUIElement>().SetItemInfo(value, collectedItem.itemName);
       
    }
    
    
}
