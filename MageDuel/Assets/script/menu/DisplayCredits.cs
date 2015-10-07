// Mage Duel Game credits screen
// Created : 25 September 2015
// Author : Shaun Kong
using UnityEngine;
using System.Collections;

public class DisplayCredits : MonoBehaviour {

    public GUIStyle whiteStyle;

    public static DisplayCredits instance
    {
        private set;
        get;
    }
   
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy (this);
        }

        enabled = false;
    }
    
    // Initializes the text for the credits
    void Start ()
    {
        whiteStyle.normal.textColor = Color.white;
        whiteStyle.fontSize = (int)(Screen.height * .025f);
    }


    // Update is called once per frame
    void Update ()
    {
	    // Not required as credits screen merely shows
	}

    // Displays a list of the game developers
    void onGUI ()
    {
        GUI.Box(new Rect(Screen.width * .07f, Screen.height * .7f,
                          Screen.width * .1f, Screen.height * .2f),
                "CREDITS\n\n" +
           "PROJECT MANAGER : NG CHAN YIAP\n\n" +
           "LEAD PROGRAMMER : LOW WEI LIANG\n\n" +
           "LEAD ARTIST : HSU YEE HTIKE\n\n" +
            "GUI DEVELOPER : SHAUN KONG\n\n" +
            "ASSISTANT PROGRAMMER : MATIUS N", whiteStyle);

    }
}
