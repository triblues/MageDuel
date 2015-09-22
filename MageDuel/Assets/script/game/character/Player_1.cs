using UnityEngine;
using System.Collections;

public class Player_1 : PlayerBase {
    private float speed = 10.0f;

    protected override void SetupPlayer()
    {
        pController.SetSpeed(speed);
    }
}
