using UnityEngine;
using System.Collections;

public class iceballSlowNetwork : weaponBaseNetwork
{

    protected float ownKnowckBack;
    protected float ownDamage;
    // Use this for initialization
    void Awake()
    {

        ownDamage = damage;
        ownKnowckBack = knockBack;
    }


  
    // Update is called once per frame
    protected override void Update()
    {
        if (isServer == false)
            return;

        base.Update();
        transform.Translate(movement.normalized * speed * Time.deltaTime);


    }
    override protected void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<weaponBaseNetwork>() != null)//has this script
        {
            if (other.GetComponent<weaponBaseNetwork>().getTag() != numTag)//prevent own attack from cancel own attack
            {

                gameObject.SetActive(false);//player and enemy projectile cancel out
                return;
            }



        }
        if (other.GetComponent<CharacterBaseNetwork>() != null)//has this script
        {
            if (other.GetComponent<CharacterBaseNetwork>().getCharacterTag() != numTag)//prevent hit ownself
            {
                if (other.GetComponent<CharacterBaseNetwork>().getIsBlocking() == false)//player get hit
                {

                    //slow movement
                    //other.GetComponent<CharacterBaseNetwork>().setSpeed(0.5f);
                    Debug.Log("slow down");
                    other.GetComponent<CharacterBaseNetwork>().setIsFreeze(true);
                  //  other.GetComponent<CharacterBaseNetwork>().getEnemy().GetComponent<CharacterBaseNetwork>().showHitEffect();
                }
                else
                {

                    other.GetComponent<CharacterBaseNetwork>().setBlockAnimation();
                }

                gameObject.SetActive(false);
            }
        }

        base.OnTriggerEnter(other);

    }
}
