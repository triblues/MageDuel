using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class networkProjectileManager : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public override void PreStartClient()
    {
        Debug.Log("pre start client");
        base.PreStartClient();
    }
    public override void OnStartClient()
    {
       
        Debug.Log("start projectile client");
        base.OnStartClient();

    }
    // Update is called once per frame
    void Update () {
	
	}
}
