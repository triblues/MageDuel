using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	private mainPlayer myPlayer;
    
    

    private bool isFalling = false;
	private bool isHolding = false;


    // Initialize
    void Awake()
    {
        // Set up references.
		myPlayer = GetComponent<mainPlayer>();
        
	

    }

    // Called once per frame
    void FixedUpdate()
    {
		//myPlayer.Move ();
		//myPlayer.jump ();
	

         //If Input button is Move (A-D) then move
//        if (Input.GetButton("Move"))
//        {
//            Move();
//        }
//        // If input button is Jump (space) then jump
//        if (Input.GetButton("Jump"))
//        {
//			jump();
//            //playerRigidbody.velocity = new Vector3(0f, 7.5f, 0f);
//            //isFalling = true;
//         }
//        // If input button is Attack1 (left mouse click) then attack
        if (Input.GetButtonDown("Attack1"))
        {
            //Attack();
        }

		//transform.Translate (movement * Time.deltaTime,Space.World);
    }

    // Function to set the character's movement
    

    void Attack()
    {
		//myPlayer.shootFireBall ();
    }


}

















