// Mage Duel Main Menu GUI source code
// Created : 9 September 2015
using UnityEngine;
using System.Collections;

public class launchScene : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void gotoScene(string name)
	{
		Application.LoadLevel (name);
	}
	public void quit()
	{
		Application.Quit ();
	}
}
