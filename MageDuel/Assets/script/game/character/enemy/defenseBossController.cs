﻿using UnityEngine;
using System.Collections;

public class defenseBossController : EnemyAI
{

    protected override void Awake()
    {

        base.Awake();
    }
    // Use this for initialization
    protected override void Start()
    {
       // 
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        setAnimation();
        base.Update();
    }


}
