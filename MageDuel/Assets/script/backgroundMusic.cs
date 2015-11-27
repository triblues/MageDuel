using UnityEngine;
using System.Collections;

public class backgroundMusic : MonoBehaviour {

    public AudioClip[] myaudioclip;
    private static bool created = false;
    AudioSource myAudioSource;
    void Awake()
    {
        if (created == false)
        {
            DontDestroyOnLoad(this.gameObject);
            myAudioSource = GetComponent<AudioSource>();
            myAudioSource.clip = myaudioclip[0];//default background music
            myAudioSource.Play();
            created = true;
        }
        else
        {
            Debug.Log("here 0");
            myAudioSource = GetComponent<AudioSource>();
            // this.gameObject.GetComponent<backgroundMusic>().enabled = false;
            Destroy(this.gameObject);//duplicate
        }

        if (PlayerPrefs.HasKey("Power Level fire") == false)
        {
            PlayerPrefs.SetInt("Power Level fire", 1);
        }
        if (PlayerPrefs.HasKey("Power Level ice") == false)
        {
            PlayerPrefs.SetInt("Power Level ice", 1);
        }
        if (PlayerPrefs.HasKey("Power Level light") == false)
        {
            PlayerPrefs.SetInt("Power Level light", 1);
        }
        if(PlayerPrefs.HasKey("Difficulty") == false)
        {
            PlayerPrefs.SetInt("Difficulty", 1);//default is easy
        }

    }
	// Use this for initialization
	void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnLevelWasLoaded(int level)
    {
       
      
        myAudioSource.volume = gameSettings.myVolume;
        if (level == 4)//battle scene
        {
            if (launchScene.isPractice == true)
            {
               
                myAudioSource.clip = myaudioclip[1];
                myAudioSource.Play();
 
            }
            else
            {
                if(levelSelectController.selectedLevel == 5)//boss level
                {
                    myAudioSource.clip = myaudioclip[2];
                    myAudioSource.Play();
                }
                else
                {
                    myAudioSource.clip = myaudioclip[1];
                    myAudioSource.Play();
                }
            }
        }
        else if(level == 8)//multiplayer scene
        {
            myAudioSource.clip = myaudioclip[1];
            myAudioSource.Play();
        }
        else
        {
            if (myAudioSource.clip != myaudioclip[0])
            {
                Debug.Log("play music");
                myAudioSource.clip = myaudioclip[0];
                myAudioSource.Play();
            }
        }
    }
}
