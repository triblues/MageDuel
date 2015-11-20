using UnityEngine;
using System.Collections;

public class weaponBase : MonoBehaviour {

	[SerializeField] protected float damage;
	[SerializeField] protected float speed;
	[SerializeField] protected float deSpawn_Time;
	[SerializeField] protected float consumeMana;
	[SerializeField] protected float chargeAmount;//the amount increase to charging bar
	[SerializeField] protected float knockBack;
    protected float damageMultipler;
	protected Vector3 movement;
	protected int numTag;//store the number that the character fire this attack
    protected int comboCount;
	protected float totalTime;
    protected float powerLevel;
	// Use this for initialization
	protected virtual void Start () {

        powerLevel = 1.0f;//default
        comboCount = 1;//default
        damageMultipler = 1.0f;//default
    }

    protected virtual void Update()
    {
        if (deSpawn_Time == 0)//unlimited
            return;

        totalTime -= Time.deltaTime;

        if (totalTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void setMultipler(float amount)
    {
        damageMultipler = amount;
    }
    public float getMultipler()
    {
        return damageMultipler;
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
                    other.GetComponent<CharacterBase>().TakesDamage((damage * damageMultipler) * powerLevel);
                    other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().setComboCount(comboCount);

                    if (other.GetComponent<CharacterBase>().getisKnockBack() == true)
                    {
                        other.GetComponent<Rigidbody>().AddForce(-other.GetComponent<Transform>().forward * knockBack,
                                                             ForceMode.Impulse);
                    }


                    other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().
                  addCurrentChargingBar(chargeAmount);


                }

            }
        }
        else
        {
            if (other.GetComponent<weaponBase>() == null)//don have this script (mean hit a wall)
            {
                gameObject.SetActive(false);
               // Debug.Log("here 0");
            }
        }


    }
}









