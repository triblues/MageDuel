using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class storyText : MonoBehaviour {

    public GameObject mypanel;
    public string mystoryText;
    public float speed;
    Text mytext;
	// Use this for initialization
	void Start () {

        mytext = GetComponent<Text>();
        mytext.text = mystoryText;
    }
	
	// Update is called once per frame
	void Update () {

        mypanel.transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
