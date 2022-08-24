using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItem item;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // Add this object to the players "inrange" list
        if (collision.CompareTag("Player"))
            PlayerController.instance.itemObjects.Add(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove this object from the players "inrange" list
        if (collision.CompareTag("Player"))
            PlayerController.instance.itemObjects.Remove(this);
    }
}
