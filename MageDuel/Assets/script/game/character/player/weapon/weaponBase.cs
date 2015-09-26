using UnityEngine;
using System.Collections;

public class weaponBase : MonoBehaviour {

	[SerializeField] protected float damage;
	[SerializeField] protected float speed;
	[SerializeField] protected float deSpawn_Time;
	[SerializeField] protected float consumeMana;
	protected Vector3 movement;
	protected int numTag;//store the number that the character fire this attack
	protected float totalTime;
	// Use this for initialization
	void Start () {
	
	}

	public float getDeSpawnTime()
	{
		return deSpawn_Time;
	}
	public float getDamage()
	{
		return damage;
	}

	public float getSpeed()
	{
		return speed;
	}
	public float getConsumeMana()
	{
		return consumeMana;
	}
}
