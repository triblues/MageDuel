using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class fireUltimateNetwork : weaponBaseNetwork
{

    bool canIncrease;
    BoxCollider myBC;
    
    // Use this for initialization
    void Awake()
    {
       
        myBC = GetComponent<BoxCollider>();

    }
    protected override void Start()
    {
        Debug.Log("go ulti false");
        //if (isLocalPlayer == true)
        //{
        //    myID = GetComponent<NetworkIdentity>();
        //    myID.AssignClientAuthority(connectionToClient);
        //    Debug.Log("assigning");
        //}
        base.Start();
    }
   
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.position = mypos;
        if (canIncrease == true)
        {

            myBC.center = new Vector3(myBC.center.x, myBC.center.y + 0.5f * Time.deltaTime, myBC.center.z);
            myBC.size = new Vector3(myBC.size.x, myBC.size.y + 1.0f * Time.deltaTime, myBC.size.z);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        myBC.center = new Vector3(0, -0.2f, 0);
        myBC.size = new Vector3(1, 0, 1);
        canIncrease = false;

        mypos = transform.position;

        StartCoroutine(delay(1.0f));

    }
   

    IEnumerator delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canIncrease = true;
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterBaseNetwork>() != null)//has this script
        {
            if (other.GetComponent<CharacterBaseNetwork>().getCharacterTag() != numTag)//prevent hit ownself
            {
                if (other.GetComponent<CharacterBaseNetwork>().getIsBlocking() == false)
                {
                    other.GetComponent<CharacterBaseNetwork>().getEnemy().GetComponent<CharacterBaseNetwork>().ultimateMove();
                  
                    gameObject.SetActive(false);
                  
                }

            }
        }
    }
}
