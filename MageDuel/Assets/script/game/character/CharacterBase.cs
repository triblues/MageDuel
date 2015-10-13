/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour {

	public LayerMask groundMask;
	public LayerMask targetMask;
	
	
	[SerializeField] bool isAI = false;
	[SerializeField] protected int characterTag;
	[SerializeField] protected float startingHealth = 100.0f;
	[SerializeField] protected float startingMana = 100.0f;
	[Tooltip("minimum is 0, maximum is 1")]
	[SerializeField] protected float CurrentChargingBar = 0;

	[SerializeField] protected float normalSpeed = 10.0f;
	[SerializeField] protected float fastSpeed = 20.0f;
	[SerializeField] protected float offFactor = 1.0f;//offensive power
	[SerializeField] protected float defFactor = 1.0f;//defensive power 
	[SerializeField] protected float lowJumpSpeed = 10.0f;
	[SerializeField] protected float highJumpSpeed = 30.0f;
	[SerializeField] protected float fallSpeed = 0.5f;
	[SerializeField] protected float coolDownRangeAttackRate = 0.5f;
	[SerializeField] protected float coolDownMeleeAttackRate = 0.5f;
    [SerializeField] protected float coolDownStunRate = 0.5f;
	[SerializeField] protected float manaRegenRate = 1.0f;
	[SerializeField] float distanceToGround = 0.1f;//the amount of dist to stop falling

    protected GameObject enemy;
    protected Text comboText;
    protected Animation comboAnimation;//this is the combo text animation,fade in/out
    protected int comboCount;
	protected float jumpSpeed;
	protected float speed;
	protected melee mymelee;
    protected float stunTimer;
    protected float coolDownRangeTimer;
	protected float[] coolDownMeleeTimer;
    protected float currentHealth;
    protected float currentMana;
	protected Vector3 movement;                   // The vector to store the direction of the player's movement.
	protected Vector3 jumpingMovement;
	protected Rigidbody rb;          // Reference to the player's rigidbody.
	protected float horizontal;
	protected float jumping;
	protected bool isJumping;
	protected bool canRangeAttack;
	protected bool canMeleeAttack;
	protected bool isLose;
    protected bool isStun;
    protected bool isFinish;
    protected bool isBlockLeft;//this determine the direction the character should be going to activate block
	protected bool isBlocking;
    protected bool isCastMode;
	protected bool shouldWaitAnimationFinish;
    protected bool canCombo;//this check to determine if player hit the enemy the 1st time and let them do combo
	protected Animator myAnimator;
	protected gameController myGameController;//for all the spawning object pool

    //ui stuff
    protected Text healthText;
    protected Text manaText;
    protected GameObject combo;
    protected Image chargingBar;//charging bar inner
    //ui stuff

    float doubleTapTimer;
	float highJumpTimer;
	float tapspeed = 0.3f;
	bool isDoubleTap;


    // Use this for initialization
    protected virtual void Awake () {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;

       
        mymelee = transform.Find ("melee trigger box").GetComponent<melee> ();
        
        rb = GetComponent<Rigidbody> ();
		isJumping = false;
        isCastMode = false;
        isLose = false;
        isFinish = false;
        isStun = false;
        canCombo = false;
		shouldWaitAnimationFinish = false;
        isBlocking = false;
        isDoubleTap = false;
        canRangeAttack = true;
        canMeleeAttack = true;

        speed = normalSpeed;
        jumpSpeed = lowJumpSpeed;
        comboCount = 0;
        CurrentChargingBar = Mathf.Clamp01 (CurrentChargingBar);

		
		myGameController = GameObject.Find ("gameManager").GetComponent<gameController> ();
		coolDownRangeTimer = coolDownRangeAttackRate;

        coolDownMeleeTimer = new float[2];
        coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
        stunTimer = coolDownStunRate;
        comboText = combo.GetComponent<Text>();
        comboAnimation = combo.GetComponent<Animation>();
        //myAnimator = GetComponent<Animator> ();
        StartCoroutine (regenMana (0.5f));

    }

	void Start()
	{
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();
        comboText.text = "Combo: " + comboCount.ToString();
        chargingBar.fillAmount = CurrentChargingBar ;
	}
	IEnumerator regenMana(float time)
	{
		while(true)
		{
			if(currentMana < startingMana)
            {
                currentMana += manaRegenRate;
                manaText.text = "Mana: " + currentMana.ToString();
            }
				

			yield return new WaitForSeconds(time);
		}
	//	yield return null;
	}
    protected virtual void Update()
	{
		
		
        

		checkCoolDown ();   
		Move ();
		jump ();
		crouch ();
		if (currentMana <= 0)
			currentMana = 0;

        if (comboAnimation.IsPlaying("fade") == false)
            comboCount = 0;

    }
    public void TakesDamage(float damage)
    {
        // Damage will be adjusted
        // Normally, damage will have value of 10. If the player has a defensive factor value of 0.75:
        // damage = 10 * (1 + (1 - 0.75))
        //        = 10 * (1.25) 
        //        = 12.5
        // The player will take more damage (12.5) because of its low defensive factor
        damage = damage * (1 + (1 - defFactor));
        // Reduce health
        currentHealth -= damage;
        isStun = true;

        checkDead();
        healthText.text = "Health: " + currentHealth.ToString();

        
    }
	public void setMana(float amount)
	{
		currentMana += amount;
        manaText.text = "Mana: " + currentMana.ToString();
    }
    //public void setHealth(float amount)
    //{
    //    currentHealth += amount;
       
    //}
    /*****************GETTER**************************************/
    // This function returns speed
    public float GetSpeed()
    {
        return speed;
    }
    // This function returns off factor
    public float GetOffFactor()
    {
        return offFactor;
    }
    // This function returns def factor
    public float GetDefFactor()
    {
        return defFactor;
    }
	public float getDistanceToGround()
	{
		return distanceToGround;
	}
	public float getJumpSpeed()
	{
		return jumpSpeed;
	}
	 public float getFallSpeed()
	 {
		return fallSpeed;
	 }
	public int getCharacterTag()
	{
		return characterTag;
	}
    public void setComboCount(int amount)
    {
        comboCount += amount;
        comboText.text = "Combo: " + comboCount.ToString();

        comboAnimation.Play("fade");
        comboAnimation["fade"].time = 0;
    }
    public int getComboCount()
    {
        return comboCount;
    }
    public bool getIsCastMode()
    {
        return isCastMode;
    }
    public GameObject getEnemy()
	{
		return enemy;
	}
    
    public bool getIsStun()
    {
        return isStun;
    }
    public bool getCanCombo()
    {
        return canCombo;
    }
    public void setCanCombo(bool _combo)
    {
        canCombo = _combo;
    }
    public void setFirstCoolDownMeleeTimer()
    {
        coolDownMeleeTimer[0] = Time.time;
    }
    public bool getIsBlocking()
	{
		return isBlocking;
	}
	public void addCurrentChargingBar(float amount)
	{
        if (isCastMode == false)
        {
            CurrentChargingBar += amount;
            chargingBar.fillAmount = CurrentChargingBar;
        }
    }
    public void stopMoving()
    {
        Time.timeScale = 0;
    }
    public void startMoving()
    {
        Time.timeScale = 1;
    }
    public bool getIsLose()
    {
        return isLose;
    }
    public void setisFinish(bool finish)
    {
        isFinish = finish;
    }
    protected bool shouldTurn(Vector3 myself,Vector3 enemy)
	{
		if (myself.x > enemy.x)
			return true;
		else
			return false;
	}
	protected float getCurrentMana()
	{
		return currentMana;
	}
	protected void checkCoolDown()
	{
		coolDownRangeTimer -= Time.deltaTime;

        if(isStun == true)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                stunTimer = coolDownStunRate;
                isStun = false;
            }
                
        }

        if (canCombo == false)
            coolDownMeleeTimer[0] -= Time.deltaTime;//1st melee attack

        if (coolDownRangeTimer <= 0)
			canRangeAttack = true;
		else
			canRangeAttack = false;
        if (canCombo == false)
        {
            if (coolDownMeleeTimer[0] <= 0)
                canMeleeAttack = true;
            else
                canMeleeAttack = false;
        }
        else
            canMeleeAttack = true;

    }

    /*****************SCALE***********************/
   /* Offensive Factor (OffFactor): 0.75 - 1.25 (Weak - Strong)
      Defensive Factor (DefFactor): 0.75 - 1.25 (Weak - Strong)
   */
	public virtual void checkDead()
	{
		if (currentHealth <= 0)
		{
            currentHealth = 0;
            //myAnimator.SetBool ("die", true);
            gameObject.SetActive (false);
            isLose = true;
            isFinish = true;
            enemy.GetComponent<CharacterBase>().setisFinish(true);
        }
	}
	protected virtual void attack()
	{
//		if (currentMana <= 0)
//			return;
//		if (canRangeAttack == false)
//			return;
		//myAnimator.SetBool ("attack", true);
	//	StartCoroutine (WaitForAnimation ("attack"));
	}
	protected virtual void Move()
	{

		if(isAI == false)
			horizontal = Input.GetAxisRaw ("Horizontal");

		if (Input.GetButtonDown ("Horizontal"))
		{
			if((Time.time - doubleTapTimer) < tapspeed){
				

				isDoubleTap = true;
				speed = fastSpeed;
			}
			
			doubleTapTimer = Time.time;
		}
		if(Input.GetButtonUp("Horizontal"))
		{
			speed = normalSpeed;
		}

		//if(isBlockLeft == true)
		//{
		//	if(horizontal < 0)//going left side
		//		isBlocking = true;
		//	else
		//		isBlocking = false;
		//}
		//else
		//{
		//	if(horizontal > 0)//going right side
		//		isBlocking = true;
		//	else
		//		isBlocking = false;
		//}

		
//		if (horizontal >= 1 || horizontal < 0)
//			myAnimator.SetBool ("walk", true);
//		else//equal to 0
//			myAnimator.SetBool ("walk", false);

        if(isStun == false)
        {
            movement.x = horizontal * speed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, rb.velocity.z);
        }
			
	}
	protected virtual void crouch()
	{
        if (isStun == false)
        {
            if (Input.GetKeyDown("s"))
            {
                highJumpTimer = Time.time;

            }
        }

	}
	protected virtual void jump()
	{
        
		if(isAI == false)
			jumping = Input.GetAxisRaw ("Jump");

		if(Input.GetButtonDown("Jump"))
		{
            if (isStun == false)
            {
                if (Time.time - highJumpTimer < tapspeed)
                {
                    jumpSpeed = highJumpSpeed;
                }
            }
		}
		if(jumping > 0 && check_touchGround(groundMask) == true)//player press jump while on the ground
		{
            if (isStun == false)
            {
                jumpingMovement.y = jumpSpeed;
                rb.velocity = new Vector3(rb.velocity.x, jumpingMovement.y,
                                               rb.velocity.z);
                isJumping = true;
            }


		}
		else if(jumping == 0 && check_touchGround(groundMask) == true)//player reach the ground
		{
		
			jumpingMovement.y = 0;
			isJumping = false;
			jumpSpeed = lowJumpSpeed;

		}
		else//player still in the air
		{

			jumpingMovement.y = -fallSpeed;
			rb.AddForce(jumpingMovement,ForceMode.Acceleration);
			
		}

		if (check_touchGround (targetMask) == true)//prevent player from standing on top
		{
			Debug.Log("touch");
			rb.AddForce (transform.forward * -speed * 2,ForceMode.VelocityChange);
		}
		

	}
	protected void rangeAttack(Vector3 position, Vector3 direction,gameController.projectileType myType)
	{
		GameObject temp = myGameController.getPoolObjectInstance(myType).getPoolObject ();
		
		if (temp == null)
			return;
		weaponBase projectile = temp.GetComponent<weaponBase> ();
		if (currentMana < projectile.getConsumeMana ())//not enough mana to cast spell
			return;
		
		temp.transform.position = position + direction.normalized;
		temp.SetActive (true);
		projectile.launch (direction);
		projectile.setTag (characterTag);
		setMana (-projectile.getConsumeMana ());
		coolDownRangeTimer = coolDownRangeAttackRate;
	}

	protected void meleeAttack()
	{
		if(canMeleeAttack == true)
		{
            Debug.Log("melee");
			

          
            mymelee.enabled = true;
            if(canCombo == false)
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
         
        }

	}
   
	protected bool check_touchGround(LayerMask mymask)
	{

		if(Physics.Raycast(transform.position,Vector3.down,distanceToGround,mymask) == true)//origin,direction,max dist
		{
			//Debug.Log("touch ground");
			return true;//player touch the ground
		}
		else
			return false;
	}
	public int getRandomNum(int min,int max)
	{
		int rand = Random.Range (min, max);
		return rand;
	}
	IEnumerator WaitForAnimation ( string name )
	{
		yield return new WaitForSeconds(0.5f);
		while(myAnimator.GetCurrentAnimatorStateInfo(0).IsName(name) == true)
		{
			yield return null;
		}
		myAnimator.SetBool (name, false);
		yield return null;
	}
   

}







