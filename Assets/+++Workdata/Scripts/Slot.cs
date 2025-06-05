using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
     [HideInInspector] 
     public Item item;

     public Image slotImage;
     public TextMeshProUGUI slotAmount;
     
     private Toggle toggle;

     private void Awake()
     {
          toggle = GetComponent<Toggle>();
     }

     public void RefreshSlot(Item newItem = null, int amount = 0)
     {
          if (newItem == null)
          {
               ClearSlot();
               return;
          }

          toggle.interactable = true;
          slotImage.enabled = true;
          item = newItem;
          slotImage.sprite = item.itemSprite;
          slotAmount.SetText(amount.ToString());
     }

     public void ClearSlot()
     {
          toggle.interactable = false;
          item = null;
          slotImage.enabled = false;
          slotAmount.SetText("");
     }

     public void SlotHighlight()
     {
          if (!toggle.isOn) return;

          toggle.graphic.enabled = true;
          InventoryManager.Instance.ShowItemInformation(item);
     }

     public void ForceHideHighlight()
     {
          if (!toggle.isOn) return;

          toggle.graphic.enabled = false;
     }

     public void ForceToggleOn()
     {
          toggle.isOn = true;
     }

     public bool NoItemButIsOn()
     {
          return item == null && toggle.isOn;
     }
}
