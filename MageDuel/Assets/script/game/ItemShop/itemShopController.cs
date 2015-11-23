using UnityEngine;
using System.Collections;

public class itemShopController : MonoBehaviour {

    public GameObject panel;
    public AudioClip myclickSound;
    AudioSource myAudioSource;
    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {

        myAudioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {

    }
    public void togglePanel(bool _active)
    {
        clickSound();
        panel.SetActive(_active);
    }
    public void clickSound()
    {
        myAudioSource.PlayOneShot(myclickSound);
    }
    //public void buyItem(string name,int amount)
    //{
    //    if(PlayerPrefs.GetInt("coin") >= amount)
    //    {
    //        PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") - amount);
    //        PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
    //    }
    //}
}
