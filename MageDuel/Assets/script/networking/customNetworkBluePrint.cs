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
    [Command]//a command to send to the server
    public void CmdSendPositionToServer(bool check)//must have Cmd as the start of the name, this function only run in the server
    {
        isTwoPlayerJoin = check;
    }
}
