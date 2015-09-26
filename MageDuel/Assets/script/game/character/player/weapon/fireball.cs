using UnityEngine;
using System.Collections;

public class fireball : weaponBase {

	Rigidbody rb;
	// Use this for initialization
	void Awake () {
	
		//rb = GetComponent<Rigidbody> ();
	}

	void OnEnable()
	{
	//	rb.velocity = Vector3.zero;
		totalTime = Time.time + deSpawn_Time;

	}
	// Update is called once per frame
	void Update () {
	
		if (deSpawn_Time == 0)//unlimited
			return;

		transform.Translate (movement.normalized * speed * Time.deltaTime);

		if(Time.time > totalTime)
		{
			gameObject.SetActive(false);
		}
	}
	public void setTag(int num)
	{
		numTag = num;
	}

	public void launch(Vector3 direction)
	{
		movement = direction;
		//transform.Translate (direction.normalized * speed * Time.deltaTime);
		//rb.AddForce (direction.normalized * speed, ForceMode.Impulse);
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Rigidbody>() != null)//has a body
		{
			if(other.GetComponent<CharacterBase>().getCharacterTag() != numTag)//prevent own attack to hit ownself
			{
				other.GetComponent<CharacterBase>().TakesDamage(damage);
				other.GetComponent<CharacterBase>().checkDead();
				gameObject.SetActive(false);
			}
		}

	}
}






