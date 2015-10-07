// Mage Duel Main Menu GUI source code
// Created : 9 September 2015
using UnityEngine;
using System.Collections;

public class launchScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Goes to the game scene
	public void gotoScene(string name)
	{
		Application.LoadLevel (name);
	}

    // Loads the game credits window
    public void loadCredits ()
    {
        Application.LoadLevel("credits");
    }

    // Loads the game settings window
    public void loadSettings ()
    {
        Application.LoadLevel("gameSettings");
    }

    // Goes back to the main menu upon clicking the "Main Menu" button in credits and settings option
    public void MainMenuGUI ()
    {
        Application.LoadLevel("MainMenuGUI");
    }

    // Quits the application
	public void quit()
	{
		Application.Quit ();
	}
}
