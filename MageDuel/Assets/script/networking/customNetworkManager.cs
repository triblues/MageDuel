using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class customNetworkManager : NetworkManager
{
 
    public GameObject[] allchar;
    public static bool isMultiplayer;
    public static bool hasTwoPlayerJoin;
  //  customNetworkBluePrint myNBP;

    // Use this for initialization
    void Start () {

        hasTwoPlayerJoin = false;
        isMultiplayer = false;
        //myNBP = GameObject.Find("networkBP").GetComponent<customNetworkBluePrint>();
       // GameObject a = GameObject.Find("networkBP");
    }
	

	// Update is called once per frame
	void Update () {
	
	}

    //public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
    public void spawnPlayer()
    {
        GameObject obj = GameObject.Instantiate(allchar[0]);
        NetworkServer.Spawn(obj);
    }

    public void startMatchMaking()
    {
       
        NetworkManager.singleton.StartMatchMaker();
      
    }
    public void CreateInternetMatch(string matchName)
    {
        startMatchMaking();
        CreateMatchRequest create = new CreateMatchRequest();
        create.name = matchName;
        create.size = 2;
        create.advertise = true;
        create.password = "";

        NetworkManager.singleton.matchMaker.CreateMatch(create, OnInternetMatchCreate);
    }
    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(CreateMatchResponse matchResponse)
    {
        if (matchResponse != null && matchResponse.success)
        {
            Debug.Log("Create match succeeded");

            Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
            MatchInfo hostInfo = new MatchInfo(matchResponse);
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);

            NetworkManager.singleton.playerPrefab = allchar[0];
            //NetworkManager.singleton.playerPrefab = allchar[characterSelectManager.selectedCharacter];

        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }

    //call this method to find a match through the matchmaker
    public void FindInternetMatch(string matchName)
    {
        startMatchMaking();
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, matchName, OnInternetMatchList);
    }

    //this method is called when a list of matches is returned
    private void OnInternetMatchList(ListMatchResponse matchListResponse)
    {
        if (matchListResponse.success)
        {
            if (matchListResponse.matches.Count != 0)
            {
                Debug.Log("A list of matches was returned");
                NetworkManager.singleton.playerPrefab = allchar[characterSelectManager.selectedCharacter];
                //join the last server (just in case there are two...)
                NetworkManager.singleton.matchMaker.JoinMatch(matchListResponse.matches[matchListResponse.matches.Count - 1].networkId, "", OnJoinInternetMatch);
                NetworkManager.singleton.playerPrefab = allchar[0];
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
    //this method is called when your request to join a match is returned
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
            hasTwoPlayerJoin = true;
            isMultiplayer = true;
        }
        else
        {
            Debug.LogError("Join match failed");

        }
    }

   
}
