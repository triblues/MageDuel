using UnityEngine;
using System.Collections;

public class weaponBase : MonoBehaviour {

	[SerializeField] protected float damage;
	[SerializeField] protected float speed;
	[SerializeField] protected float deSpawn_Time;
	[SerializeField] protected float consumeMana;
	[SerializeField] protected float chargeAmount;//the amount increase to charging bar
	[SerializeField] protected float knockBack;
	protected Vector3 movement;
	protected int numTag;//store the number that the character fire this attack
    protected int comboCount;
	protected float totalTime;
	// Use this for initialization
	void Start () {

        comboCount = 1;//default

    }

	public float getDeSpawnTime()
	{
		return deSpawn_Time;
	}
	public float getDamage()
	{
		return damage;
	}
	public int getTag()
	{
		return numTag;
	}
	public float getSpeed()
	{
		return speed;
	}
	public float getConsumeMana()
	{
		return consumeMana;
	}
	public void setTag(int num)
	{
		numTag = num;
	}
    public void setknockBack(float amount)
    {
        knockBack = amount;
    }

    public void launch(Vector3 direction)
	{
		movement = direction;

	}
	protected virtual void OnTriggerEnter(Collider other)
	{
        if (other.GetComponent<CharacterBase>() != null)//has this script
        {
            if (other.GetComponent<CharacterBase>().getCharacterTag() != numTag)//prevent own attack to hit ownself
            {
                if (other.GetComponent<CharacterBase>().getIsBlocking() == false)
                {
                   other.GetComponent<CharacterBase>().TakesDamage(damage);
                    other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().setComboCount(comboCount);

                    other.GetComponent<Rigidbody>().AddForce(-other.GetComponent<Transform>().forward * knockBack,
                                                             ForceMode.Impulse);


                    
                    other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().
                  addCurrentChargingBar(chargeAmount);


                }
           
            }
        }
        else
        {
            if (other.GetComponent<weaponBase>() == null)//don have this script (mean hit a wall)
                gameObject.SetActive(false);
        }


    }
}









