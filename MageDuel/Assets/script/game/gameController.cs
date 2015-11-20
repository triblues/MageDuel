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
    public GameObject[] myEnemyProjectile;
    public int[] amount;
    public int[] amountEnemy;
    public bool isTest = false;
    public bool donSpawn = true;
    public int testchar;
    public int testEenemy;

    public GameObject[] mystar;
    public GameObject winPanel;
    public Text healthText;
    public Text comboText;
    public Text CoinEarnText;
    public gameTime mygameTime;
    public GameObject myPausePanel;
    List<poolObject> myPoolObj;

    GameObject player;
    GameObject enemy;
    GameObject myStarting;
    GameObject mygameover;
    GameObject myTimeOut;
    int tempRandomPractice;

    // Use this for initialization
    void Awake()
    {
        

        isFinish = false;

        mygameover = GameObject.Find("gameover");
        myStarting = GameObject.Find("starting");
        myTimeOut = GameObject.Find("timeout");
        myPoolObj = new List<poolObject>();
        if (isTest == true)
        {
            characterSelectManager.selectedCharacter = testchar;
            levelSelectController.selectedLevel = testEenemy;

            poolObject temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amount[characterSelectManager.selectedCharacter], true, myProjectile[characterSelectManager.selectedCharacter], myParent);
            myPoolObj.Add(temp);//player attack

            temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amountEnemy[levelSelectController.selectedLevel - 1], true, myEnemyProjectile[levelSelectController.selectedLevel - 1], myParent);
            myPoolObj.Add(temp);//enemy attack

        }
        else
        {
            if (launchScene.isPractice == false)
            {
                poolObject temp = gameObject.AddComponent<poolObject>();
                temp.setPoolObject(amount[characterSelectManager.selectedCharacter], true, myProjectile[characterSelectManager.selectedCharacter], myParent);
                myPoolObj.Add(temp);//player attack

                temp = gameObject.AddComponent<poolObject>();
                temp.setPoolObject(amountEnemy[levelSelectController.selectedLevel - 1], true, myEnemyProjectile[levelSelectController.selectedLevel - 1], myParent);
                myPoolObj.Add(temp);//enemy attack
            }
            else
            {
                tempRandomPractice = Random.Range(0, allEnemy.Length);
                poolObject temp = gameObject.AddComponent<poolObject>();
                temp.setPoolObject(amount[characterSelectManager.selectedCharacter], true, myProjectile[characterSelectManager.selectedCharacter], myParent);
                myPoolObj.Add(temp);//player attack

                temp = gameObject.AddComponent<poolObject>();
                temp.setPoolObject(amountEnemy[tempRandomPractice], true, myEnemyProjectile[tempRandomPractice], myParent);
                myPoolObj.Add(temp);//enemy attack
            }
        }
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
                    
                    enemy = Instantiate(allEnemy[tempRandomPractice], spawnPos[1],
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
    IEnumerator showWinPanel(float currentHealth, float enemyHealth,float maxHealth,int highestCombo,bool isWin)
    {
        yield return new WaitForSeconds(2.0f);

        if (gameTime.isTimeOut == true)
        {
            if (currentHealth > enemyHealth)
                isWin = true;
            else
                isWin = false;

        }

        winPanel.SetActive(true);
        healthText.text = "Health left: " + currentHealth.ToString();
        comboText.text = "HighestCombo: " + highestCombo.ToString();
        if (isWin == true)
        {
           

            PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
            CoinEarnText.text = "Coin earn: " + 10.ToString();

            if (levelSelectController.selectedLevel != 5)
            {
                PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString()
                    + "character" + characterSelectManager.selectedCharacter.ToString(), 1);//this mean the player clear this stage
            }
            mystar[0].SetActive(true);//1st star

            if (levelSelectController.selectedLevel == 5)//boss level
            {
                if (PlayerPrefs.HasKey("boss" + "star" +  1.ToString()
               + "character" + characterSelectManager.selectedCharacter.ToString()) == false)
                {
                    PlayerPrefs.SetInt("boss" + "star" + 1.ToString()
                + "character" + characterSelectManager.selectedCharacter.ToString(), 1);
                }
            }
            else
            {
                if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 1.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString()) == false)
                {
                    PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 1.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString(), 1);
                }
            }
            if (currentHealth >= maxHealth / 2)//more then half
            {
                PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                CoinEarnText.text = "Coin earn: " + 20.ToString();
                mystar[1].SetActive(true);//2nd star

                if (levelSelectController.selectedLevel == 5)//boss level
                {
                    if (PlayerPrefs.HasKey("boss" + "star" + 2.ToString()
               + "character" + characterSelectManager.selectedCharacter.ToString()) == false)
                    {
                        PlayerPrefs.SetInt("boss" + "star" + 2.ToString()
                    + "character" + characterSelectManager.selectedCharacter.ToString(), 1);
                    }
                }
                else
                {
                    if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString()) == false)
                    {
                        PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
                        characterSelectManager.selectedCharacter.ToString(), 1);
                    }
                }

                if (highestCombo >= 5)
                {
                    PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                    CoinEarnText.text = "Coin earn: " + 30.ToString();
                    mystar[2].SetActive(true);//3rd star

                    if (levelSelectController.selectedLevel == 5)//boss level
                    {
                        if (PlayerPrefs.HasKey("boss" + "star" + 3.ToString()
                         + "character" + characterSelectManager.selectedCharacter.ToString()) == false)
                        {
                            PlayerPrefs.SetInt("boss" + "star" + 3.ToString()
                        + "character" + characterSelectManager.selectedCharacter.ToString(), 1);
                        }
                    }
                    else
                    {


                        if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString()) == false)
                        {
                            PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
                            characterSelectManager.selectedCharacter.ToString(), 1);
                        }
                       
                    }
                   
                }
                yield return null;
            }
            if (highestCombo >= 5)
            {
                PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                CoinEarnText.text = "Coin earn: " + 20.ToString();
                mystar[1].SetActive(true);//2nd star

                if (levelSelectController.selectedLevel == 5)//boss level
                {
                    if (PlayerPrefs.HasKey("boss" + "star" + 2.ToString()
                        + "character" + characterSelectManager.selectedCharacter.ToString()) == false)
                    {
                        PlayerPrefs.SetInt("boss" + "star" + 2.ToString()
                    + "character" + characterSelectManager.selectedCharacter.ToString(), 1);
                    }
                }
                else
                {
                    if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString()) == false)
                    {
                        PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 2.ToString() + "character" +
                        characterSelectManager.selectedCharacter.ToString(), 1);
                    }
                }

                if (currentHealth >= maxHealth / 2)//more then half
                {
                    PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 10);
                    CoinEarnText.text = "Coin earn: " + 30.ToString();
                    mystar[2].SetActive(true);//3rd star

                    if (levelSelectController.selectedLevel == 5)//boss level
                    {
                        if (PlayerPrefs.HasKey("boss" + "star" + 3.ToString()
                        + "character" + characterSelectManager.selectedCharacter.ToString()) == false)
                        {
                            PlayerPrefs.SetInt("boss" + "star" + 3.ToString()
                        + "character" + characterSelectManager.selectedCharacter.ToString(), 1);
                        }
                    }
                    else
                    {
                        if (PlayerPrefs.HasKey("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
                    characterSelectManager.selectedCharacter.ToString()) == false)
                        {
                            PlayerPrefs.SetInt("level" + levelSelectController.selectedLevel.ToString() + "star" + 3.ToString() + "character" +
                            characterSelectManager.selectedCharacter.ToString(), 1);
                        }
                    }
                    
                }
                yield return null;
            }
            
        }
        else
        {
            CoinEarnText.text = "Coin earn: 0";
        }
    }
    public void activateMovement()
    {
        if (donSpawn == false)
        {
            player.transform.Find("controller").GetComponent<CharacterBase>().enabled = true;
            enemy.transform.Find("controller").GetComponent<CharacterBase>().enabled = true;
        }
        if (launchScene.isPractice == false)
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
    public void showGameOver(float currentHealth, float enemyHealth, float maxHealth, int highestCombo, bool isWin)
    {
        if (gameTime.isTimeOut == true)
        {
            for (int i = 0; i < myTimeOut.transform.childCount; i++)
            {
                myTimeOut.transform.GetChild(i).GetComponent<Animator>().enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < mygameover.transform.childCount; i++)
            {
                mygameover.transform.GetChild(i).GetComponent<Animator>().enabled = true;
            }
        }
        //if(levelSelectController.selectedLevel == 5)//boss level
        //{

        //}
        //else
            StartCoroutine(showWinPanel(currentHealth, enemyHealth,maxHealth, highestCombo, isWin));
    }

    public poolObject getPoolObjectInstancebyType(projectileType myType)
    {
      
      //  return myPoolObj[0];
          return myPoolObj[(int)myType];

    }
    public poolObject getPoolObjectInstance(int num)
    {

    
        return myPoolObj[num];

    }


}
