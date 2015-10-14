using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class gameController : MonoBehaviour
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

    // Use this for initialization
    void Start()
    {

        myPoolObj = new List<poolObject>();
        for (int i = 0; i < myProjectile.Length; i++)
        {
            //myPoolObj.Add(new poolObject(amount[i], true, myProjectile[i],myParent));

            poolObject temp = gameObject.AddComponent<poolObject>();
            temp.setPoolObject(amount[i], true, myProjectile[i], myParent);
            myPoolObj.Add(temp);
        }
        //Debug.Log("character num: " + characterSelectManager.selectedCharacter.ToString());
        Instantiate(allCharacter[characterSelectManager.selectedCharacter], spawnPos[0], Quaternion.identity);//spawn player
        Instantiate(allEnemy[launchScene.selectedLevel-1], spawnPos[1], Quaternion.identity);//spawn enemy
        Camera.main.GetComponent<CameraController>().enabled = true;
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

    // Executes the appropriate command if the escape key is pressed
    public void KeyboardListener ()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.LoadLevel("pausePractice");
        }
    }
}
