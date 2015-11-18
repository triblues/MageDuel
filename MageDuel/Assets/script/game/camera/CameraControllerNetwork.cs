using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CameraControllerNetwork : NetworkBehaviour
{
    [SerializeField]
    float distanceOffset = 0.2f;
    [SerializeField]
    float maxDist = 0.2f;
    public float networklerpRate = 25.0f;
    public float lerpRate = 15.0f;
    public float margin = 1.5f;
    private float z0 = 0; // coord z of the fighters plane
    private float zCam; // camera distance to the fighters plane
    private float wScene; // scene width
    private Transform p1; // fighter1 transform
    private Transform p2; // fighter2 transform
    private float xL; // left screen X coordinate
    private float xR; // right screen X coordinate
    Vector3 depthMovement;
    Vector3 rightMovement;
    Vector3 lastPos;
    bool canMove;


    [SyncVar(hook = "syncPositionValue")]//any changes made to this variable, the server will send it to all other client
    private Vector3 syncPos;
    List<Vector3> allPos = new List<Vector3>();//all the position of the transform
    // Use this for initialization

    void Start()
    {
        canMove = false;
        lastPos = transform.position;
        StartCoroutine(delay(4.0f));


    }
    IEnumerator delay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Main Player");
        for (int i = 0; i < temp.Length; i++)//length should be 2
        {
            if (i == 0)
                p1 = temp[i].transform;
            else
                p2 = temp[i].transform;
        }


        CalcScreen(p1, p2);
        wScene = xR - xL;
        zCam = transform.position.z - z0;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == false)
            return;
        if (isServer == false)
        {
            lerpPosition();//for the other client
        }
        else
        {


            CalcScreen(p1, p2);
            float width = xR - xL;


            if (width > wScene)
            { // if fighters too far adjust camera distance
              // transform.position = new Vector3(transform.position.x, transform.position.y, zCam * width / wScene + z0);
                depthMovement = new Vector3(transform.position.x, transform.position.y, zCam * width / wScene + z0);
                transform.position = Vector3.Lerp(transform.position, depthMovement, Time.deltaTime * lerpRate);
            }

            // centers the camera
            //transform.position = new Vector3((xR + xL) / 2, transform.position.y, transform.position.z);
            rightMovement = new Vector3((xR + xL) / 2, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, rightMovement, Time.deltaTime * lerpRate);
            transmitTransform();
        }
    }

    void CalcScreen(Transform p1, Transform p2)
    {
        // Calculates the xL and xR screen coordinates 
        if (p1.position.x < p2.position.x)
        {
            xL = p1.position.x - margin;
            xR = p2.position.x + margin;
        }
        else
        {
            xL = p2.position.x - margin;
            xR = p1.position.x + margin;
        }
    }
    [ServerCallback]
    void transmitTransform()
    {
        if(isServer == true)
        {
            if (Vector3.Distance(transform.position, lastPos) > distanceOffset)
            {
                CmdSendPositionToServer(transform.position);
                lastPos = transform.position;
            }
        }
    }
    [Command]
    void CmdSendPositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }
    [ClientCallback]
    void syncPositionValue(Vector3 mypos)
    {
        syncPos = mypos;
        allPos.Add(syncPos);
    }
    void lerpPosition()
    {
        if (allPos.Count > 0)
        {
            
            transform.position = Vector3.Lerp(transform.position, allPos[0], Time.deltaTime * networklerpRate);

            if (Vector3.Distance(transform.position, allPos[0]) < maxDist)
            {
                allPos.RemoveAt(0);
            }


        }
    }

}
