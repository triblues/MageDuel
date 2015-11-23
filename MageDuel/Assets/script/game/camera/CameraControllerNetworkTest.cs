using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControllerNetworkTest : MonoBehaviour
{
  
  
    public float zAmout;
   
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
  
    bool canMove;


   
    List<Vector3> allPos = new List<Vector3>();//all the position of the transform
    // Use this for initialization

    void Start()
    {
        canMove = false;
       
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
      


        CalcScreen(p1, p2);
        float width = xR - xL;


        if (width > wScene)
        { // if fighters too far adjust camera distance
            // transform.position = new Vector3(transform.position.x, transform.position.y, zCam * width / wScene + z0);
            depthMovement = new Vector3(transform.position.x, transform.position.y, (zCam * width / wScene + z0) * zAmout);
            transform.position = Vector3.Lerp(transform.position, depthMovement, Time.deltaTime * lerpRate);
        }

        // centers the camera
        //transform.position = new Vector3((xR + xL) / 2, transform.position.y, transform.position.z);
        rightMovement = new Vector3((xR + xL) / 2, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, rightMovement, Time.deltaTime * lerpRate);
       
        
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
  
  
    
}
