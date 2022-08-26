using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int maxInventorySize;
    [SerializeField] private TextMeshProUGUI inventorySizeText;

    [HideInInspector] public List<InventorySlot> inventory = new List<InventorySlot>();
    private Dictionary<InventoryItem, InventorySlot> itemDictionary = new Dictionary<InventoryItem, InventorySlot>();

    [SerializeField] private List<GameObject> inventorySlots = new List<GameObject>();
    private List<Image> inventoryIcons = new List<Image>();
    private List<TextMeshProUGUI> inventoryText = new List<TextMeshProUGUI>();

    private void Awake() { 
        instance = this;

        // Loop over the inventory slots parents and assign the components of their children
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventoryIcons.Add(inventorySlots[i].GetComponentsInChildren<Image>()[1]);
            inventoryText.Add(inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>());
        }

        UpdateIventoryUI();
    }

    private int InventorySize() {
        int size = 0;
        for (int i = 0; i < inventory.Count; i++) {
            size += inventory[i].stackSize;
        }
        return size;
    }

    public void AddItem(InventoryItem item)
    {
        // If inventory is full don't add any items
        if (InventorySize() >= maxInventorySize + (PlayerController.instance.shirt ? PlayerController.instance.shirt.itemPower : 0))
            return;

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

    public void RemoveItem(int index)
    {
        // See if the item is in the inventory
        if (inventory.Count > index && itemDictionary.TryGetValue(inventory[index].item, out InventorySlot slot))
        {
            slot.stackSize--;
            // Remove the slot if there are none left
            if (slot.stackSize <= 0)
            {
                itemDictionary.Remove(inventory[index].item);
                inventory.Remove(slot);
            }

            UpdateIventoryUI();
        }
    }

    public void UpdateIventoryUI() {
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

        // Update the inventory size text
        inventorySizeText.text = InventorySize().ToString() + " / " + (maxInventorySize + (PlayerController.instance.shirt ? PlayerController.instance.shirt.itemPower : 0)).ToString();
    }

    public void PressedSlot(int index) {
        // Remove item in slot if possible
        if (inventory.Count > index && ShopkeeperManager.instance.IsRequestedItem(inventory[index].item))
        {
            ShopkeeperManager.instance.RemoveItem(inventory[index].item);
            RemoveItem(index);
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
