using UnityEngine;
using System.Collections;

public class iceball : weaponBase {

	// Use this for initialization
	void Start () {
	
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
//		if(other.GetComponent<CharacterBase>() != null)//has a body
//		{
//			if(other.GetComponent<CharacterBase>().getCharacterTag() != numTag)//prevent own attack to hit ownself
//			{
//				other.GetComponent<CharacterBase>().TakesDamage(damage);
//				other.GetComponent<Rigidbody>().AddForce(-other.GetComponent<Transform>().forward * knockBack,
//				                                         ForceMode.VelocityChange);
//				other.GetComponent<CharacterBase>().checkDead();
//
//				gameObject.SetActive(false);
//			}
//		}
		
	}
}
