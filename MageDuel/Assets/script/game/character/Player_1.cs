using UnityEngine;
using System.Collections;

public class Player_1 : PlayerBase {

    protected override void SetupPlayer()
    {
        speed = 10.0f;
        offFactor = 1.25f;
        defFactor = 0.75f;
    }
}
