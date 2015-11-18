using UnityEngine;
using System.Collections;

public class iceUltimateNetwork : weaponBaseNetwork
{

    CapsuleCollider myCapCollider;
    // Use this for initialization
    protected override void Start()
    {

        myCapCollider = GetComponent<CapsuleCollider>();
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        mypos = transform.position;

    }
    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        base.Update();


        //if (isLocalPlayer)
        //{
        //   // transform.position = new Vector3(mypos.x, transform.position.y, mypos.z);
        //    //transform.Translate(Vector3.down * speed * Time.deltaTime);
        //}

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

                    Debug.Log("hit ice ulti");
                }

            }
        }
        gameObject.SetActive(false);
    }
}
