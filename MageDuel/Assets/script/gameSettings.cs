// Mage Duel in-game settings
// Created : 6 October 2015
// Author : Shaun Kong
using UnityEngine;
using System.Collections;

public class gameSettings : MonoBehaviour {

    int width;
    int height;

    bool fullscreen = false;

    // Changes the screen resolution to 800 x 600 pixels
    public void changeTo800by600pixels ()
    {
        Screen.SetResolution(800, 600, false);
    }

    // Changes the screen resolution to 1024 x 768 pixels
    public void changeTo1024by768pixels ()
    {
        Screen.SetResolution(1024, 768, false);
    }

    // Changes the game to full screen
    public void changeToFullScreen (int width, int height)
    {
        Screen.SetResolution(width, height, true);
    }

    // Changes the game to windowed screen
    public void changeToWindowedScreen (int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, false);
    }
}
