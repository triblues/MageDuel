﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class objectActive : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    [ServerCallback]
    void transmitActive()
    {

    }
    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}
