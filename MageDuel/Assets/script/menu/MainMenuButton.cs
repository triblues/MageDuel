// Mage Duel credits window (back to main menu script)
// Created : 3 October 2015

using UnityEngine;
using System.Collections;

public class MainMenuButton : MonoBehaviour {

	// Executes the function of the main menu button
    public void backToMainMenu ()
    {
        Application.LoadLevel("MainMenuGUI");
    }
}
