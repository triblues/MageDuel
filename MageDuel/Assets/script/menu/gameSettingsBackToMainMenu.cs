// Mage Duel settings back to main menu script
// Created : 3 October 2015
// Author : Shaun Kong
using UnityEngine;
using System.Collections;

public class gameSettingsBackToMainMenu : MonoBehaviour {

	// Executes the back button
    public void settingsToMainMenu ()
    {
        Application.LoadLevel("MainMenuGUI");
    }
}
