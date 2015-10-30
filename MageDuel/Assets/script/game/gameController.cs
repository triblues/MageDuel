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
    public GameObject[] allEnvironment;
    public Vector3[] spawnPos;//0 for player, 1 for enemy;

    public Transform myParent;
    public GameObject[] myProjectile;
    public int[] amount;
    public bool isTest = false;
    public bool donSpawn = true;
 
    List<poolObject> myPoolObj;
  

   
    
    // Use this for initialization
    void Awake()
    {
        isFinish = false;

        myPoolObj = new List<poolObject>();
        for (int i = 0; i < myProjectile.Length; i++)
        {
           

            poolObject temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amount[i], true, myProjectile[i], myParent);
            myPoolObj.Add(temp);
        }
        if (donSpawn == false)
        {
            if (isTest == true)
            {
                Instantiate(allCharacter[0], spawnPos[0],
                    Quaternion.identity);//spawn player

                Instantiate(allEnemy[0], spawnPos[1],
                  Quaternion.identity);//spawn enemy

                Instantiate(allEnvironment[0], Vector3.zero,
                 Quaternion.identity);//spawn environment
            }
            else
            {
                Instantiate(allCharacter[characterSelectManager.selectedCharacter], spawnPos[0],
                   Quaternion.identity);//spawn player

                Instantiate(allEnemy[levelSelectController.selectedLevel - 1], spawnPos[1],
                  Quaternion.identity);//spawn enemy

                Instantiate(allEnvironment[levelSelectController.selectedLevel - 1], Vector3.zero,
                   Quaternion.identity);//spawn environment
            }
        }

        

        //GameObject myplayer = Instantiate(allCharacter[characterSelectManager.selectedCharacter], spawnPos[0],
        //    Quaternion.identity) as GameObject;//spawn player

        //GameObject myenemy = Instantiate(allEnemy[levelSelectController.selectedLevel - 1], spawnPos[1],
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
