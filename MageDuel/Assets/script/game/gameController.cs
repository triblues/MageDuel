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
    public Transform myParent;
    public GameObject[] myProjectile;
    public int[] amount;
    //poolObject[] myPoolObj;
    List<poolObject> myPoolObj;
    //	public GameObject myProjectile2;
    //
    //	poolObject myPoolObj2;

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
