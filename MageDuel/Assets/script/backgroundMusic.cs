using UnityEngine;
using System.Collections;

public class backgroundMusic : MonoBehaviour {

    public AudioClip[] myaudioclip;
    AudioSource myAudioSource;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = myaudioclip[0];//default background music
        myAudioSource.Play();

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
                myAudioSource.clip = myaudioclip[0];
                myAudioSource.Play();
            }
        }
    }
}
