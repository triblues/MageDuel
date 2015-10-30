using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelSelectController : MonoBehaviour {

    public static int selectedLevel;
    public static bool[] isClearLevel = new bool[3];
    public Button bossbtn;
	// Use this for initialization
	void Start () {
	
        if(PlayerPrefs.HasKey("boss" + characterSelectManager.selectedCharacter) == true)
        {
            bossbtn.interactable = true;
        }
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void selectLevel(int level)
    {
        selectedLevel = level;

    }
}
