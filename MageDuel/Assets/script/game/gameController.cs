using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class gameController : MonoBehaviour
{

    public enum projectileType
    {
        fireball,
        iceball
    };
    public static bool isFinish;
    public GameObject[] allCharacter;
    public GameObject[] allEnemy;
    public Vector3[] spawnPos;//0 for player, 1 for enemy;

    public Transform myParent;
    public GameObject[] myProjectile;
    public int[] amount;
 
    List<poolObject> myPoolObj;
  

   
    
    // Use this for initialization
    void Start()
    {
        isFinish = false;

        myPoolObj = new List<poolObject>();
        for (int i = 0; i < myProjectile.Length; i++)
        {
           

            poolObject temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amount[i], true, myProjectile[i], myParent);
            myPoolObj.Add(temp);
        }

        
        //GameObject myplayer = Instantiate(allCharacter[characterSelectManager.selectedCharacter], spawnPos[0], 
        //    Quaternion.identity) as GameObject;//spawn player

        //GameObject myenemy = Instantiate(allEnemy[launchScene.selectedLevel - 1], spawnPos[1], 
        //    Quaternion.identity) as GameObject;//spawn enemy

        //myplayer.GetComponent<CharacterBase>().enabled = true;
        //myenemy.GetComponent<CharacterBase>().enabled = true;
        //Camera.main.GetComponent<CameraController>().enabled = true;


    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public poolObject getPoolObjectInstance(projectileType myType)
    {
        return myPoolObj[(int)myType];
        //		if (name == "fireball")
        //			return myPoolObj[0];
        //		else
        //			return null;
    }

   
}
