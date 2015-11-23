// Mage Duel in-game settings
// Created : 6 October 2015
// Author : Shaun Kong
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gameSettings : MonoBehaviour {

    public Color myPressedColor;
    public Color myNormalColor;
    public static float myVolume = 1.0f;
    public Button[] myDifficultyObj;
    // public static int myDifficulty = 1;
    public Slider myslider;
    int width;
    int height;

    bool fullscreen = false;

    void Start()
    {
        if (PlayerPrefs.HasKey("Difficulty") == true)
        {
            ColorBlock cb = myDifficultyObj[PlayerPrefs.GetInt("Difficulty") - 1].colors;
            cb.normalColor = myPressedColor;
            myDifficultyObj[PlayerPrefs.GetInt("Difficulty") - 1].colors = cb;
        }
       
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
        PlayerPrefs.SetInt("Difficulty", num);

        for(int i=0;i<myDifficultyObj.Length;i++)
        {
            if(i != PlayerPrefs.GetInt("Difficulty")-1)
            {
                ColorBlock cb = myDifficultyObj[i].colors;
                cb.normalColor = myNormalColor;
                myDifficultyObj[i].colors = cb;
            }
            else
            {
                ColorBlock cb = myDifficultyObj[i].colors;
                cb.normalColor = myPressedColor;
                myDifficultyObj[i].colors = cb;
            }
        }
        
        //myDifficulty = num;
    }
   
}
