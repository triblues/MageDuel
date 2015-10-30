using UnityEngine;
using System.Collections;

public class ultimateCameraController : MonoBehaviour {

    public float shakeTime = 2.0f;
    public float shakeAmount = 0.1f;
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
        if (isFaceRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        //transform.LookAt(characterTrans);
        mycam.enabled = true;
    }
    void OnDisable()
    {
        mymainCam.enabled = true;
        mycam.enabled = false;
        Debug.Log("disable ucam");
    }
    public void setCharacterDetail(Transform charTrans,Vector3 mypos,bool isRight)
    {
        characterTrans = charTrans;
        transform.position = mypos;
      
        isFaceRight = isRight;
    
    }
    void shake()
    {
        StartCoroutine(countDown(shakeTime));

    }
    IEnumerator countDown(float waitTime)
    {
        while (waitTime > 0)
        {
            transform.position += Random.insideUnitSphere * shakeAmount;
            waitTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
