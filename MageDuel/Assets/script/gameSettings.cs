﻿// Mage Duel in-game settings
// Created : 6 October 2015
// Author : Shaun Kong
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gameSettings : MonoBehaviour {

    public static float myVolume = 1.0f;
    public static int myDifficulty = 1;
    public Slider myslider;
    int width;
    int height;

    bool fullscreen = false;

    void Start()
    {
        myslider.value = myVolume;
    }

    // Changes the screen resolution to 800 x 600 pixels
    public void changeTo800by600pixels ()
    {
        Screen.SetResolution(Screen.width, 600, false);
    }

    // Changes the screen resolution to 1024 x 768 pixels
    public void changeTo1024by768pixels ()
    {
        Screen.SetResolution(1024, 768, false);
    }

    // Changes the game to full screen
    public void changeToFullScreen (int width, int height)
    {
        Screen.fullScreen = true;
    }

    // Changes the game to windowed screen
    public void changeToWindowedScreen (int width, int height, bool fullscreen)
    {
        Screen.fullScreen = false;
    }
    public void changeWindowMode(bool isFullScreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, isFullScreen);
    }
    public void setVolume()
    {
        myVolume = myslider.value;
    }
    public void setDifficulty(int num)
    {
        myDifficulty = num;
    }
   
}
