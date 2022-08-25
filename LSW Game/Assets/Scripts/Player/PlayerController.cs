using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public int money;

    [Header("Movement")]
    [SerializeField] [Range(0, 1)] private float acceleration;
    [SerializeField] [Range(0, 1)] private float dampening;
    [SerializeField] private float maxSpeed;

    [Header("Clothing")]
    public ClothingItem shirt;
    public ClothingItem shoes;

    private GameObject shirtObject, shoesObject;
    private Animator shirtAnim, shoesAnim;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;
    
    [HideInInspector] public List<ItemObject> itemObjects = new List<ItemObject>();
    [HideInInspector] public bool isLocked;
    [HideInInspector] public bool inShopkeeperRange;

    private void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Create gameObjects from the clothingObjects and get the animators from it
        if (shirtObject) {
            shirtObject = Instantiate(shirt.clothingPrefab, transform);
            shirtAnim = shirtObject.GetComponent<Animator>();
        }

        if (shoesObject) {
            shoesObject = Instantiate(shoes.clothingPrefab, transform);
            shoesAnim = shoesObject.GetComponent<Animator>();
        }

    }

    private void Update()
    {
        // Store keyboard input in a Vector2
        input = new Vector2(isLocked ? 0 : Input.GetAxisRaw("Horizontal"), isLocked ? 0 : Input.GetAxisRaw("Vertical"));

        UpdateAnimators();

        // Code doesnt go further if the player is locked
        if (isLocked)
            return;

        // Check if we press the interact button
        if (Input.GetKeyDown(KeyCode.E)) {

            // if we are in Shopkeeper Range we dont search for an item
            if (inShopkeeperRange) {
                ShopkeeperManager.instance.Interact();
                return;
            }

            // Get the item closest to the player that is in range
            ItemObject shortest = null;
            float dist = 0;

            for (int i = 0; i < itemObjects.Count; i++)
            {
                float d = Mathf.Abs((itemObjects[i].transform.position - transform.position).sqrMagnitude);
                if(d > dist) {
                    shortest = itemObjects[i];
                    dist = d;
                }
            }

            // if we have an item that is the shortest to us add it to our inventory
            if (shortest != null)
                InventoryManager.instance.AddItem(shortest.item);
        }
    }

    private void FixedUpdate() 
    {
        // Code doesnt go further if the player is locked
        if (isLocked) {
            rb.velocity = Vector2.zero;
            return;
        }

        // Set the players velocity to a value between the current velocity and the "target" velocity to make it smoothed
        // if there is no input dampen the velocity
        rb.velocity = Vector2.Lerp(rb.velocity, input.normalized * maxSpeed, input == Vector2.zero ? dampening : acceleration);
    }

    private void UpdateAnimators()
    {
        // Set all the velocities in the animator as scaled velocities
        animator.SetFloat("VelocityX", input.x);
        animator.SetFloat("VelocityY", input.y);

        if (shirtAnim) {
            shirtAnim.SetFloat("VelocityX", input.x);
            shirtAnim.SetFloat("VelocityY", input.y);
        }

        if (shoesAnim) {
            shoesAnim.SetFloat("VelocityX", input.x);
            shoesAnim.SetFloat("VelocityY", input.y);
        }
    }
}
