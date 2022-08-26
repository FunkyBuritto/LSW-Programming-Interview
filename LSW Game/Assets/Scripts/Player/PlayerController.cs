using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public int Money
    {
        get { return money; }
        set 
        {
            money = value;
            UpdateMoneyText();
        }
    }
    private int money;
    [SerializeField] private TextMeshProUGUI moneyText;

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
    
    [HideInInspector] public List<Interactable> interactables = new List<Interactable>();
    [HideInInspector] public bool isLocked;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

            // Get the item closest to the player that is in range
            Interactable shortest = null;
            float dist = 0;

            for (int i = 0; i < interactables.Count; i++)
            {
                float d = Mathf.Abs((interactables[i].transform.position - transform.position).sqrMagnitude);
                if(d > dist) {
                    shortest = interactables[i];
                    dist = d;
                }
            }

            // Interact with the interacteble clossest to us
            if (shortest != null)
                shortest.Interact();
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
        rb.velocity = Vector2.Lerp(rb.velocity, input.normalized * (maxSpeed + (shoes ? shoes.itemPower : 0)), input == Vector2.zero ? dampening : acceleration);
    }

    private void UpdateMoneyText() {
        moneyText.text = "$" + money;
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

    public void UpdateClothing()
    {
        // Destroy Current clothing 
        GameObject[] clothing = GameObject.FindGameObjectsWithTag("Clothing");
        for (int i = 0; i < clothing.Length; i++)
        {
            Destroy(clothing[i]);
        }

        // Create gameObjects from the clothingObjects and udate values
        if (shirt)
        {
            shirtObject = Instantiate(shirt.clothingPrefab, transform);
            shirtObject.GetComponent<SpriteRenderer>().color = shirt.clothingColor;
            shirtAnim = shirtObject.GetComponent<Animator>();
            shirtAnim.Play("RunUp");
        }

        if (shoes)
        {
            shoesObject = Instantiate(shoes.clothingPrefab, transform);
            shoesObject.GetComponent<SpriteRenderer>().color = shoes.clothingColor;
            shoesAnim = shoesObject.GetComponent<Animator>();
            shoesAnim.Play("RunUp");
        }

        // Syncing up the animations
        animator.Play("RunUp");
    }
}
