using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class networkGameManager : NetworkBehaviour {

    public GameObject checkObj;
    public GameObject myCam;
    public GameObject myUltiCam;
    customNetworkManager mycustomNetworkManager;
    //public GameObject projectile;
    serverLogic myserverLogic;
    
    bool gotEnter;
    // Use this for initialization
    void Start () {

        gotEnter = false;
        mycustomNetworkManager = GameObject.Find("networkController").GetComponent<customNetworkManager>();
    }
   
    public override void OnStartServer()
    {
        GameObject go = MonoBehaviour.Instantiate(checkObj, transform.position, Quaternion.identity) as GameObject;

        NetworkServer.Spawn(go);

        myserverLogic = go.GetComponent<serverLogic>();

        go = MonoBehaviour.Instantiate(myCam, new Vector3(0,2.5f,-2.0f), Quaternion.identity) as GameObject;

        NetworkServer.Spawn(go);

        go = MonoBehaviour.Instantiate(myUltiCam, new Vector3(0, 2.5f, 2.0f), Quaternion.identity) as GameObject;

        NetworkServer.Spawn(go);
        //
        Debug.Log("start server");
        

        base.OnStartServer();
    }
    public override void OnStartClient()
    {
        //Debug.Log("start client: " + characterSelectManager.selectedCharacter.ToString());
        //for (int i = 0; i < 3; i++)
        //{
        //    GameObject go = MonoBehaviour.Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        //    go.transform.SetParent(transform);
        //    NetworkServer.Spawn(go);
        //}
        //NetworkServer.SpawnObjects();
        Debug.Log("start client");
        base.OnStartClient();

    }
    // Update is called once per frame
    void Update () {

        if (gotEnter == true)
            return;
        if (isServer == true)
        {
            if (customNetworkManager.count > 1)
            {
                enter();
                gotEnter = true;
            }
        }
	}
    [ServerCallback]
    void enter()
    {
        myserverLogic.setIsTwoPlayerJoin(true);
    }
   
  
}
