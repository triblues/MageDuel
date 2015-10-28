using UnityEngine;
using System.Collections;

public class ultimateCameraController : MonoBehaviour {

    Camera mymainCam;
    Camera mycam;
    bool isFaceRight;
    Transform characterTrans;
	// Use this for initialization
	void Awake () {

        mymainCam = Camera.main;
        mycam = GetComponent<Camera>();
      
        
    }
	void Start()
    {

    }
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        //if(characterTag == 1)//player
        //{

        //  //  Debug.Log("1 characterTag: " + characterTag.ToString());
        //}
        //else//enemy
        //{
        //    Debug.Log("2 characterTag: " + characterTag.ToString());
        //}
        if(isFaceRight == true)
        {
            transform.position = new Vector3(characterTrans.position.x + 3, 3.0f, 2);
        }
        else
        {
            transform.position = new Vector3(characterTrans.position.x - 3, 3.0f, 2);
        }
        transform.LookAt(characterTrans);
        mycam.enabled = true;
    }
    void OnDisable()
    {
        mymainCam.enabled = true;
        mycam.enabled = false;
        Debug.Log("disable ucam");
    }
    public void setCharacterDetail(Transform mytrans,bool isRight)
    {
        characterTrans = mytrans;
        isFaceRight = isRight;
      //  characterTag = num;
    }
}
