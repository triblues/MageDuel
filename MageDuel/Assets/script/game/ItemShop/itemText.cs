using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class itemText : MonoBehaviour {

    public string type;
    public string infrontText;
    Text mytext;
    // Use this for initialization
    void Start () {

        mytext = GetComponent<Text>();

        mytext.text = infrontText + PlayerPrefs.GetInt( (type).ToString());
    }
	
	// Update is called once per frame
	void Update () {

        mytext.text = infrontText + PlayerPrefs.GetInt((type).ToString());
    }
}
