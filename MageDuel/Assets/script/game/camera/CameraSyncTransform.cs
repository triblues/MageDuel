using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CameraSyncTransform : NetworkBehaviour
{
    //[SyncVar(hook = "syncPositionValue")]//any changes made to this variable, the server will send it to all other client
    [SyncVar]
    private Vector3 syncPos;
    [SerializeField]
    float lerpRate = 15.0f;
    [SerializeField]
    float distanceOffset = 0.5f;

    Vector3 lastPos;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        LerpTransform();//for non local player
    }
    void FixedUpdate()
    {

        trasmitTransform();

    }

    void LerpTransform()
    {
        if (isServer == false)
        {
            normalLerp();
            //if (isHistoricalLerp == false)
            //{
            //    normalLerp();

            //}
            //else
            //{
            //    syncPos = Vector3.zero;
            //    historicalLerp();

            //}
        }
    }
    void normalLerp()
    {
        transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * lerpRate);
      
    }

    [ServerCallback]//command can only be send from local player
    void trasmitTransform()
    {
        if (isServer == true)
        {
            if (Vector3.Distance(transform.position, lastPos) > distanceOffset)
            {
                CmdSendPositionToServer(transform.position);
                lastPos = transform.position;
            }

            
        }
    }
    [Command]//a command to send to the server
    void CmdSendPositionToServer(Vector3 pos)//must have Cmd as the start of the name, this function only run in the server
    {
        syncPos = pos;
    }
}
