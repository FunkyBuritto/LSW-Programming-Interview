using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

public class ShopkeeperManager : MonoBehaviour
{
    public static ShopkeeperManager instance;

    [SerializeField] private float dialogTime;
    [SerializeField] private TextMeshProUGUI textBox;

    [SerializeField] private List<ShopkeeperInteraction> interactions = new List<ShopkeeperInteraction>();
    private List<InventorySlot> requestedItems = new List<InventorySlot>();
    private bool requestMade;

    private int interactionCount = -1;

    // Start is called before the first frame update
    void Start() {  instance = this; }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact() {
        interactionCount++;

        // Check if we still have enough interactions
        if (interactionCount > interactions.Count - 1)
            return;

        // Check which type of interaction this is
        switch (interactions[interactionCount].type)
        {
            case InteractionType.Dialog:
                StartCoroutine(Dialog(interactions[interactionCount].dialog));
                break;
            case InteractionType.Request:
                Request(interactions[interactionCount].request);
                Debug.Log("Request");
                break;
            case InteractionType.RandomRequest:
                // Request(RandomRequest);
                Debug.Log("RandomRequest");
                break;
            default:
                break;
        }
    }

    void Request(List<InventorySlot> request) {
        // See if its the first time we go over this code
        if (requestMade) {
            // if we are not done with this request yet set the interaction one back so we return here
            // else we are done with this request
            if (requestedItems.Count > 0) {
                interactionCount--;
            } else {
                requestMade = false;
            }
        } else {
            // if the request is not yet made set the requested items to the right one
            requestedItems = request;
            requestMade = true;
            interactionCount--;
        }
    }

    IEnumerator Dialog(string text)
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
            PlayerController.instance.inShopkeeperRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            PlayerController.instance.inShopkeeperRange = false;
    }
}


