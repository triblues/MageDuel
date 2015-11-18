using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class networktestassign : MonoBehaviour {

    public Button mm;
    public Button create;
    networktest mynetworktest;
    // Use this for initialization
    void Start () {

        mynetworktest = GameObject.Find("network bla").GetComponent<networktest>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void myassign()
    {
        mynetworktest.createmymatch();
       // mm.onClick.AddListener(() => mynetworktest.mymm());
       // create.onClick.AddListener(() => mynetworktest.createmymatch());
    }
}
