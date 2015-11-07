// Mage Duel pause game options script
// Created : 2 November 2015
// Author : Shaun Kong

using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Go back to the main menu if Yes is chosen
    void backToMainMenu ()
    {
        Application.LoadLevel("MainMenuGUI");
    }
}
