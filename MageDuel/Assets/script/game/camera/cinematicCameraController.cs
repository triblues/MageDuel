using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cinematicCameraController : MonoBehaviour {

    public float speed = 0.5f;
    public GameObject mycinematicPanel;
    public Vector3 startingPos = new Vector3(0,2.5f,-2);
    bool[] isSkip;
    bool[] canMove;
    bool isCutSceneFinish;
    float myalpha;

    gameController myGC;
    RawImage myfadeImage;
    cinematicText mycinematicText;
    Coroutine co;
    GameObject myplayer;
    GameObject myEnemy;
    GameObject[] gameUI;
    // Use this for initialization
    void Start () {

        isCutSceneFinish = false;
        mycinematicText = GameObject.Find("cinematic text").GetComponent<cinematicText>();
        isSkip = new bool[2];
        canMove = new bool[2];
        for (int i = 0; i < isSkip.Length; i++)
        {
            canMove[i] = false;
            isSkip[i] = false;
        }
        myGC = GameObject.Find("gameManager").GetComponent<gameController>();
        myfadeImage = GameObject.Find("fade image").GetComponent<RawImage>();
        myalpha = 1;
        myfadeImage.color = new Color(0, 0, 0, myalpha);
       

        myEnemy = GameObject.FindWithTag("Enemy").gameObject;
        myplayer = GameObject.FindWithTag("Main Player").gameObject;

        gameUI = GameObject.FindGameObjectsWithTag("gameUI");
        foreach(GameObject temp in gameUI)
        {
            temp.SetActive(false);
        }
        canMove[0] = true;

        if(launchScene.isPractice == true)
        {
            myalpha = 0;
            myfadeImage.color = new Color(0, 0, 0, myalpha);
            endDialog();
            return;
        }

        co = StartCoroutine(waitForFade(0, 1.0f, false));
    }
	
	// Update is called once per frame
	void Update () {
        if (launchScene.isPractice == true)
            return;

        moveCam();
        checkSkip();
       
    }
    void moveCam()
    {
        
        if (canMove[1] == true)
        {
            transform.LookAt(myEnemy.transform);

              
        }
        else if (canMove[0] == true)
        {
            transform.LookAt(myplayer.transform);

              
        }

        if (transform.position.z < 6)
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);

        
    }
    void checkSkip()
    {
        if (isCutSceneFinish == false)
        {
            if (Input.GetMouseButtonDown(0))//left click
            {
                if(isSkip[0] == false)
                {
                    isSkip[0] = true;
                        
                   
                    StopAllCoroutines();
                    StartCoroutine(goNextCinematic(1));

                }
                else
                {
                    isSkip[1] = true;
                    isCutSceneFinish = true;

                    StopAllCoroutines();
                    StartCoroutine(goNextCinematic(-1));
                   
                }
            }

        
        }
    }
    void endDialog()
    {
       
        
        mycinematicPanel.SetActive(false);

        for (int i = 0; i < gameUI.Length; i++)
            gameUI[i].SetActive(true);
    

         myGC.activateMovement();
        myGC.showStarting();
        if (launchScene.isPractice == true)
        {
            gameObject.GetComponent<cinematicCameraController>().enabled = false;
            gameObject.GetComponent<CameraController>().enabled = true;
        }

    }
    //void startMovement()
    //{
    //    StartCoroutine(delay());
    //}
    //IEnumerator delay()
    //{
    //    yield return new WaitForSeconds(3.0f);
    //    myGC.activateMovement();
    //    Debug.Log("can move");
    //}
    IEnumerator goNextCinematic(int index)
    {

        co = StartCoroutine(waitForFade(-1, 1.0f, true));//turn screen to black
        yield return co;

        if (index >= 0)
        {
            transform.position = startingPos;
            transform.rotation = Quaternion.identity;
            canMove[index] = true;
        
        }

        mycinematicText.resetText();
        StartCoroutine(waitForFade(index, 1.0f, false));
        
       
    }
    IEnumerator waitForFade(int index, float fadeTime, bool isFadeOut)//negative index mean do not show cinematic text
    {
        while (true)
        {
            if (isFadeOut == false)//black to transparent
            {
                myalpha -= 0.1f;
                myfadeImage.color = new Color(0, 0, 0, myalpha);

                if (myalpha <= 0)
                    break;
            }
            else//transparent to black
            {
                myalpha += 0.1f;
                myfadeImage.color = new Color(0, 0, 0, myalpha);

                if (myalpha >= 1)
                    break;
            }



            yield return new WaitForSeconds(fadeTime / 10);
        }

        if (index >= 0)
        {
          //  canMove[index] = true;
            mycinematicText.startText(index);
        }

        if (isCutSceneFinish == true)
        {
            Camera.main.transform.position = startingPos;
            transform.rotation = Quaternion.identity;
            gameObject.GetComponent<cinematicCameraController>().enabled = false;
            gameObject.GetComponent<CameraController>().enabled = true;
            yield return new WaitForSeconds(1.0f);

            endDialog();
        }
      

    }
}
