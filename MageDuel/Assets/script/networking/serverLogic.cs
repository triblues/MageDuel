using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class serverLogic : NetworkBehaviour {

    GameObject[] mystar;
  
  

    [SyncVar]
    bool isFinish;
    [SyncVar]
    int mygametime;
    [SyncVar]
    bool isTwoPlayerJoin;
    [SyncVar]
    bool isInUltimateServer;


    serverTime myserverTime;

    public static bool hasSpawn;
    
    customNetworkManager CNM;

    Text countDownText;
    GameObject winPanel;
    GameObject myStarting;
    GameObject mygameover;
    GameObject myTimeOut;
    GameObject waitForPlayer;
    GameObject myCanvas;
    Text healthText;
    Text comboText;

    // Use this for initialization
    void Start () {

        

        
        mygametime = 100;

        myCanvas = GameObject.Find("Canvas");
        winPanel = myCanvas.transform.Find("win panel").gameObject;

        healthText = winPanel.transform.Find("health").gameObject.GetComponent<Text>();
        comboText = winPanel.transform.Find("combo").gameObject.GetComponent<Text>();
        countDownText = winPanel.transform.Find("countDown").GetComponent<Text>();

        mystar = new GameObject[3];
        for (int i = 0; i < mystar.Length; i++)
        {
            mystar[i] = winPanel.transform.Find("star achieve/star " + (i + 1).ToString()).gameObject;
        }

        mygameover = GameObject.Find("gameover");
        myStarting = GameObject.Find("starting");
        myTimeOut = GameObject.Find("timeout");
        waitForPlayer = GameObject.Find("waiting text");

        //myserverTime = GameObject.Find("time").GetComponent<serverTime>();
        CNM = GameObject.Find("networkController").GetComponent<customNetworkManager>();
        hasSpawn = false;
     
        isInUltimateServer = false;
      //  healthText.fontSize = Screen.width / 30;
      //  comboText.fontSize = Screen.width / 30;

    }
	
	// Update is called once per frame
	void Update () {

        if (isTwoPlayerJoin == true && hasSpawn == false)
        {
            hasSpawn = true;
            StartCoroutine( startGame());//show the start game text
            waitForPlayer.SetActive(false);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControllerNetworkTest>().enabled = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSyncTransform>().enabled = true;

            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControllerNetwork>().enabled = true;

            //  myserverTime.startCountDown();
            // StartCoroutine(countDownTimer());

          
        }

        //if(isFinish == true)
        //{
        //    if(hasEnterGameOver == false)
        //    {
        //        hasEnterGameOver = true;
        //        gameOver();
        //    }
        //}
	}
    public bool getisInUltimateServer()
    {
        return isInUltimateServer;
    }
    [ServerCallback]
    public void setisInUltimateServer(bool _isInUltimateServer)
    {
        isInUltimateServer = _isInUltimateServer;
    }
    public bool getIsFinish()
    {
        return isFinish;
    }
    [ServerCallback]
    public void setIsFinish(bool _isFinish)
    {
        isFinish = _isFinish;
        //if(isFinish == true)
        //{
        //    for (int i = 0; i < mygameover.transform.childCount; i++)
        //    {
        //        mygameover.transform.GetChild(i).GetComponent<Animator>().enabled = true;
               
        //    }
        //}
    }
    public int getGameTime()
    {
        return mygametime;
    }
    public void setIsTwoPlayerJoin(bool check)
    {
        isTwoPlayerJoin = check;    
    }

   
    IEnumerator countDownTimer()
    {
        while(mygametime > 0)
        {
            mygametime--;
            sendTimer(mygametime);
            yield return new WaitForSeconds(1.0f);
        }
        setIsFinish(true);
    }

    [ServerCallback]
    void sendTimer(int _gametime)
    {
        mygametime = _gametime;
    }
    
    IEnumerator startGame()
    {
        yield return new WaitForSeconds(4.0f);
        for (int i = 0; i < myStarting.transform.childCount; i++)
        {
            myStarting.transform.GetChild(i).GetComponent<Animator>().enabled = true;
        }
    }
    public void showGameOver(float currentHealth, float enemyHealth, float maxHealth, int highestCombo, bool isWin)
    {
        for (int i = 0; i < mygameover.transform.childCount; i++)
        {
            mygameover.transform.GetChild(i).GetComponent<Animator>().enabled = true;

        }
        StartCoroutine(showWinPanel(currentHealth, enemyHealth, maxHealth, highestCombo, isWin));
      
    }

    IEnumerator showWinPanel(float currentHealth, float enemyHealth, float maxHealth, int highestCombo, bool isWin)
    {
        yield return new WaitForSeconds(2.0f);

        winPanel.SetActive(true);
        healthText.text = "Health left: " + currentHealth.ToString();
        comboText.text = "HighestCombo: " + highestCombo.ToString();
        //if (mygametime <= 0)
        //{
        //    if (currentHealth > enemyHealth)
        //        isWin = true;
        //    else
        //        isWin = false;
          
        //}

        if (isWin == true)
        {
            mystar[0].SetActive(true);
            if(currentHealth >= maxHealth/2)
            {
                mystar[1].SetActive(true);

                if(highestCombo >= 5)
                {
                    mystar[2].SetActive(true);
                }
            }
            else
            {
                if (highestCombo >= 5)
                {
                    mystar[1].SetActive(true);
                }
            }

        }
        else
        {

        }
        StartCoroutine(countDown(5));

    }
    public IEnumerator countDown(int num)
    {
       // yield return new WaitForSeconds(num);
        while (num > 0)
        {
            countDownText.text = "Disconnect in " + num.ToString();
            num--;
            yield return new WaitForSeconds(1.0f);
        }
        if (isServer == true)
            CNM.stopMyHost();//disconnect both side
    }
}
