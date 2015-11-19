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
       
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnLevelWasLoaded(int level)
    {
        if(level == 4)//battle scene
        {
            if (myAudioSource.clip != myaudioclip[1])
            {
                myAudioSource.clip = myaudioclip[1];
                myAudioSource.Play();
            }
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
