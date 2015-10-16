using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class gameController : NetworkBehaviour
{

    public enum projectileType
    {
        fireball,
        iceball
    };
    public GameObject[] allCharacter;
    public GameObject[] allEnemy;
    public Vector3[] spawnPos;//0 for player, 1 for enemy;

    public Transform myParent;
    public GameObject[] myProjectile;
    public int[] amount;
 
    List<poolObject> myPoolObj;
    customNetworkManager myCNM;

    [SyncVar]
    bool twoplayerJoin;
    
    // Use this for initialization
    void Start()
    {
        
        myPoolObj = new List<poolObject>();
        for (int i = 0; i < myProjectile.Length; i++)
        {
           

            poolObject temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amount[i], true, myProjectile[i], myParent);
            myPoolObj.Add(temp);
        }

        if (customNetworkManager.isMultiplayer == false)
        {
          GameObject myplayer = Instantiate(allCharacter[characterSelectManager.selectedCharacter], spawnPos[0], 
              Quaternion.identity) as GameObject;//spawn player

            GameObject myenemy = Instantiate(allEnemy[launchScene.selectedLevel - 1], spawnPos[1], 
                Quaternion.identity) as GameObject;//spawn enemy

            myplayer.GetComponent<CharacterBase>().enabled = true;
            myenemy.GetComponent<CharacterBase>().enabled = true;
            Camera.main.GetComponent<CameraController>().enabled = true;
        }
        else
        {
            Debug.Log("in mul");
            //myCNM = GameObject.Find("networkController").GetComponent<customNetworkManager>();
            //myCNM.spawnPlayer();

            if (customNetworkManager.hasTwoPlayerJoin == true)
            {
                CmdSendPositionToServer(true);
               // Debug.Log("is true");
            }
            //else
              //  Debug.Log("is false");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (twoplayerJoin == true)
            Debug.Log("true");
        else
            Debug.Log("false");
    }

    public poolObject getPoolObjectInstance(projectileType myType)
    {
        return myPoolObj[(int)myType];
        //		if (name == "fireball")
        //			return myPoolObj[0];
        //		else
        //			return null;
    }

    [Command]//a command to send to the server
    public void CmdSendPositionToServer(bool check)//must have Cmd as the start of the name, this function only run in the server
    {
        twoplayerJoin = check;
    }
}
