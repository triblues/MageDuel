using UnityEngine;
using System.Collections;

public class drawGizmo : MonoBehaviour {

    public float radius;
    public Color mycolor = Color.red;
	// Use this for initialization
	void Start () {

      
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnDrawGizmos()
    {
        Gizmos.color = mycolor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
