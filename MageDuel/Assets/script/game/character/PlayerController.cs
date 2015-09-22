using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private float speed;                   // Reference to player's speed
    private Vector3 movement;                   // The vector to store the direction of the player's movement.
    //Animator anim;                      // Reference to the animator component.
    private Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

    // Initialize
    void Awake()
    {
        // Set up references.
       // anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Function to set the character's speed
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    // Function to set the character's movement
    public void Move()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, 0f);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    // Function to animate the character's movement
   /* void Animating(float h)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }
    */
}
