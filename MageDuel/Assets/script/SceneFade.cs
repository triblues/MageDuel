// Mage Duel Game scene fade in/out script
// Created : 17 October 2015
// Author : Shaun Kong

using UnityEngine;
using System.Collections;

public class SceneFade : MonoBehaviour {

    public Texture2D fadeOutTexture;                // This is the texture overlay for the screen which can be a black image or loading graphic
    public float fadeSpeed = 0.8f;                  // The fade speed for the scene

    public int drawDepth = -1000;                   // This is the texture's order in the draw hierarchy, a low number means it renders on top
    public float alpha = 1.0f;                      // This is the texture's alpha value between 0 and 1
    public int fadeDirection = -1;                  // This is the fade direction, -1 indicates fade in while 1 indicates fade out

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Draws the fade screen GUI
    void onGUI ()
    {
        alpha += fadeDirection * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        // Sets the colour of the fade GUI
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    // Sets fade direction to the direction parameter making the scene fade in if value is -1 and out if value is 1
    public float beginFade (int direction)
    {
        fadeDirection = direction;
        return (fadeSpeed);
    }

    // This function is called whenever a level is loaded so that the the fade in can be limited to certain scenes
    void OnLevelWasLoaded ()
    {
        beginFade(-1);
    }
}
