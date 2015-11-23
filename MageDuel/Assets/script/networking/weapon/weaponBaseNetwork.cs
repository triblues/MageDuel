using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class weaponBaseNetwork : NetworkBehaviour
{
    [SyncVar(hook = "changeActive")]
    protected bool isActive;
    
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float deSpawn_Time;
    [SerializeField]
    protected float consumeMana;
    [SerializeField]
    protected float chargeAmount;//the amount increase to charging bar
    [SerializeField]
    protected float knockBack;
    protected Vector3 movement;
    public int numTag;//store the number that the character fire this attack
    protected int comboCount;
    protected float totalTime;
    protected Vector3 mypos;
    protected float damageMultipler;
  

   
    // Use this for initialization
    protected virtual void Start()
    {

        damageMultipler = 1;//default
        comboCount = 1;//default
      
        //deSpawn_Time = 0;
        gameObject.SetActive(false);

    }
    [ClientCallback]
    protected void changeActive(bool _isActive)
    {
        if (_isActive == false)
            transform.position = new Vector3(0, 100, 4);
        gameObject.SetActive(_isActive);
    }
    [ClientCallback]//command can only be send from local player
    protected void trasmitActive(bool _active)
    {

        CmdSendActiveToServer(_active);


    }
    [Command]//a command to send to the server
    protected void CmdSendActiveToServer(bool _isActive)//must have Cmd as the start of the name, this function only run in the server
    {
        isActive = _isActive;
        
    }
    protected virtual void OnEnable()
    {  
      
         trasmitActive(true);
        totalTime = deSpawn_Time;
    }

    protected virtual void OnDisable()
    {
      
        trasmitActive(false);
    }
    protected virtual void Update()
    {
        if (deSpawn_Time == 0)//unlimited
            return;

        totalTime -= Time.deltaTime;

        if (totalTime <= 0)
        {
            //trasmitActive(false);
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
        if (other.GetComponent<CharacterBaseNetwork>() != null)//has this script
        {
            if (other.GetComponent<CharacterBaseNetwork>().getCharacterTag() != numTag)//prevent own attack to hit ownself
            {
                if (other.GetComponent<CharacterBaseNetwork>().getIsBlocking() == false)
                {
                    Debug.Log("take damage");
                    other.GetComponent<CharacterBaseNetwork>().TakesDamage(damage * damageMultipler);
                 
                    other.GetComponent<CharacterBaseNetwork>().getEnemy().GetComponent<CharacterBaseNetwork>().setComboCount(comboCount);

                    if (other.GetComponent<CharacterBaseNetwork>().getisKnockBack() == true)
                    {
                        other.GetComponent<Rigidbody>().AddForce(-other.GetComponent<Transform>().forward * knockBack,
                                                             ForceMode.Impulse);
                    }


                    other.GetComponent<CharacterBaseNetwork>().getEnemy().GetComponent<CharacterBaseNetwork>().
                  addCurrentChargingBar(chargeAmount);


                }

            }
        }
        else
        {
            if (other.GetComponent<weaponBaseNetwork>() == null)//don have this script (mean hit a wall)
            {
                //trasmitActive(false);
                gameObject.SetActive(false);

            }
        }


    }
    
}









