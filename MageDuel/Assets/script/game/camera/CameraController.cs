using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float zAmout;
    public float distanceApart;
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
    
	// Use this for initialization
	void Start () {
       
        // find references to the players
        p1 = GameObject.FindWithTag("Main Player").transform;
		p2 = GameObject.FindWithTag("Enemy").transform;
        // initializes scene size and camera distance
        CalcScreen(p1, p2);
        wScene = xR - xL;
        zCam = transform.position.z - z0;
      

    }
	
	// Update is called once per frame
	void Update () {
        CalcScreen(p1, p2);
        float width = xR - xL;

        //if(Vector3.Distance(p1.transform,p2.transform) >= distanceApart)
        //{
        //    Camera.main.fieldOfView = 
        //}

        if (width > wScene)
        { // if fighters too far adjust camera distance
           // transform.position = new Vector3(transform.position.x, transform.position.y, zCam * width / wScene + z0);
			depthMovement = new Vector3(transform.position.x, transform.position.y, (zCam * width / wScene + z0) * zAmout);
			transform.position = Vector3.Lerp(transform.position,depthMovement,Time.deltaTime * lerpRate);
        }

	        // centers the camera
	        //transform.position = new Vector3((xR + xL) / 2, transform.position.y, transform.position.z);
		rightMovement = new Vector3((xR + xL) / 2, transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position,rightMovement,Time.deltaTime * lerpRate);
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
