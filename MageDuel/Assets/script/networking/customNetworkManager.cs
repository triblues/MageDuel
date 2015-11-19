using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class MsgTypes
{
    public const short PlayerPrefab = MsgType.Highest + 1;

    public class PlayerPrefabMsg : MessageBase
    {
        public short controllerID;
        public short prefabIndex;
    }
}

public class customNetworkManager : NetworkManager
{
    public short playerPrefabIndex;
    public static int count = 0;
    public GameObject[] allchar;
    public GameObject[] allProjectile;
    public GameObject iceSlowProjectile;
    List<GameObject> myobject;
    NetworkManager myNetworkManager;
 

    // Use this for initialization
    void Start () {

        myobject = new List<GameObject>();
        myNetworkManager = GetComponent<NetworkManager>();
    }
 


    // Update is called once per frame
    void Update () {
	
	}

    public void stopMyHost()
    {
        if (NetworkServer.active && NetworkClient.active)
        {
            myNetworkManager.StopHost();
            count = 0;
            Debug.Log("here");
            //myNetworkManager.StopServer();
            //NetworkServer.Shutdown();

        }
    }
   
    public List<GameObject> getmyobj()
    {
        return myobject;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
        msg.controllerID = playerControllerId;
        NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefab, msg);
    }

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    //Debug.Log("in add player: " + spawnPrefabs[0].name);

    //    var player = (GameObject)GameObject.Instantiate(allchar[characterSelectManager.selectedCharacter], Vector3.zero, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
    public override void OnClientConnect(NetworkConnection conn)
    {
        client.RegisterHandler(MsgTypes.PlayerPrefab, OnRequestPrefab);
        base.OnClientConnect(conn);
    }
    private void OnRequestPrefab(NetworkMessage netMsg)
    {
        MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
        msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
        msg.prefabIndex = playerPrefabIndex;
        client.Send(MsgTypes.PlayerPrefab, msg);
        Debug.Log("on request prefabs");
    }

    private void OnResponsePrefab(NetworkMessage netMsg)
    {
        MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();
        playerPrefab = spawnPrefabs[msg.prefabIndex];
        spawnProjectile(msg.prefabIndex,netMsg.conn);
        base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
        
        Debug.Log(playerPrefab.name + " spawned!");
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        count++;
   //     Debug.Log("connect server: " + count.ToString());
    }
    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(MsgTypes.PlayerPrefab, OnResponsePrefab);
        Debug.Log("here server");
        base.OnStartServer();
    }
    void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("server connected");
    }
    //public void createplayer()
    //{
    //    //this 1
    //    ClientScene.AddPlayer(client.connection, 0);

    //}
    public void startMatchMaking()
    {

      
        NetworkManager.singleton.StartMatchMaker();

    }
    public void CreateInternetMatch(string matchName)
    {
        startMatchMaking();
        //manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
        CreateMatchRequest create = new CreateMatchRequest();
        create.name = matchName;
        create.size = 2;
        create.advertise = true;
        create.password = "";

        //NetworkManager.singleton.matchMaker.CreateMatch(create, OnInternetMatchCreate);
        NetworkManager.singleton.matchMaker.CreateMatch(create, OnInternetMatchCreate);
    }
  //  this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(CreateMatchResponse matchResponse)
    {
        if (matchResponse != null && matchResponse.success)
        {
            Debug.Log("Create match succeeded");

            Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
            MatchInfo hostInfo = new MatchInfo(matchResponse);
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);

            //  NetworkManager.singleton.playerPrefab = allchar[characterSelectManager.selectedCharacter];
        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }

   // call this method to find a match through the matchmaker
    public void FindInternetMatch()
    {
        startMatchMaking();
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", OnInternetMatchList);
    
        //manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
    }

  //  this method is called when a list of matches is returned
    private void OnInternetMatchList(ListMatchResponse matchListResponse)
    {
        if (matchListResponse.success)
        {
            if (matchListResponse.matches.Count != 0)
            {
                Debug.Log("A list of matches was returned");

                //join the last server (just in case there are two...)
                NetworkManager.singleton.matchMaker.JoinMatch(matchListResponse.matches[matchListResponse.matches.Count - 1].networkId, "", OnJoinInternetMatch);
                // NetworkManager.singleton.playerPrefab = allchar[characterSelectManager.selectedCharacter];
            }
            else
            {
                Debug.Log("No matches in requested room!");
                CreateInternetMatch("");//create room when no other room is been found
            }
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
        }
    }
  //  this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(JoinMatchResponse matchJoin)
    {
        if (matchJoin.success)
        {
            Debug.Log("Able to join a match");

            if (Utility.GetAccessTokenForNetwork(matchJoin.networkId) == null)
                Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));

            MatchInfo hostInfo = new MatchInfo(matchJoin);
            NetworkManager.singleton.StartClient(hostInfo);

            // myNBP.CmdSendPositionToServer(true);
            //  customNetworkBluePrint.CmdSendPositionToServer(true);

        }
        else
        {
            Debug.LogError("Join match failed");

        }
    }
    void spawnProjectile(short num,NetworkConnection mync)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate(allProjectile[num], new Vector3(0, 100, 4), Quaternion.identity) as GameObject;


            NetworkServer.Spawn(go);
            //NetworkServer.SpawnWithClientAuthority(go, mync);
            myobject.Add(go);
            Debug.Log("adding obj");
        }
        for (int i = 0; i < 2; i++)
        {
            if (num == 1)//ice slow ball
            {

                GameObject go = Instantiate(iceSlowProjectile, new Vector3(0, 100, 4), Quaternion.identity) as GameObject;
                NetworkServer.Spawn(go);
                myobject.Add(go);
            }
        }
    }
   
}
