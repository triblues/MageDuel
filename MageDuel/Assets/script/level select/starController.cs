using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class starController : MonoBehaviour {

    public Image[] levelOne;
    public Image[] levelTwo;
    public Image[] levelThree;
    public Image[] levelFour;
    public Image[] bossLevel;
    // Use this for initialization
    void Start () {
	
        for(int i=0;i<4;i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (PlayerPrefs.HasKey("level" + (i + 1).ToString() + "star" + (j + 1).ToString() + 
                    "character" + characterSelectManager.selectedCharacter.ToString()) == true)
                {
                    if(i == 0)
                    {
                        levelOne[j].color = new Color(1, 1, 1, 1);
                    }
                    else if (i == 1)
                    {
                        levelTwo[j].color = new Color(1, 1, 1, 1);
                    }
                    else if (i == 2)
                    {
                        levelThree[j].color = new Color(1, 1, 1, 1);
                    }
                    else if (i == 3)
                    {
                        levelFour[j].color = new Color(1, 1, 1, 1);
                    }
                }
            }
        }

        for (int i = 0; i < bossLevel.Length; i++)
        {
            if (PlayerPrefs.HasKey("boss" + "star" + (i + 1).ToString() 
                + "character" + characterSelectManager.selectedCharacter.ToString()) == true)
            {
                bossLevel[i].color = new Color(1, 1, 1, 1);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
