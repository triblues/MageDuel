using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ultimateCameraControllerNetwork : NetworkBehaviour
{

    public float shakeTime = 2.0f;
    public float shakeAmount = 0.1f;
    //Camera mymainCam;
    Camera mycam;
    bool isFaceRight;
  
    Transform characterTrans;
    Transform enemyTrans;

    [SyncVar]//any changes made to this variable, the server will send it to all other client
    private Vector3 syncPos;
    [SyncVar(hook ="setIsShake")]
    bool isShake;
    //[SyncVar(hook = "setIsStart")]
    //bool isStart;
    // Use this for initialization
    void Awake()
    {
     
       // mymainCam = Camera.main;
        mycam = GetComponent<Camera>();


    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (isServer == false)
        {
            transform.position = syncPos;
        }
    }
    [Command]
    void CmdSendPositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }
    [Command]
    void CmdSendShakeToServer(bool _shake)
    {
        isShake = _shake;
    }
    [ServerCallback]
    void transmitTransform()
    {
        if (isServer == true)
        {
            CmdSendPositionToServer(transform.position);
        }
    }
    [ServerCallback]
    void transmitShake(bool _isShake)
    {
        if(isServer == true)
        {
            CmdSendShakeToServer(_isShake);
        }
    }


    public void removeUltimate()
    {


        //mymainCam.enabled = true;
        mycam.enabled = false;
    }
    public void setDetail(Transform _charTrans, Transform _enemyTrans, bool isRight, float waitTime)
    {
        //if (isServer == false)
          //  return;

        characterTrans = _charTrans;
        enemyTrans = _enemyTrans;
        isFaceRight = isRight;

        if (isFaceRight)
            transform.position = new Vector3(characterTrans.position.x + 3, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(characterTrans.position.x - 3, transform.position.y, transform.position.z);

       

        transform.LookAt(characterTrans);//look at player
        mycam.enabled = true;


         transmitTransform();

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
        transmitShake(false);
    }

    IEnumerator delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isFaceRight)//look at enemy
            transform.position = new Vector3(enemyTrans.position.x - 10, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(enemyTrans.position.x + 10, transform.position.y, transform.position.z);
        transform.LookAt(enemyTrans);

        transmitTransform();
        transmitShake(true);
        shake();
    }
    [ClientCallback]
    void setIsShake(bool _isShake)
    {
        if(isShake == true)
        {
            shake();
        }
    }



}
