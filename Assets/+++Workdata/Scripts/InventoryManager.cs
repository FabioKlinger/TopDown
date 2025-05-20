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
}
