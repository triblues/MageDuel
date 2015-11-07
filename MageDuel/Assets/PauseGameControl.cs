// Mage Duel pause game script
// Created : 2 November 2015
// Author : Shaun Kong

using UnityEngine;
using System.Collections;

public class PauseGameControl : MonoBehaviour {

    public static bool readKeys = false;
	
	// Update is called once per frame
	void Update ()
    {
	    if (readKeys = true)
        {
            keyboardListener();
        }
	}

    // Function to invoke the keyboard listener when the escape key is pressed
    void keyboardListener ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("pauseGame");
        }
    }
}
