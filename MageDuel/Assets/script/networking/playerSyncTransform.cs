using UnityEngine;
using System.Collections;

public class playerSyncTransform : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        move();
	}
    void move()
    {
        if(Input.GetKeyDown(KeyCode.A))
        { 
            transform.Translate(-Vector3.right * 2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right * 2);
        }
    }
}
