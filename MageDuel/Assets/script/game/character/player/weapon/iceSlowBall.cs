using UnityEngine;
using System.Collections;

public class iceSlowBall : weaponBase {

    protected float ownKnowckBack;
    protected float ownDamage;
    // Use this for initialization
    void Awake()
    {

        ownDamage = damage;
        ownKnowckBack = knockBack;
    }


    void OnEnable()
    {

        totalTime = deSpawn_Time;

    }
    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        transform.Translate(movement.normalized * speed * Time.deltaTime);


    }
    override protected void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<weaponBase>() != null)//has this script
        {
            if (other.GetComponent<weaponBase>().getTag() != numTag)//prevent own attack from cancel own attack
            {

                gameObject.SetActive(false);//player and enemy projectile cancel out
                return;
            }



        }
        if (other.GetComponent<CharacterBase>() != null)//has this script
        {
            if (other.GetComponent<CharacterBase>().getCharacterTag() != numTag)//prevent hit ownself
            {
                if (other.GetComponent<CharacterBase>().getIsBlocking() == false)//player get hit
                {

                    //slow movement
                    other.GetComponent<CharacterBase>().setSpeed(0.5f);
                    other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().showHitEffect();
                }
                else
                {

                    other.GetComponent<CharacterBase>().setBlockAnimation();
                }

                gameObject.SetActive(false);
            }
        }

        base.OnTriggerEnter(other);

    }
}
