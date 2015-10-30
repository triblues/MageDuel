// Mage Duel Main Menu GUI source code
// Created : 9 September 2015
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class launchScene : MonoBehaviour {

    RawImage myfadeImage;
    float myalpha;
    Coroutine co;
    // Use this for initialization
    void Awake () {

        myalpha = 1;
        myfadeImage = GameObject.Find("fade image").GetComponent<RawImage>();
        myfadeImage.color = new Color(0, 0, 0, myalpha);
       
    }
	void Start()
    {
       
        co = StartCoroutine(waitForFade(name, 1.0f, false));
    }
	// Update is called once per frame
	void Update () {
	
	}

    // Goes to the game scene
	public void gotoScene(string name)
	{
        // Fade out game and load level
        // float fadeTime = GameObject.Find("FadeController").GetComponent<Fading>().BeginFading(1);
        // yield return new WaitForSeconds(fadeTime);

        // Application.LoadLevel (name);
       
        if (co != null)
            StopCoroutine(co);
        StartCoroutine(goScene(name));
	}

   
    

    IEnumerator goScene(string name)
    {
        yield return StartCoroutine(waitForFade(name, 1.0f,true));
        Application.LoadLevel(name);
    }
    IEnumerator waitForFade(string name,float fadeTime,bool isFadeOut)
    {
        while(true)
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
            

           
            yield return new WaitForSeconds(fadeTime/10);
        }
    }
    //void OnLevelWasLoaded(int level)
    //{
    //    Debug.Log("in");
    //    StartCoroutine(waitForFade(name,1.0f,false));
    //}


    // Loads the game credits window
    //public void loadCredits ()
    //{
    //    Application.LoadLevel("credits");
    //}

    //// Loads the game settings window
    //public void loadSettings ()
    //{
    //    Application.LoadLevel("gameSettings");
    //}

    //// Goes back to the main menu upon clicking the "Main Menu" button in credits and settings option
    //public void MainMenuGUI ()
    //{
    //    Application.LoadLevel("MainMenuGUI");
    //}

    // Quits the application
    public void quit()
	{
		Application.Quit ();
	}
}
