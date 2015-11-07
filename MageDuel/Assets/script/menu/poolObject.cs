using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class poolObject : MonoBehaviour
{

    GameObject myobject;
    int poolAmount;
    bool willGrow;

    List<GameObject> poolObject_list;
    //List<GameObject>[] poolObject_lists;
    // Use this for initialization

    void Start()
    {
    }
    public void setPoolObject(int amount, bool isGrow, GameObject obj, Transform myparent)
    {
        myobject = obj;
        poolAmount = amount;
        willGrow = isGrow;

        poolObject_list = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject temp = (GameObject)Instantiate(myobject);
            temp.transform.SetParent(myparent);
            temp.SetActive(false);
            poolObject_list.Add(temp);

        }
    }
    public poolObject(int amount, bool isGrow, GameObject obj, Transform myparent)
    {
        myobject = obj;
        poolAmount = amount;
        willGrow = isGrow;
        //poolObject_list = _poolObject_list;
        poolObject_list = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject temp = (GameObject)Instantiate(myobject);
            temp.transform.SetParent(myparent);
            temp.SetActive(false);
            poolObject_list.Add(temp);

        }
    }


    public GameObject getPoolObject()
    {
        for (int i = 0; i < poolObject_list.Count; i++)
        {
            if (poolObject_list[i].activeInHierarchy == false)
            {
                return poolObject_list[i];
            }
        }

        if (willGrow == true)
        {
            GameObject temp = (GameObject)Instantiate(myobject);
            poolObject_list.Add(temp);
            return temp;
        }
        return null;
    }
}
