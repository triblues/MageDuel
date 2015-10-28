/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour {

	public LayerMask groundMask;
	public LayerMask targetMask;
    public static bool isCastModeAnimation;

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
  
    [SerializeField] protected float coolDownBlockTimer = 1.5f;
    [SerializeField] protected float manaRegenRate = 1.0f;
	[SerializeField] float distanceToGround = 0.1f;//the amount of dist to stop falling
    [SerializeField] protected int maxBlockCount = 5;

    protected GameObject enemy;
    protected Text comboText;
    protected Animation comboAnimation;//this is the combo text animation,fade in/out
    protected int comboCount;
    protected int blockCount;//for blocking
	protected float jumpSpeed;
	protected float speed;
	protected melee mymelee;
    protected float stunTimer;
    protected float canMoveTimer;
    protected float myblockTimer;
    protected float stunRate;
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
    protected bool isInComboAnimation;
    protected bool isStun;
    protected bool isEndOfRangeAttack;
    protected bool isAttack;
    protected bool playBlockAnimation;
    protected bool isWalking;
    protected bool isCrouch;
   
    protected bool isBlockLeft;//this determine the direction the character should be going to activate block
	protected bool isBlocking;
    protected bool isCastMode;
    
    protected bool shouldWaitAnimationFinish;
    protected bool canCombo;//this check to determine if player hit the enemy the 1st time and let them do combo
    protected bool[] isMeleeComboCount;
    protected bool isFinishCombo;
    protected Animator myAnimator;
	protected gameController myGameController;//for all the spawning object pool

    //ultimate camera
    protected ultimateCameraController myUltiCamera;
    //ultimate camera

    //block stuff
    protected blockController myblockController;
    //block stuff

    //ui stuff
    protected Image healthBar;
    protected Image manaBar;
    protected GameObject combo;
    protected Image chargingBar;//charging bar inner
    //ui stuff

    float doubleTapTimer;
	float highJumpTimer;
	float tapspeed = 0.3f;
	bool isDoubleTap;

    //networking stuff
  //  protected NetworkInstanceId mynetworkID;
    

    // Use this for initialization
    protected virtual void Awake () {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;

        if (isAI == false)
            myblockController = transform.Find("block").GetComponent<blockController>();
        myUltiCamera = GameObject.FindGameObjectWithTag("ultimateCamera").GetComponent<ultimateCameraController>();
        Debug.Log(myUltiCamera.name.ToString());
        mymelee = transform.Find ("melee trigger box").GetComponent<melee> ();
        
        rb = GetComponent<Rigidbody> ();
		isJumping = false;
        isAttack = false;
        isCastMode = false;
        playBlockAnimation = false;
        isLose = false;
        isWalking = false;
        isEndOfRangeAttack = true;

        isFinishCombo = true;
        isInComboAnimation = false;
        isStun = false;
        isCrouch = false;
        canCombo = false;
		shouldWaitAnimationFinish = false;
        isBlocking = false;
        isDoubleTap = false;
        canRangeAttack = true;
        canMeleeAttack = true;

        speed = normalSpeed;
        jumpSpeed = lowJumpSpeed;
        comboCount = 0;
        blockCount = maxBlockCount;
        stunRate = 1;//default
        CurrentChargingBar = Mathf.Clamp01 (CurrentChargingBar);

		
		myGameController = GameObject.Find ("gameManager").GetComponent<gameController> ();
		//coolDownRangeTimer = coolDownRangeAttackRate;
        coolDownRangeTimer = 0;

        coolDownMeleeTimer = new float[2];
        coolDownMeleeTimer[0] = coolDownMeleeAttackRate;

        isMeleeComboCount = new bool[3];
        for (int i = 0; i < isMeleeComboCount.Length; i++)
            isMeleeComboCount[i] = false;//for melee combo animation

      
        myblockTimer = coolDownBlockTimer;
        stunTimer = coolDownStunRate;
        comboText = combo.GetComponent<Text>();
        comboAnimation = combo.GetComponent<Animation>();
        
        StartCoroutine (regenMana (0.5f));
        StartCoroutine(regenBlockCount(2.0f));
    }

	protected virtual void Start()
	{
      
        healthBar.fillAmount = currentHealth / 100;
        manaBar.fillAmount = currentMana / 100;
        comboText.text = "Combo: " + comboCount.ToString();
        chargingBar.fillAmount = CurrentChargingBar ;
        if(isAI == false)
        myAnimator = transform.Find("model").GetComponent<Animator>();
    }
    IEnumerator regenMana(float time)
	{
		while(true)
		{
			if(currentMana < startingMana)
            {
                currentMana += manaRegenRate;
                manaBar.fillAmount = currentMana / 100;
                //manaText.text = "Mana: " + currentMana.ToString();
            }
				

			yield return new WaitForSeconds(time);
		}

	}
    IEnumerator regenBlockCount(float time)
    {
        while(true)
        {
            if(blockCount < maxBlockCount)
            {
                blockCount++;
            }
            yield return new WaitForSeconds(time);
        }
    }
    protected virtual void Update()
	{
       // if(isAI == false)
		checkCoolDown ();
        
        //checkComboAnimation();
        Move ();
		jump ();
		crouch ();
        //if (isAI == false)
        //    setAnimation();

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

        if (damage <= 0)
            return;

        damage = damage * (1 + (1 - defFactor));
        // Reduce health
        currentHealth -= damage;
        Debug.Log("damage: " + damage.ToString());
        stunTimer = coolDownStunRate * stunRate;
        isStun = true;

        checkDead();
        healthBar.fillAmount = currentHealth / 100;

        
    }
	public void setMana(float amount)
	{
		currentMana += amount;
        manaBar.fillAmount = currentMana / 100;
    }
  
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
    public void setStunRate(float amount)
    {
        playBlockAnimation = false;
        stunRate = amount;
    }
    public void setBlockAnimation()
    {
        blockCount--;
        if (blockCount <= 0)
        {
            playBlockAnimation = false;
            //isBlocking = false;
            return;
        }
        isWalking = false;
        isDoubleTap = false;
        myblockController.animateBlock(blockCount,maxBlockCount);
        myblockTimer = coolDownBlockTimer;
        playBlockAnimation = true;
        

        //Debug.Log("blockcount: " + blockCount.ToString());
    }
    public void setComboCount(int amount)
    {
        if (amount <= 0)
            return;
        comboCount += amount;
        comboText.text = "Combo: " + comboCount.ToString();

        comboAnimation.Play("fade");
        comboAnimation["fade"].time = 0;
    }
    public int getComboCount()
    {
        return comboCount;
    }
   
    public GameObject getEnemy()
	{
		return enemy;
	}
    public bool getisDoubleTap()
    {
        return isDoubleTap;
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
        Debug.Log("at cancombo: " + canCombo.ToString());
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
        if(isStun == true)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                stunTimer = coolDownStunRate;
                isStun = false;
            }
                
        }
       
        if(playBlockAnimation == true)
        {
            myblockTimer -= Time.deltaTime;
            if(myblockTimer <= 0)
            {
                myblockTimer = coolDownBlockTimer;
                playBlockAnimation = false;
            }
        }

        coolDownRangeTimer -= Time.deltaTime;

        if (coolDownRangeTimer <= 0)
        {
            canRangeAttack = true;
           // if (isAI == false)
              //  myAnimator.SetBool("rangeAttack", false);
        }
        else
            canRangeAttack = false;

        if (isAI == false)
        {
            if (canCombo == false)
            {
                if (isMeleeComboCount[0] == true)
                {
                  //  Debug.Log("melee time: " + coolDownMeleeTimer[0].ToString());
                    coolDownMeleeTimer[0] -= Time.deltaTime;//1st melee attack
                    if (coolDownMeleeTimer[0] <= 0)
                    {
                        myAnimator.SetBool("meleeAttack1", false);
                        //isAttack = false;
                        canMeleeAttack = true;
                        isMeleeComboCount[0] = false;
                    }
                    else
                        canMeleeAttack = false;
                }
            }
            else
            {
                if (isMeleeComboCount[2] == true)//doing final combo
                {
                    Debug.Log("in melee count 2");
                    myAnimator.SetBool("meleeAttack1", false);
                    //myAnimator.SetBool("meleeAttack2", false);
                }
                else if (isMeleeComboCount[1] == true)//doing 2nd combo now
                {

                    coolDownMeleeTimer[1] -= Time.deltaTime;//2nd melee attack
                    if (coolDownMeleeTimer[1] <= 0)
                    {
                        myAnimator.SetBool("meleeAttack1", false);
                        myAnimator.SetBool("meleeAttack2", false);
                        isAttack = false;
                        isMeleeComboCount[0] = false;
                        isMeleeComboCount[1] = false;
                        canCombo = false;
                        Debug.Log("i am here 2");
                    }
                }
                else if (isMeleeComboCount[0] == true)//doing 1st combo now
                {

                    coolDownMeleeTimer[0] -= Time.deltaTime;//1st melee attack
                    if (coolDownMeleeTimer[0] <= 0)
                    {
                        myAnimator.SetBool("meleeAttack1", false);

                        isAttack = false;
                        isMeleeComboCount[0] = false;
                        canCombo = false;
                        Debug.Log("i am here 1");
                    }
                }


                canMeleeAttack = true;
            }
        }

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

            isStun = false;
            isLose = true;
             gameController.isFinish = true;
            //enemy.GetComponent<CharacterBase>().setisFinish(true);

           
        }
	}
	protected virtual void attack()
	{
//		if (currentMana <= 0)
//			return;
//		if (canRangeAttack == false)
//			return;
		
	}
    protected void rangeAttackAnimation()
    {
        
        if (isStun == true)
            return;
        isAttack = true;

        myAnimator.SetTrigger("rangeAttack");
        //myAnimator.SetBool("rangeAttack", true);
        StartCoroutine(WaitForAnimation("range attack",0));
        
    }

	protected virtual void Move()
	{
        if (Input.GetButtonUp("Horizontal"))
        {
            speed = normalSpeed;
            isDoubleTap = false;
            isWalking = false;

        }
        if (isCastModeAnimation == true)
            return;


        if (isAttack == true)
        {
         
            speed = normalSpeed;
            isDoubleTap = false;
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            return;
        }
      
        if (isCrouch == true)
            return;

		if(isAI == false)
			horizontal = Input.GetAxisRaw ("Horizontal");

       
        if (Input.GetButtonDown ("Horizontal"))
		{
			if((Time.time - doubleTapTimer) < tapspeed)
            {

                //isWalking = false;
                isDoubleTap = true;
				speed = fastSpeed;
             
             
			}
          
			doubleTapTimer = Time.time;
		}
		


        if (isJumping == false && isDoubleTap == false)
        {
            if (playBlockAnimation == false)
            {
                if (horizontal >= 1 || horizontal < 0)
                {
                 
                    isWalking = true;
                }
                else//equal to 0
                {
                    isWalking = false;
                
                }
             }
            //else
            //{
              
            //    isWalking = false;
            //}
            
           
        }
      
        if (isStun == false && playBlockAnimation == false)
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
                //if (isAttack == false && isCastModeAnimation == false)
                if(isCastModeAnimation == false)
                    isCrouch = true;
              

            }
           
        }
        if (Input.GetKeyUp("s"))
        {
            isCrouch = false;

        }

    }
	protected virtual void jump()
	{

        if (isCastModeAnimation == true)
        {
            return;
        }
        if (isAI == false)
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
		if(jumping > 0 && check_touchGround(groundMask, distanceToGround) == true)//player press jump while on the ground
		{
        
            if (isStun == false && isAttack == false)
            {
                jumpingMovement.y = jumpSpeed;
                rb.velocity = new Vector3(rb.velocity.x, jumpingMovement.y,
                                               rb.velocity.z);
                isJumping = true;
                isCrouch = false;
            }
           

		}
		else if(jumping == 0 && check_touchGround(groundMask, distanceToGround) == true)//player reach the ground
		{
       
			jumpingMovement.y = 0;
			isJumping = false;
			jumpSpeed = lowJumpSpeed;
        

        }
		else//player still in the air
		{

            jumpingMovement.y = -fallSpeed;
            rb.AddForce(jumpingMovement, ForceMode.Acceleration);
            if (transform.position.y < 0)
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        }

		if (check_touchGround (targetMask,0.1f) == true)//prevent player from standing on top
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
        isEndOfRangeAttack = false;
    }

	protected void meleeAttack()
	{
        if (isStun == true)
            return;
        if (canMeleeAttack == true)
		{
           
            isAttack = true;
            mymelee.enabled = true;
       
        }

	}
   
  
	protected bool check_touchGround(LayerMask mymask,float dist)
	{
        if (Physics.Raycast(transform.position,  -transform.up,dist,mymask) == true)//origin,direction,max dist
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
    protected void resetAnimation()
    {
        myAnimator.SetBool("walk", false);

        myAnimator.SetBool("dash", false);
        myAnimator.SetBool("jump", false);
        myAnimator.SetBool("crouch", false);
        myAnimator.SetBool("defend", false);
        myAnimator.SetBool("stun", false);
        //myAnimator.SetBool("die", true);

        myAnimator.SetBool("meleeAttack1", false);
        myAnimator.SetBool("meleeAttack2", false);
        myAnimator.SetBool("meleeAttack3", false);
        myAnimator.SetBool("finishCombo", false);
    }
    protected void setAnimation()
    {
        
        myAnimator.SetBool("walk", isWalking);

        myAnimator.SetBool("dash", isDoubleTap);
        myAnimator.SetBool("jump", isJumping);
        myAnimator.SetBool("crouch", isCrouch);
        myAnimator.SetBool("defend", playBlockAnimation);
        myAnimator.SetBool("stun", isStun);
        myAnimator.SetBool("die", isLose);

        //myAnimator.SetBool("meleeAttack1", isMeleeComboCount[0]);
        //myAnimator.SetBool("meleeAttack2", isMeleeComboCount[1]);
        //myAnimator.SetBool("meleeAttack3", isMeleeComboCount[2]);
        //myAnimator.SetBool("finishCombo", true);

       
    }
	protected IEnumerator WaitForAnimation ( string name,int count )
	{
		yield return new WaitForSeconds(0.5f);
		while(myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == true)//the animation is still running
		{
          
            yield return new WaitForSeconds(0.2f);
		//	yield return null;
		}
        if (name == "range attack")
        {
            Debug.Log("end range");
            isAttack = false;
            isEndOfRangeAttack = true;
        //    myAnimator.SetBool("rangeAttack", false);

        }
        else if(name == "melee 1")
        {
            if (canCombo == false)
                isAttack = false;
           
        }
        else if (name == "melee 3")
        {
            if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
            {
              
                enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);

            }
            Debug.Log("finish combo");
            isAttack = false;
            canCombo = false;
            canMeleeAttack = true;
            isMeleeComboCount[0] = false;
            isMeleeComboCount[1] = false;

            isMeleeComboCount[2] = false;
            myAnimator.SetBool("finishCombo", false);
            myAnimator.SetBool("meleeAttack2", false);
            myAnimator.SetBool("meleeAttack3", false);


        }
       
		yield return null;
	}

    
   
}







