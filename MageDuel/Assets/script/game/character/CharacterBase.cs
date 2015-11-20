/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour {

	public LayerMask groundMask;
	public LayerMask targetMask;
    //public static bool isCastModeAnimation;

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
    [SerializeField] public int ultimateDamage = 30;

    protected GameObject enemy;
    protected Transform enemyTrans;
    protected Text comboText;
    protected Animation comboAnimation;//this is the combo text animation,fade in/out
    protected int comboCount;
    protected int blockCount;//for blocking
	protected float jumpSpeed;
	protected float speed;
	protected melee mymelee;
    protected bool isKnockBack;
    protected float stunTimer;
    protected float myblockTimer;
    protected float stunRate;
    protected float coolDownRangeTimer;
	protected float[] coolDownMeleeTimer;
    protected float currentHealth;
    protected int highestComboAchieve;
    protected float currentMana;
    protected float myDamageMultipler;
    protected int spellCoolDownRate;
	protected Vector3 movement;                   // The vector to store the direction of the player's movement.
	protected Vector3 jumpingMovement;
	protected Rigidbody rb;          // Reference to the player's rigidbody.
	protected float horizontal;
	protected float jumping;
	protected bool isJumping;
	protected bool canRangeAttack;
	protected bool canMeleeAttack;
	protected bool isLose;
    protected bool isGodMode;
    protected bool isUnlimitedSpell;
    protected bool canMove;
    protected bool canCastUltimate;
    protected bool isStun;
    protected bool isEndOfRangeAttack;
    protected bool isAttack;
    protected bool isPause;
    protected bool playBlockAnimation;
    protected bool isWalking;
    protected bool isCrouch;
    protected bool isNotEnoughMana;
    protected bool isInResult;
    protected bool isUseHealthItem;
    protected bool isUseManaItem;

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
    protected bool isInUltimate;
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
	protected bool isDoubleTap;

    //audio
    public AudioClip gotHitSound;
    public AudioClip jumpedSound;
    public AudioClip attackSound;
    protected AudioSource myaudio;
    //audio

    //particle stuff
    protected ParticleSystem myArmorPS;//armor
    protected ParticleSystem myActivePS;//active
    protected ParticleSystem myPassivePS;//passive
    protected ParticleSystem myUltimatePS;//ultimate
    protected GameObject ultimateObj;
    //particle stuff

    protected UICoolDown UIarmorCD;
    protected UICoolDown UIActiveCD;
    protected float[] spellComboArmor;
    protected float[] spellComboActive;
    protected float[] spellComboPassive;
    protected bool[] canCastSpell;

    //networking stuff
    //  protected NetworkInstanceId mynetworkID;


    // Use this for initialization
    protected virtual void Awake () {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;

      
        myblockController = transform.Find("block").GetComponent<blockController>();
        myUltiCamera = GameObject.FindGameObjectWithTag("ultimateCamera").GetComponent<ultimateCameraController>();
       
        mymelee = transform.Find ("melee trigger box").GetComponent<melee> ();
        myaudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody> ();
        isInUltimate = false;
        isJumping = false;
        isAttack = false;
        isCastMode = false;
        playBlockAnimation = false;
        isLose = false;
        canMove = true;
        isWalking = false;
        isPause = false;
        canCastUltimate = true;
        isGodMode = false;
        isUnlimitedSpell = false;

        isNotEnoughMana = false;
        isKnockBack = true;
        isEndOfRangeAttack = true;

        isFinishCombo = true;
        isUseHealthItem = false;
        isUseManaItem = false;
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
        myDamageMultipler = 1.0f;//default
        highestComboAchieve = 0;
        blockCount = maxBlockCount;
        stunRate = 1;//default
        spellCoolDownRate = 1;//how fast the spell cooldown, 1 is normal rate
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

        spellComboArmor = new float[3];
        spellComboActive = new float[3];
        spellComboPassive = new float[3];
        for (int i = 0; i < spellComboArmor.Length; i++)
        {
            spellComboArmor[i] = 0;
            spellComboActive[i] = 0;
            spellComboPassive[i] = 0;
        }
        canCastSpell = new bool[3];
        for (int i = 0; i < canCastSpell.Length; i++)
            canCastSpell[i] = true;

        StartCoroutine (regenMana (0.5f));
        StartCoroutine(regenBlockCount(5.0f));
    }

	protected virtual void Start()
	{
      
        healthBar.fillAmount = currentHealth / 100;
        manaBar.fillAmount = currentMana / 100;
        comboText.text = "Combo: " + comboCount.ToString();
        if(isAI == false)
            chargingBar.fillAmount = CurrentChargingBar ;
      
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
        pause();
        //if (isPause == true)
          //  return;

        checkCoolDown ();
        cheatCode();

        Move ();
		jump ();
		crouch ();
        useItem();



        if (currentMana <= 0)
			currentMana = 0;

        if (comboAnimation.IsPlaying("fade") == false)
        {
            if(comboCount > highestComboAchieve)
                highestComboAchieve = comboCount;

            comboCount = 0;
        }

    }
    public void TakesDamage(float damage)
    {

        // Damage will be adjusted
        // Normally, damage will have value of 10. If the player has a defensive factor value of 0.75:
        // damage = 10 * (1 + (1 - 0.75))
        //        = 10 * (1.25) 
        //        = 12.5
        // The player will take more damage (12.5) because of its low defensive factor
        
        if (launchScene.isPractice == true)
        {
            damage = 0;
        }
        if(isGodMode == true)
        {
            damage = 0;
        }
        Debug.Log(damage.ToString());
        if (damage < 0)//mean heal
        {
            currentHealth -= damage;
            if (currentHealth >= startingHealth)
                currentHealth = startingHealth;
        }
        else
        {
          
            myaudio.PlayOneShot(gotHitSound);

            damage = Mathf.Abs(damage * (1 + (1 - defFactor)));
            // Reduce health
            currentHealth -= damage;

          


            stunTimer = coolDownStunRate * stunRate;

            if (isKnockBack == true)
            {
                isStun = true;
                // stunTimer = coolDownStunRate;
                myAnimator.SetBool("stun", isStun);
            }


            checkDead();
            
        }
        healthBar.fillAmount = currentHealth / 100;
    }
    protected void useItem()
    {
        if (isAI == true)
            return;
        if (launchScene.isPractice == true)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isUseHealthItem == false)
            {
                if (PlayerPrefs.GetInt("Health Potion") > 0)
                {
                    isUseHealthItem = true;
                    TakesDamage(-20.0f);
                    PlayerPrefs.SetInt("Health Potion", PlayerPrefs.GetInt("Health Potion") - 1);
                }
            }
            Debug.Log("press 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(isUseManaItem == false)
            {
                if (PlayerPrefs.GetInt("Mana Potion") > 0)
                {
                    isUseManaItem = true;
                    setMana(20.0f);

                    PlayerPrefs.SetInt("Mana Potion", PlayerPrefs.GetInt("Mana Potion") - 1);
                }
            }
            Debug.Log("press 1");
        }
    }
    public virtual void showHitEffect()
    {
        //use to show some particle effect when get hit
    }
    public void setSpeed(float rate)
    {
        normalSpeed = normalSpeed * rate;
        fastSpeed = fastSpeed * rate;
    }
	public void setMana(float amount)
	{
        if(isGodMode == true)
        {
            amount = 0;
        }
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
    public void setDefFactor(float amount)
    {
        defFactor = amount;
    }
    public bool getisKnockBack()
    {
        return isKnockBack;
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
    public void setSpellCoolDownRate(int amount)
    {
        spellCoolDownRate = amount;
    }
    public void setStunRate(float amount)
    {
        playBlockAnimation = false;
        myAnimator.SetBool("defend", false);
        stunRate = amount;
    }
    public void setBlockAnimation()
    {
        blockCount--;
        if (blockCount <= 0)
        {
            isBlocking = false;
            playBlockAnimation = false;
            myAnimator.SetBool("defend", false);
            return;
        }
        isWalking = false;
        isDoubleTap = false;
        myblockController.animateBlock(blockCount,maxBlockCount);
        myblockTimer = coolDownBlockTimer;
        playBlockAnimation = true;
        myAnimator.SetBool("defend", true);


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
        if (isAI == true)
            return;
        
        CurrentChargingBar += amount;
        chargingBar.fillAmount = CurrentChargingBar;
        
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
	public float getCurrentMana()
	{
		return currentMana;
	}
    public float getCurrentHealth()
    {
        return currentHealth;
    }
	protected void checkCoolDown()
	{
        if(isStun == true)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                
                stunRate = 1;
                stunTimer = coolDownStunRate;
                isStun = false;
                myAnimator.SetBool("stun", isStun);
               
            }
                
        }
       
        if(playBlockAnimation == true)
        {
            myblockTimer -= Time.deltaTime;
            if(myblockTimer <= 0)
            {
                myblockTimer = coolDownBlockTimer;
                playBlockAnimation = false;
                myAnimator.SetBool("defend", false);
            }
        }

        coolDownRangeTimer -= Time.deltaTime;

        if (coolDownRangeTimer <= 0)
        {
            
            canRangeAttack = true;
          
        }
        else
            canRangeAttack = false;



        //if (isMeleeComboCount[0] == true)
        //{
        //    if (isInCombo == false)
        //    {
        //        coolDownMeleeTimer[0] -= Time.deltaTime;//1st melee attack
        //        if (coolDownMeleeTimer[0] <= 0)
        //        {
        //            myAnimator.SetBool("meleeAttack1", false);
        //            canMove = true;
        //            canMeleeAttack = true;
        //            canCombo = false;
        //            isMeleeComboCount[0] = false;
        //            Debug.Log("in here");
        //        }
        //        else
        //            canMeleeAttack = false;
        //    }
        //    else
        //    {
        //        canMeleeAttack = true;
        //    }
        //}

        /* else
         {
             if (isMeleeComboCount[2] == true)//doing final combo
             {
                 //Debug.Log("in melee count 2");
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
     }*/

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
            myAnimator.SetBool("stun", isStun);
            isLose = true;
            myAnimator.SetBool("die", true);
            gameController.isFinish = true;
         //   myGameController.showGameOver(currentHealth,startingHealth, highestComboAchieve,false);
            isPause = false;
         
        }
	}
    public void setisInUltimate(bool inUlti)
    {
        isInUltimate = inUlti;
    }
    public virtual void ShapeDraw(drawShape.shape myshape)
    {
        //empty for overwrite
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
        myaudio.PlayOneShot(attackSound);
        //isAttack = true;
        canMove = false;
        myAnimator.SetTrigger("rangeAttack");
        //myAnimator.SetBool("rangeAttack", true);
        StartCoroutine(WaitForAnimation("range attack",0));
        Debug.Log("in range att");
        
    }
    public virtual void ultimateMove()
    {
     //   Debug.Log("wtf ulti");
    }
    protected void showResult()
    {
        if (isAI == true)
            return;
        if (isInResult == false)
        {
            if (gameController.isFinish == true)
            {
                if (currentHealth <= 0)
                {
                    myGameController.showGameOver(currentHealth, enemy.GetComponent<CharacterBase>().getCurrentHealth(),
                        startingHealth, highestComboAchieve, false);

                }
                if (enemy.GetComponent<CharacterBase>().getCurrentHealth() <= 0)
                {
                    myGameController.showGameOver(currentHealth, enemy.GetComponent<CharacterBase>().getCurrentHealth(),
                        startingHealth, highestComboAchieve, true);

                }
                if (currentHealth >= enemy.GetComponent<CharacterBase>().getCurrentHealth())
                {
                    myGameController.showGameOver(currentHealth, enemy.GetComponent<CharacterBase>().getCurrentHealth(),
                        startingHealth, highestComboAchieve, true);

                }
                else
                {
                    myGameController.showGameOver(currentHealth, enemy.GetComponent<CharacterBase>().getCurrentHealth(),
                        startingHealth, highestComboAchieve, false);

                }
                isInResult = true;
            }

        }
    }
    protected void pause()
    {
        if (isAI == false)
        {
            if (gameController.isFinish == false)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isPause = !isPause;
                  

                    if (isPause == true)
                        stopMoving();
                    else
                        startMoving();

                    myGameController.showPause(isPause);
                }
            }
        }
    }
	protected virtual void Move()
	{
        if (isAI == false)
        {
            if (Input.GetButtonUp("Horizontal"))
            {
                speed = normalSpeed;
                isDoubleTap = false;
                isWalking = false;

            }
        }
      

        if (canMove == false || isInUltimate == true)
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


        if (isAI == false)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if ((Time.time - doubleTapTimer) < tapspeed)
                {

                    isDoubleTap = true;
                    speed = fastSpeed;

                }

                doubleTapTimer = Time.time;
            }
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
           
        }
      
        if (isStun == false && playBlockAnimation == false)
        {
         
            movement.x = horizontal * speed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, rb.velocity.z);
           
        }
			
	}
	protected virtual void crouch()
	{
        if (isAI == true)
            return;
        if (isStun == false)
        {
            if (Input.GetKeyDown("s"))
            {
                highJumpTimer = Time.time;

                if (isInUltimate == false)
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


        if (isAI == false)
        {
            jumping = Input.GetAxisRaw("Jump");
            if (isInUltimate == true)
                jumping = 0;
        }

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
		if(jumping > 0)//player press jump while on the ground
		{
            if (check_touchGround(groundMask, distanceToGround) == true)
            {
                
                if (isStun == false && canRangeAttack == true && canMeleeAttack == true)//isAttack
                {
                    jumpingMovement.y = jumpSpeed;
                    rb.velocity = new Vector3(rb.velocity.x, jumpingMovement.y,
                                                   rb.velocity.z);
                    isJumping = true;
                    isCrouch = false;
                    jumpSpeed = lowJumpSpeed;//reset
                    myaudio.PlayOneShot(jumpedSound);
                }
            }
            else
            {
                jumpingMovement.y = -fallSpeed * Time.deltaTime;
                rb.AddForce(jumpingMovement, ForceMode.Impulse);
                
                //if (transform.position.y <= 0.3)
                //{
                //    Debug.Log("in end jump");
                //    transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
                //}
            }

		}
		else if(jumping == 0)//player reach the ground
		{
            if (check_touchGround(groundMask, distanceToGround) == true)
            {
                jumpingMovement.y = 0;
                isJumping = false;
                jumpSpeed = lowJumpSpeed;
            }
            else
            {
                jumpingMovement.y = -fallSpeed * Time.deltaTime;
                rb.AddForce(jumpingMovement, ForceMode.Impulse);



            }

        }
		//else//player still in the air
		//{
  //          jumpingMovement.y = -fallSpeed;
  //          rb.AddForce(jumpingMovement, ForceMode.Impulse);

  //          if (transform.position.y <= 0.3)
  //          {
  //              //Debug.Log("end air");
  //              transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
  //          }

  //      }

		if (check_touchGround (targetMask,0.6f) == true)//prevent player from standing on top
		{
			Debug.Log("touch");
			rb.AddForce (transform.forward * -speed * 10,ForceMode.Impulse);
		}
		

	}
    protected void cheatCode()
    {
        if (isAI == true)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            isGodMode = !isGodMode;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isUnlimitedSpell = !isUnlimitedSpell;
            if(isUnlimitedSpell == true)
            {
                for(int i=0;i< canCastSpell.Length;i++)
                {
                    canCastSpell[i] = true;
                }
                canCastUltimate = true;
            }
        }
    }
	protected void rangeAttack(Vector3 position, Vector3 direction,int num,float damageMultipler)//gameController.projectileType myType
    {
        //the value num mean 0 for player, 1 for enemy
        GameObject temp = myGameController.getPoolObjectInstance(num).getPoolObject ();
		
		if (temp == null)
			return;
		weaponBase projectile = temp.GetComponent<weaponBase> ();
        if (currentMana < projectile.getConsumeMana())//not enough mana to cast spell
        {
            isNotEnoughMana = true;
            return;
        }
        else
        {
            temp.transform.position = position + direction.normalized;
            temp.SetActive(true);
            projectile.launch(direction);
            projectile.setTag(characterTag);
            projectile.setMultipler(damageMultipler);
            setMana(-projectile.getConsumeMana());
            coolDownRangeTimer = coolDownRangeAttackRate;
            isEndOfRangeAttack = false;
            isDoubleTap = false;
            isNotEnoughMana = false;
        }
    }

	protected void meleeAttack()
	{
        if (isStun == true)
            return;
        if (canMeleeAttack == true || canCombo == true)
        {
            myaudio.PlayOneShot(attackSound);
            isDoubleTap = false;
            canMove = false;
            canMeleeAttack = false;
            mymelee.enabled = true;
            Debug.Log("hit");
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
       
        if (check_touchGround(groundMask, distanceToGround) == false)
        {
            jumpingMovement.y = -fallSpeed * Time.deltaTime;
            rb.AddForce(jumpingMovement, ForceMode.Impulse);


        }

        //myAnimator.SetBool("meleeAttack1", false);
        //myAnimator.SetBool("meleeAttack2", false);
        //myAnimator.SetBool("meleeAttack3", false);

    }
    protected void setAnimation()
    {
        
        myAnimator.SetBool("walk", isWalking);

        myAnimator.SetBool("dash", isDoubleTap);
        myAnimator.SetBool("jump", isJumping);
        myAnimator.SetBool("crouch", isCrouch);

        //myAnimator.SetBool("defend", playBlockAnimation);
        //myAnimator.SetBool("stun", isStun);
      //  myAnimator.SetBool("die", isLose);

       
       
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
            canMove = true;
         
            isEndOfRangeAttack = true;
      

        }
        else if(name == "melee 1")
        {
            if (isMeleeComboCount[1] == true)//isInCombo
            {
                //if in here mean in my 2nd melee combo state
               
       
                Debug.Log("end melee 1");
            }
            else
            {
                //only do 1st melee only or no combo
                canMove = true;
               
                canCombo = false;
                canMeleeAttack = true;
                isMeleeComboCount[0] = false;
               
            }

        }
        else if(name == "melee 2")
        {
            if (isMeleeComboCount[2] == true)
            {
                //if in here mean i in my final melee combo state
               
              //  myAnimator.SetBool("meleeAttack2", false);
                Debug.Log("end melee 2");
            }
            else
            {
             
                canMove = true;
                
                canCombo = false;
                canMeleeAttack = true;
                isMeleeComboCount[0] = false;
                isMeleeComboCount[1] = false;
                

             //   myAnimator.SetBool("meleeAttack1", false);
               // myAnimator.SetBool("meleeAttack2", false);
               
            }
        }
        else if (name == "melee 3")
        {
            if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
            {
                enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);

            }
            Debug.Log("finish combo");
            canMove = true;
            
            canCombo = false;
            canMeleeAttack = true;
            isMeleeComboCount[0] = false;
            isMeleeComboCount[1] = false;
            isMeleeComboCount[2] = false;

            //myAnimator.SetBool("meleeAttack1", false);
            //myAnimator.SetBool("meleeAttack2", false);
            //myAnimator.SetBool("meleeAttack3", false);


        }
        else if(name == "cast ultimate")
        {
            
            canMove = true;
            yield return new WaitForSeconds(5.0f);
            canCastUltimate = true;
        }
       
		yield return null;
	}
    protected void comboAttack()
    {
        if (isEndOfRangeAttack == false)//still doing range attack
            return;
        if (Input.GetKeyDown("o"))//melee attack
        {

            if (isMeleeComboCount[0] == false)//1st attack
            {

                meleeAttack();
                isMeleeComboCount[0] = true;

                myAnimator.SetTrigger("TmeleeAttack1");
                StartCoroutine(WaitForAnimation("melee 1", 0));

            }
            else
            {
                if (canCombo == true)
                {
                    if (isMeleeComboCount[1] == false)//haven do 2nd combo
                    {
                        
                        meleeAttack();

                        isMeleeComboCount[1] = true;
                        myAnimator.SetTrigger("TmeleeAttack2");
                        StartCoroutine(WaitForAnimation("melee 2", 0));


                    }
                    else
                    {
                        if (isMeleeComboCount[2] == false)//haven do final combo
                        {

                            enemy.GetComponent<CharacterBase>().setStunRate(3.5f);
                            meleeAttack();
                            isMeleeComboCount[2] = true;
                            myAnimator.SetTrigger("TmeleeAttack3");

                            myAnimator.SetTrigger("finishCombo");
                            StartCoroutine(WaitForAnimation("melee 3", 0));

                        }
                    }
                }

            }
        }

    }
    protected IEnumerator spellCoolDown(int waitTime, bool[] cd, int num)
    {

        yield return new WaitForSeconds(waitTime);
        cd[num] = true;//cool down finish

    }


}







