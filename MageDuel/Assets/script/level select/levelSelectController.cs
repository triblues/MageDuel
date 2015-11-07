using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelSelectController : MonoBehaviour {

    public static int selectedLevel;
    public Button bossbtn;
    bool hasBoss;
	// Use this for initialization
	void Start () {

        hasBoss = true;
        if (PlayerPrefs.HasKey("boss" + characterSelectManager.selectedCharacter) == false)
        {
            for (int i = 1; i <= 4; i++)
            {
                // PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString()
                //   +"character" + characterSelectManager.selectedCharacter.ToString(), 1);
                if (PlayerPrefs.HasKey("level" + i.ToString() + "character" + characterSelectManager.selectedCharacter.ToString()) == false)
                {
                    hasBoss = false;
                    break;
                }
            }
            if (hasBoss == true)
            {
                bossbtn.interactable = true;
                PlayerPrefs.SetInt("boss" + characterSelectManager.selectedCharacter,1);
            }
        }
        else
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
