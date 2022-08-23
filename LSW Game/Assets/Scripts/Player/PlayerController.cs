using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float acceleration;
    [Range(0, 1)]
    [SerializeField] private float dampening;
    [SerializeField] private float maxSpeed;

    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Store keyboard input in a Vector2
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate() 
    {
        // Set the players velocity to a value between the current velocity and the "target" velocity to make it smoothed
        // if there is no input dampen the velocity
        rb.velocity = Vector2.Lerp(rb.velocity, input.normalized * maxSpeed, input == Vector2.zero ? dampening : acceleration);
    }
}
