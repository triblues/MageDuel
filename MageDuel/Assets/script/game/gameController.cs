using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class gameController : MonoBehaviour
{

    public enum projectileType
    {
        fireball,
        iceball,
        lightball
    };

    public static bool isFinish;
    public GameObject[] allCharacter;
    public GameObject[] allEnemy;
    public GameObject[] allEnvironment;
    //public GameObject[] allUltimate;
    public Vector3[] spawnPos;//0 for player, 1 for enemy;

    public Transform myParent;
    public GameObject[] myProjectile;
    public int[] amount;
    public bool isTest = false;
    public bool donSpawn = true;
    public int testchar;

    public GameObject[] mystar;
    public GameObject winPanel;
    public Text healthText;
    public Text comboText;
    public gameTime mygameTime;
    public GameObject myPausePanel;
    List<poolObject> myPoolObj;

    GameObject player;
    GameObject enemy;
    GameObject myStarting;
    GameObject mygameover;
    
  

    // Use this for initialization
    void Awake()
    {
        isFinish = false;

        mygameover = GameObject.Find("gameover");
        myStarting = GameObject.Find("starting");
        myPoolObj = new List<poolObject>();
        if(isTest == true)
             characterSelectManager.selectedCharacter = testchar;
        //for (int i = 0; i < myProjectile.Length; i++)
       // {

         
            poolObject temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amount[characterSelectManager.selectedCharacter], true, myProjectile[characterSelectManager.selectedCharacter], myParent);
            myPoolObj.Add(temp);
       // }
        if (donSpawn == false)
        {
            if (isTest == true)
            {
                player = Instantiate(allCharacter[0], spawnPos[0],
                    Quaternion.identity) as GameObject;//spawn player

                enemy = Instantiate(allEnemy[0], spawnPos[1],
                  Quaternion.identity) as GameObject;//spawn enemy

                Instantiate(allEnvironment[0], Vector3.zero,
                 Quaternion.identity);//spawn environment

               // Instantiate(allUltimate[0], Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.Log("spawn plyer: " + characterSelectManager.selectedCharacter.ToString());
               player = Instantiate(allCharacter[characterSelectManager.selectedCharacter], spawnPos[0],
                   Quaternion.identity) as GameObject;//spawn player

                if (launchScene.isPractice == false)
                {
                    enemy = Instantiate(allEnemy[levelSelectController.selectedLevel - 1], spawnPos[1],
                      Quaternion.identity) as GameObject;//spawn enemy

                    Instantiate(allEnvironment[levelSelectController.selectedLevel - 1], Vector3.zero,
                       Quaternion.identity);//spawn environment
                }
                else
                {
                    enemy = Instantiate(allEnemy[0], spawnPos[1],
                      Quaternion.identity) as GameObject;//spawn enemy

                    Instantiate(allEnvironment[0], Vector3.zero,
                       Quaternion.identity);//spawn environment
                }
             

            }
        }

        healthText.fontSize = Screen.width / 30;
        comboText.fontSize = Screen.width / 30;



    }
    public void showPause(bool show)
    {
        myPausePanel.SetActive(show);
    }
    IEnumerator showWinPanel(float currentHealth, float maxHealth,int highestCombo,bool isWin)
    {
        yield return new WaitForSeconds(2.0f);
        winPanel.SetActive(true);
        healthText.text = "Health left: " + currentHealth.ToString();
        comboText.text = "HighestCombo: " + highestCombo.ToString();
        if (isWin == true)
        {
            PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
            PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString()
                + "character" + characterSelectManager.selectedCharacter.ToString(), 1);

            mystar[0].SetActive(true);//1st star
                                      
            if(PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 1.ToString() + "character" + 
                characterSelectManager.selectedCharacter.ToString()) == false)
            {
                PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 1.ToString() + "character" +
                characterSelectManager.selectedCharacter.ToString(), 1);
            }
            if (currentHealth >= maxHealth / 2)//more then half
            {
                PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                mystar[1].SetActive(true);//2nd star

                if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
               characterSelectManager.selectedCharacter.ToString()) == false)
                {
                    PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString(), 1);
                }

                if (highestCombo >= 5)
                {
                    PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                    mystar[2].SetActive(true);//3rd star

                    if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
               characterSelectManager.selectedCharacter.ToString()) == false)
                    {
                        PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
                        characterSelectManager.selectedCharacter.ToString(), 1);
                    }
                    yield return null;
                }
            }
            if (highestCombo >= 5)
            {
                PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                mystar[1].SetActive(true);//2nd star
                if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
               characterSelectManager.selectedCharacter.ToString()) == false)
                {
                    PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString(), 1);
                }

                if (currentHealth >= maxHealth / 2)//more then half
                {
                    PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                    mystar[2].SetActive(true);//3rd star
                    if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
               characterSelectManager.selectedCharacter.ToString()) == false)
                    {
                        PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
                        characterSelectManager.selectedCharacter.ToString(), 1);
                    }
                    yield return null;
                }
            }
        }
    }
    public void activateMovement()
    {
        if (donSpawn == false)
        {
            player.transform.Find("controller").GetComponent<CharacterBase>().enabled = true;
            enemy.transform.Find("controller").GetComponent<CharacterBase>().enabled = true;
        }
        mygameTime.startTimer();
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    public void showStarting()
    {
        for (int i = 0; i < myStarting.transform.childCount; i++)
        {
            myStarting.transform.GetChild(i).GetComponent<Animator>().enabled = true;
        }
    }
    public void showGameOver(float currentHealth, float maxHealth, int highestCombo, bool isWin)
    {
        for(int i=0;i< mygameover.transform.childCount;i++)
        {
            mygameover.transform.GetChild(i).GetComponent<Animator>().enabled = true;
        }
        StartCoroutine(showWinPanel(currentHealth, maxHealth, highestCombo, isWin));
    }

    public poolObject getPoolObjectInstancebyType(projectileType myType)
    {
      
      //  return myPoolObj[0];
          return myPoolObj[(int)myType];

    }
    public poolObject getPoolObjectInstance()
    {

    
        return myPoolObj[0];

    }


}
