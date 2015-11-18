using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class playerSyncTransform : MonoBehaviour {

    List<GameObject> myobj;
   // protected NetworkInstanceId mynetworkID;
    public GameObject go;
    //public override void OnStartLocalPlayer()
    //{
    //    base.OnStartLocalPlayer();
    //    mynetworkID = GetComponent<NetworkIdentity>().netId;
    //    Debug.Log("localplayer: " + mynetworkID.ToString());//3
    //    Cmdspawnstuff();
    //    //GameObject go2 = Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;

    //    // NetworkServer.Spawn(go2);
    //}
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    Debug.Log("start client");//2
        
    //}
    //public override void PreStartClient()
    //{
    //    Debug.Log("start pre client");//1

        
    //    base.PreStartClient();
    //}
    void Awake()
    {
        myobj = new List<GameObject>();
        Debug.Log("awake");//0
    }
    // Use this for initialization
    void Start () {
        Debug.Log("staer");//4
        //mynetworkID = GetComponent<NetworkIdentity>().netId;
        //Debug.Log("localplayer: " + isLocalPlayer.ToString());
        //if (isLocalPlayer == false)
        //{
        //    GetComponent<playerSyncTransform>().enabled = false;
        //}

        //if (int.Parse(mynetworkID.ToString()) % 2 == 0)
        //    Debug.Log("is zero: ");
        //else
        //    Debug.Log("is not zero");
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
          //  Cmdspawnstuff();
        }
    }
    //[Command]
    //void Cmdspawnstuff()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        GameObject go2 = Instantiate(go, new Vector3(0+i, 2, 4), Quaternion.identity) as GameObject;
    //        myobj.Add(go2);
    //        NetworkServer.Spawn(go2);
    //    }
    //    Debug.Log("myobj: " + myobj.Count.ToString());
    //    //NetworkServer.SpawnWithClientAuthority(go2, base.connectionToClient);    
    //}
}
