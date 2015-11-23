using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class iceballNetwork : weaponBaseNetwork
{

    protected float ownKnowckBack;
    protected float ownDamage;
    protected SphereCollider mySC;
    // Use this for initialization
    void Awake()
    {
        mySC = GetComponent<SphereCollider>();
        ownDamage = damage;
        ownKnowckBack = knockBack;
    }
    protected override void OnEnable()
    {
        StartCoroutine(delayCollider(0.35f));
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        mySC.enabled = false;
        base.OnDisable();
    }
    protected override void Update()
    {
        if (isServer == false)
            return;

        base.Update();
        transform.Translate(movement.normalized * speed * Time.deltaTime);



    }
    protected IEnumerator delayCollider(float _time)
    {
        yield return new WaitForSeconds(_time);
        mySC.enabled = true;
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
                    if (other.GetComponent<CharacterBaseNetwork>().getisDoubleTap() == true)
                    {
                        damage = 0;
                        comboCount = 0;
                        knockBack = 0;
                       

                    }
                    else
                    {
                        damage = ownDamage;
                        comboCount = 1;
                        knockBack = ownKnowckBack;


                    }
                    other.GetComponent<CharacterBaseNetwork>().setStunRate(1);
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







