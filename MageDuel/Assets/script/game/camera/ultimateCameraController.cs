using UnityEngine;
using System.Collections;

public class ultimateCameraController : MonoBehaviour {

    public float shakeTime = 2.0f;
    public float shakeAmount = 0.1f;
    Camera mymainCam;
    Camera mycam;
    bool isFaceRight;
    Transform characterTrans;
    Transform enemyTrans;
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
        //if (isFaceRight == true)
        //{
        //    transform.rotation = Quaternion.Euler(0, 270, 0);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, 90, 0);
        //}
        //if(isFaceRight)
        //    transform.position = new Vector3(characterTrans.position.x + 3, transform.position.y, transform.position.z);
        //else
        //    transform.position = new Vector3(characterTrans.position.x - 3, transform.position.y, transform.position.z);

        //transform.LookAt(characterTrans);
        // mycam.enabled = true;
    }
    void OnDisable()
    {
        if(mymainCam != null)
            mymainCam.enabled = true;
        mycam.enabled = false;
       
    }
    public void removeUltimate()
    {
        mymainCam.enabled = true;
        mycam.enabled = false;
    }
    public void setDetail(Transform _charTrans, Transform _enemyTrans,bool isRight,float waitTime)
    {
        characterTrans = _charTrans;
        enemyTrans = _enemyTrans;
        isFaceRight = isRight;

        if (isFaceRight)
            transform.position = new Vector3(characterTrans.position.x + 3, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(characterTrans.position.x - 3, transform.position.y, transform.position.z);

        transform.LookAt(characterTrans);
        mycam.enabled = true;

        StartCoroutine(delay(waitTime));

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

    IEnumerator delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isFaceRight)
            transform.position = new Vector3(enemyTrans.position.x - 10, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(enemyTrans.position.x + 10, transform.position.y, transform.position.z);
        transform.LookAt(enemyTrans);
        shake();
    }
}
