using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class comboText : MonoBehaviour {

    Text mytext;
	// Use this for initialization
	void Start () {

        mytext = GetComponent<Text>();
        mytext.fontSize = Screen.width / 40;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
