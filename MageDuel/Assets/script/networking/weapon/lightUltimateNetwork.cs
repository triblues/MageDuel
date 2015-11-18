using UnityEngine;
using System.Collections;

public class lightUltimateNetwork : weaponBaseNetwork
{


    SphereCollider mySC;
    // Use this for initialization
    void Awake()
    {

        mySC = GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


    }

    protected override void OnEnable()
    {
        base.OnEnable();

      
        StartCoroutine(delay(2.0f));

    }
    protected override void OnDisable()
    {
        mySC.enabled = false;
        base.OnDisable();
    }

    IEnumerator delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mySC.enabled = true;
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
