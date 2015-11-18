using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class customNetworkBluePrint : NetworkBehaviour {

    [SyncVar]
     bool isTwoPlayerJoin;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        isTwoPlayerJoin = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool getisTwoPlayerJoin()
    {
        return isTwoPlayerJoin;
    }
   
}
