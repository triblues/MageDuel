using UnityEngine;
using System.Collections;

public class fireball : weaponBase {


	// Use this for initialization
	void Awake () {
	

	}

	void OnEnable()
	{
	
		totalTime = Time.time + deSpawn_Time;

	}
	// Update is called once per frame
	void Update () {
	
		transform.Translate (movement.normalized * speed * Time.deltaTime);

		if (deSpawn_Time == 0)//unlimited
			return;
		if(Time.time > totalTime)
		{
			gameObject.SetActive(false);
		}
	}


	override protected void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter (other);
        if (other.GetComponent<weaponBase>() != null)//has this script
        {
            if (other.GetComponent<weaponBase>().getTag() != numTag)//prevent own attack from cancel own attack
                gameObject.SetActive(false);

            return;
        }
        gameObject.SetActive(false);
        //		if(other.GetComponent<CharacterBase>() != null)//has this script
        //		{
        //			if(other.GetComponent<CharacterBase>().getCharacterTag() != numTag)//prevent own attack to hit ownself
        //			{
        //				other.GetComponent<CharacterBase>().TakesDamage(damage);
        //				other.GetComponent<Rigidbody>().AddForce(-other.GetComponent<Transform>().forward * knockBack,
        //				                                         ForceMode.VelocityChange);
        //				other.GetComponent<CharacterBase>().checkDead();
        //
        //
        //			}
        //		}
        //		if(other.GetComponent<weaponBase>() != null)//has this script
        //		{
        //			if(other.GetComponent<weaponBase>().getTag() != numTag)
        //				gameObject.SetActive(false);
        //			else
        //				return;
        //		}
        //		gameObject.SetActive(false);
    }
}






