// Mage Duel practice scene keyboard listener script
// Created : 14 October 2015
// Author : Shaun Kong
using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour {

    public bool readKeys = false;
    
	// Update is called once per frame
	void Update ()
    {
        if (readKeys == true)
            KeyboardListener();
	}

    // Executes the pause practice command if the escape key is pressed
    public void KeyboardListener ()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.LoadLevel("pausePractice");
        }
    }
}
