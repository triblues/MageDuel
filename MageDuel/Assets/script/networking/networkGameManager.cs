using UnityEngine;
using System.Collections;

public class networkGameManager : MonoBehaviour {

    customNetworkManager myCNM;
	// Use this for initialization
	void Start () {

        myCNM = GameObject.Find("networkController").GetComponent<customNetworkManager>();
        myCNM.spawnPlayer();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
