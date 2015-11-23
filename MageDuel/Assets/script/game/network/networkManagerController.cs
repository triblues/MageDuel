using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class networkManagerController : NetworkManager
{

    //for lan
    public void startUpHost()
    {
        setPost();
        NetworkManager.singleton.StartHost();

    }
    //for lan
    public void joinGame()
    {
        setIpAdress();
        setPost();
        NetworkManager.singleton.StartClient();
    }
    void setPost()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
    void setIpAdress()
    {
        string ip = GameObject.Find("input").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ip;
    }

    //void OnLevelWasLoaded(int level)
    //{
    //    Debug.Log("level: " + level.ToString());
    //    if (level == 0)
    //        setupmenu();
    //}

    //void setupmenu()
    //{
    //    Debug.Log("in");
    //    GameObject.Find("host").GetComponent<Button>().onClick.RemoveAllListeners();
    //    GameObject.Find("host").GetComponent<Button>().onClick.AddListener(startUpHost);

    //    GameObject.Find("join").GetComponent<Button>().onClick.RemoveAllListeners();
    //    GameObject.Find("join").GetComponent<Button>().onClick.AddListener(joinGame);
    //}

    public void startMatchMaking()
    {
       // setPost();
        NetworkManager.singleton.StartMatchMaker();
       
    }
    //create match
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
            //Debug.Log("Create match succeeded");

            Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
            MatchInfo hostInfo = new MatchInfo(matchResponse);
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }
    //create match

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
                //Debug.Log("A list of matches was returned");

                //join the last server (just in case there are two...)
                NetworkManager.singleton.matchMaker.JoinMatch(matchListResponse.matches[matchListResponse.matches.Count - 1].networkId, "", OnJoinInternetMatch);
            }
            else
            {
                Debug.Log("No matches in requested room!");
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
            Debug.Log("client connect");
        }
        else
        {
            Debug.LogError("Join match failed");

        }
    }
}











