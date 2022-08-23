using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Range(0, 1)]
    [SerializeField] private float acceleration;
    [Range(0, 1)]
    [SerializeField] private float dampening;
    [SerializeField] private float maxSpeed;

    [Header("Clothing")]
    public ClothingObject shirt;
    public ClothingObject shoes;

    private GameObject shirtObject, shoesObject;
    private Animator shirtAnim, shoesAnim;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;

    [HideInInspector] public bool isLocked;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Create gameObjects from the clothingObjects and get the animators from it
        shirtObject = Instantiate(shirt.itemPrefab, transform);
        shirtAnim = shirtObject.GetComponent<Animator>();

        shoesObject = Instantiate(shoes.itemPrefab, transform);
        shoesAnim = shoesObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Store keyboard input in a Vector2
        input = new Vector2(isLocked ? 0 : Input.GetAxisRaw("Horizontal"), isLocked ? 0 : Input.GetAxisRaw("Vertical"));

        // Set all the velocities in the animator as scaled velocities.
        animator.SetFloat("VelocityX", input.x);
        animator.SetFloat("VelocityY", input.y);

        shirtAnim.SetFloat("VelocityX", input.x);
        shirtAnim.SetFloat("VelocityY", input.y);

        shoesAnim.SetFloat("VelocityX", input.x);
        shoesAnim.SetFloat("VelocityY", input.y);

        // Code doesnt go further if the player is locked
        if (isLocked)
            return;
    }

    void FixedUpdate() 
    {

        // Code doesnt go further if the player is locked
        if (isLocked)
            return;

        // Set the players velocity to a value between the current velocity and the "target" velocity to make it smoothed
        // if there is no input dampen the velocity
        rb.velocity = Vector2.Lerp(rb.velocity, input.normalized * maxSpeed, input == Vector2.zero ? dampening : acceleration);
    }
}
