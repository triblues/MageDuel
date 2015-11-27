using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class storyTrailer : MonoBehaviour {

    public MovieTexture myMovieTexture;
    AudioSource myAudioSource;
    bool isSkip;
    float myalpha;
    RawImage myfadeImage;
    // Use this for initialization
   
    void Start () {

        myfadeImage = GameObject.Find("fade image").GetComponent<RawImage>();
        isSkip = false;
        GetComponent<RawImage>().texture = myMovieTexture as MovieTexture;
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = myMovieTexture.audioClip;
        myMovieTexture.Play();
        myAudioSource.Play();
	}

    // Update is called once per frame
    void Update()
    {
        if (myMovieTexture.isPlaying == false)//play finish
        {
            if (isSkip == false)
            {
                isSkip = true;
                StartCoroutine(waitForFade("MainMenuGUI", 3.0f, true));
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isSkip == false)
                {
                    isSkip = true;
                    myAudioSource.Stop();
                    StartCoroutine(waitForFade("MainMenuGUI", 3.0f, true));
                }
            }
        }
        
    }
    IEnumerator waitForFade(string name, float fadeTime, bool isFadeOut)
    {
        while (true)
        {
            if (isFadeOut == false)//black to transparent
            {
                myalpha -= 0.5f;
                myfadeImage.color = new Color(0, 0, 0, myalpha);


                if (myalpha <= 0)
                {

                    break;
                }
            }
            else//transparent to black
            {
                myalpha += 0.5f;
                myfadeImage.color = new Color(0, 0, 0, myalpha);

                if (myalpha >= 1)
                {
                    Application.LoadLevel(name);
                    break;
                }
            }



            yield return new WaitForSeconds(fadeTime / 10);
        }

    }
}
