using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Setup")] 
    public List<Slot> slots;

    public GameObject inventoryContainer;

    public Image itemImage;
    public TextMeshProUGUI itemHeader, itemDescription;

    private bool alreadyForcedToggle;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        PlayerController.InventoryAction += InventoryMenu;
    }

    private void OnDisable()
    {
        PlayerController.InventoryAction -= InventoryMenu;
    }

    public void InventoryMenu()
    {
        if (inventoryContainer.activeSelf)
        {
            inventoryContainer.SetActive(false);
            return;
        }
        
        inventoryContainer.SetActive(true);

        List<State> currentStates = GameState.Instance.GetAllStates();

        for (int i = 0; i < currentStates.Count; i++) 
        {
            Item newItem = ItemManager.Instance.GetItemById(currentStates[i].id);
        }

        for (int i = currentStates.Count; i < slots.Count; i++)
        {
            slots[i].RefreshSlot();
        }

        if (currentStates.Count == 0)
        {
            alreadyForcedToggle = false;

            ShowItemInformation();
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].ForceHideHighlight();
            }

            return;
        }

        if (!alreadyForcedToggle)
        {
            slots[0].ForceHideHighlight();
            alreadyForcedToggle = true;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].NoItemButIsOn())
            {
                slots[0].ForceToggleOn();
                ShowItemInformation(slots[0].item);
                break;
            }
        }
    }

    public void ShowItemInformation(Item item = null)
    {
        if (item == null)
        {
            itemImage.enabled = false;
            itemHeader.SetText("");
            itemDescription.SetText("");
            return;
        }
        
        itemImage.enabled = true;
        itemImage.sprite = item.itemSprite;
        itemHeader.SetText(item.itemName);
        itemDescription.SetText(item.itemDescription);
    }
}
