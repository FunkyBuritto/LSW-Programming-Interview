using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Interactable
{
    public InventoryItem item;

    public override void Interact() => InventoryManager.instance.AddItem(item);

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // Add this object to the players "inrange" list
        if (collision.CompareTag("Player"))
            PlayerController.instance.interactables.Add(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove this object from the players "inrange" list
        if (collision.CompareTag("Player"))
            PlayerController.instance.interactables.Remove(this);
    }
}
