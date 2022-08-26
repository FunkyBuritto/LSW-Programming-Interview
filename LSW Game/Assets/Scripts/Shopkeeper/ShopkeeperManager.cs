using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopkeeperManager : Interactable
{
    public static ShopkeeperManager instance;

    [SerializeField] private float dialogTime;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private GameObject requestPanel;
    [SerializeField] private GameObject requestPrefab;
    [SerializeField] private List<GameObject> sellButtons;
    [SerializeField] private List<GameObject> removeButtons;

    [SerializeField] private List<ShopkeeperInteraction> interactions = new List<ShopkeeperInteraction>();
    private List<InventorySlot> requestedItems = new List<InventorySlot>();
    private bool requestMade;

    [SerializeField] private int interactionCount = -1;

    void Start() {  instance = this; }

    public override void Interact() 
    {
        interactionCount++;

        // Check if we still have enough interactions
        if (interactionCount > interactions.Count - 1)
            return;

        // Reset the textbox
        if (textBox.text != "")
            textBox.text = "";

        // Check which type of interaction this is
        switch (interactions[interactionCount].type)
        {
            case InteractionType.Dialog:
                StartCoroutine(Dialog(interactions[interactionCount].dialog));
                break;
            case InteractionType.Request:
                Request(interactions[interactionCount].request);
                break;
            case InteractionType.RandomRequest:
                // Request(RandomRequest);
                Debug.Log("RandomRequest");
                break;
            default:
                break;
        }
    }

    public void UpdateRequestUI()
    {
        // Clear the pannel (for loop starts at 1 so it ignores the parrents transform)
        Transform[] children = requestPanel.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        // Renew the pannel
        for (int i = 0; i < requestedItems.Count; i++)
        {
            GameObject g = Instantiate(requestPrefab, requestPanel.transform);
            g.GetComponentInChildren<Image>().sprite = requestedItems[i].item.icon;
            g.GetComponentInChildren<TextMeshProUGUI>().text = requestedItems[i].stackSize.ToString();
        }
    }

    public void RemoveItem(InventoryItem item){
        for (int i = 0; i < requestedItems.Count; i++) 
        {
            // Check if the item has a match decrease size if there are multiple
            // it there is only 1 remove it completely
            if (requestedItems[i].item == item) {
                if (requestedItems[i].stackSize > 1)
                {
                    requestedItems[i].stackSize--;
                } else {
                    requestedItems.RemoveAt(i);

                    // If there are no more requested items move to the next interaction
                    if (requestedItems.Count == 0)
                    {
                        interactionCount++;
                        requestMade = false;
                    }
                        
                }
                UpdateRequestUI();
                return;
            }  
        }
    }

    /// <returns>True if it is one of the requested item, False if its not</returns>
    public bool IsRequestedItem(InventoryItem item) {
        for (int i = 0; i < requestedItems.Count; i++)
        {
            if (requestedItems[i].item == item)
                return true;
        } 
        return false;
    }

    private void Request(List<InventorySlot> request) 
    {
        // See if its the first time we go over this code
        if (requestMade) {
            // if we are not done with this request yet set the interaction one back so we return here
            // else we are done with this request
            if (requestedItems.Count > 0) 
                interactionCount--;
            else 
                requestMade = false;
        } else {
            // if the request is not yet made set the requested items to the right one
            requestedItems = request;
            requestMade = true;
            interactionCount--;
            UpdateRequestUI();
        }
    }

    private IEnumerator Dialog(string text)
    {
        PlayerController.instance.isLocked = true;

        // Get the time per character 
        float t = dialogTime / text.Length;

        textBox.text = "";
        // Loop over every character in the string
        for (int i = 0; i < text.Length; i++)
        {
            // Add a character to the string and wait "t" amount of seconds
            textBox.text += text[i];
            yield return new WaitForSeconds(t);
        }

        PlayerController.instance.isLocked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Toggle sell and remove buttons
            for (int i = 0; i < sellButtons.Count; i++) 
            {
                sellButtons[i].SetActive(true);
                removeButtons[i].SetActive(false);
            }
            PlayerController.instance.interactables.Add(this);
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Toggle sell and remove buttons
            for (int i = 0; i < sellButtons.Count; i++)
            {
                sellButtons[i].SetActive(false);
                removeButtons[i].SetActive(true);
            }
            PlayerController.instance.interactables.Remove(this);
        }
            
    }
}

public enum InteractionType
{
    Dialog,
    Request,
    RandomRequest
}

[System.Serializable]
class ShopkeeperInteraction
{
    public InteractionType type;
    public string dialog;
    public List<InventorySlot> request;
}


