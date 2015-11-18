using UnityEngine;
using System.Collections;

public class networkPlayerController : MonoBehaviour {

    
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        checkTwoPlayer();
    }
    void checkTwoPlayer()
    {
        if(serverLogic.hasSpawn == true)
        {
            GetComponent<CharacterBaseNetwork>().enabled = true;
            transform.Find("model").gameObject.SetActive(true);
            GetComponent<networkPlayerController>().enabled = false;
        }
    }
}
