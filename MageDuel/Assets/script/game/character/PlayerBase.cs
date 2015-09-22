/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {
    protected PlayerController pController;

    protected int startingHealth = 100;
    protected int currentHealth;
    protected int startingMana = 100;
    protected int currentMana;

	// Use this for initialization
	void Awake () {
        pController = GetComponent<PlayerController>();

        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;
       
        SetupPlayer();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Update player movement
        pController.Move();
	}

    // Function to setup player's special attributes (speed etc)
    protected virtual void SetupPlayer()
    {
        // Set character speed to default 5.0
        pController.SetSpeed(5.0f);
    }
}
