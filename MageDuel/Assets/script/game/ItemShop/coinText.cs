using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class coinText : MonoBehaviour {

    Text mytext;
	// Use this for initialization
	void Start () {

        mytext = GetComponent<Text>();
      
        mytext.text = "Coins: " + PlayerPrefs.GetInt("coin").ToString();
	}
	
	// Update is called once per frame
	void Update () {

        mytext.text = "Coins: " + PlayerPrefs.GetInt("coin").ToString();
    }
}
