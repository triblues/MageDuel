using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class gameController : MonoBehaviour {

	public GameObject myfireball;
	poolObject myPoolObjFireBall;
	//List<GameObject> myfireball_list;
	// Use this for initialization
	void Start () {
	
		//myfireball_list = new List<GameObject> ();
		myPoolObjFireBall = new poolObject (5, false, myfireball);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public poolObject getPoolObjectInstance(string name)
	{
		if (name == "fireball")
			return myPoolObjFireBall;
		else
			return null;
	}
}
