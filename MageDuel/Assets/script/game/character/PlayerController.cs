using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private PlayerBase player;

    private float speed;                   // Reference to player's speed
    private Vector3 movement;                   // The vector to store the direction of the player's movement.
    private Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

    private bool isFalling = false;


    // Initialize
    void Awake()
    {
        // Set up references.
        player = GetComponent<PlayerBase>();
       // anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Called once per frame
    void FixedUpdate()
    {
        // If Input button is Move (A-D) then move
        if (Input.GetButton("Move"))
        {
            Move();
        }
        // If input button is Jump (space) then jump
        if (Input.GetButton("Jump") && isFalling == false)
        {
            playerRigidbody.velocity = new Vector3(0f, 7.5f, 0f);
            isFalling = true;
         }
        // If input button is Attack1 (mouse 0) then attack
        if (Input.GetButton("Attack1"))
        {
            Attack();
        }
    }

    // Function to set the character's movement
    void Move()
    {
        
        // Store the input axes.
        float h = Input.GetAxisRaw("Move");
        // Get player speed
        speed = player.GetSpeed();

        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, 0f);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);


        /*
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            if (!faceLeft)
            {
                transform.RotateAround(transform.position, transform.up, 180.0f);
                faceLeft = true;
                faceRight = false;
            }

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (!faceRight)
            {
                transform.RotateAround(transform.position, transform.up, 180.0f);
                faceLeft = false;
                faceRight = true;
            }

        }
        */
    }

    void Attack()
    {

    }

    // If the body collides, isFalling is false
    void OnCollisionStay()
    {
        isFalling = false;
    }
}
