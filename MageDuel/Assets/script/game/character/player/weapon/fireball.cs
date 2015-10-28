using UnityEngine;
using System.Collections;

public class fireball : weaponBase {

    protected float ownKnowckBack;
    protected float ownDamage;
    // Use this for initialization
    void Awake () {

        ownDamage = damage;
        ownKnowckBack = knockBack;
    }

	void OnEnable()
	{
        totalTime = deSpawn_Time;
        //totalTime = Time.time + deSpawn_Time;

	}
	// Update is called once per frame
	void Update () {

        if (CharacterBase.isCastModeAnimation == false)
        {
            transform.Translate(movement.normalized * speed * Time.deltaTime);

            if (deSpawn_Time == 0)//unlimited
                return;

            totalTime -= Time.deltaTime;

            if (totalTime <= 0)
            {
                gameObject.SetActive(false);
            }
        }
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
                    if(other.GetComponent<CharacterBase>().getisDoubleTap() == true)
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
                    other.GetComponent<CharacterBase>().setStunRate(1);
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






