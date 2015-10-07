// Mage Duel game settings GUI
// Created : 30 September 2015
using UnityEngine;
using System.Collections;

public class settingsGUI : MonoBehaviour {

    public GUIStyle whiteStyle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Draws the settings GUI window
    void onGUI()
    {
        GUI.Box(new Rect(Screen.width * .15f, Screen.height * .2f, Screen.width * .75f, Screen.height),
                 "RESOLUTION:\n\n\n" +
                 "\n\n\nVOLUME:" +
                 "\n\n\n", whiteStyle);
    }
}
