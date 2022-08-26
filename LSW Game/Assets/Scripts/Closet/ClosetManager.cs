using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetManager : Interactable
{
    [SerializeField] private GameObject closetMenu;

    public override void Interact() => ToggleMenu();

    private void ToggleMenu()
    {
        // Toggle the active state of the menu and the player lock
        closetMenu.SetActive(closetMenu.activeSelf ? false : true);
    }

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
        {
            PlayerController.instance.interactables.Remove(this);

            // If the menu is active when the player leaves the closet close the menu
            if (closetMenu.activeSelf)
            {
                closetMenu.SetActive(false);
                PlayerController.instance.isLocked = false;
            }
        }
    }
}
