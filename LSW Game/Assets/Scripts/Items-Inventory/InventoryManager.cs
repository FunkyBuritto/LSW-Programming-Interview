using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [HideInInspector] public List<InventorySlot> inventory = new List<InventorySlot>();
    private Dictionary<InventoryItem, InventorySlot> itemDictionary = new Dictionary<InventoryItem, InventorySlot>();

    [SerializeField] private List<GameObject> inventorySlots = new List<GameObject>();
    private List<Image> inventoryIcons = new List<Image>();
    private List<TextMeshProUGUI> inventoryText = new List<TextMeshProUGUI>();

    private void Awake() { 
        instance = this;

        // Loop over the inventory slots parents and get assign the components of their children
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventoryIcons.Add(inventorySlots[i].GetComponentsInChildren<Image>()[1]);
            inventoryText.Add(inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>());
        }

        UpdateIventoryUI();
    }

    public void AddItem(InventoryItem item)
    {
        // See if the item is already in the inventory
        // Increase the size if true, create a new slot if false
        if(itemDictionary.TryGetValue(item, out InventorySlot slot)) {
            slot.stackSize++;
        } else {
            // if we are on the max amount of slots dont add the item
            if (inventory.Count >= inventorySlots.Count)
                return;

            InventorySlot newItem = new InventorySlot(item);
            inventory.Add(newItem);
            itemDictionary.Add(item, newItem);
        }

        UpdateIventoryUI();
    }

    public void RemoveItem(InventoryItem item)
    {
        // See if the item is in the inventory
        if(itemDictionary.TryGetValue(item, out InventorySlot slot)) {
            slot.stackSize--;
            // Remove the slot if there are none left
            if (slot.stackSize <= 0) {
                inventory.Remove(slot);
                itemDictionary.Remove(item);
            }

            UpdateIventoryUI();
        }
    }

    private void UpdateIventoryUI() {
        // Loop over all the UI slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // We fill the slots until we are on the inventory amount
            // then everything extra should be empty 
            if(inventory.Count > i) {
                inventoryIcons[i].sprite = inventory[i].item.icon;
                inventoryIcons[i].color = Color.white;
                inventoryText[i].text = inventory[i].stackSize.ToString();
            } else {
                inventoryIcons[i].sprite = null;
                inventoryIcons[i].color = Color.clear;
                inventoryText[i].text = "";
            }   
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public InventoryItem item;
    public int stackSize;

    public InventorySlot(InventoryItem item) 
    {
        this.item = item;
        stackSize = 1;
    }
}
