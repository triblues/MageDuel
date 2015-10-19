/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{

    public LayerMask groundMask;
    public LayerMask targetMask;


    [SerializeField]
    bool isAI = false;
    [SerializeField]
    protected int characterTag;
    [SerializeField]
    protected float startingHealth = 100.0f;
    [SerializeField]
    protected float startingMana = 100.0f;
    [Tooltip("minimum is 0, maximum is 1")]
    [SerializeField]
    protected float CurrentChargingBar = 0;

    [SerializeField]
    protected float normalSpeed = 10.0f;
    [SerializeField]
    protected float fastSpeed = 20.0f;
    [SerializeField]
    protected float offFactor = 1.0f;//offensive power
    [SerializeField]
    protected float defFactor = 1.0f;//defensive power 
    [SerializeField]
    protected float lowJumpSpeed = 10.0f;
    [SerializeField]
    protected float highJumpSpeed = 30.0f;
    [SerializeField]
    protected float fallSpeed = 0.5f;
    [SerializeField]
    protected float coolDownRangeAttackRate = 0.5f;
    [SerializeField]
    protected float coolDownMeleeAttackRate = 0.5f;
    [SerializeField]
    protected float coolDownStunRate = 0.5f;
    [SerializeField]
    protected float coolDownMoveRate = 0.2f;
    [SerializeField]
    protected float coolDownBlockTimer = 0.5f;
    [SerializeField]
    protected float manaRegenRate = 1.0f;
    [SerializeField]
    float distanceToGround = 0.1f;//the amount of dist to stop falling

    protected GameObject enemy;
    protected Text comboText;
    protected Animation comboAnimation;//this is the combo text animation,fade in/out
    protected int comboCount;
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
    protected bool isAttack;
    protected bool playBlockAnimation;
    protected bool isWalking;
    protected bool isCrouch;
    //protected bool isFinish;
    protected bool isBlockLeft;//this determine the direction the character should be going to activate block
    protected bool isBlocking;
    protected bool isCastMode;
    protected bool shouldWaitAnimationFinish;
    protected bool canCombo;//this check to determine if player hit the enemy the 1st time and let them do combo
    protected bool[] isMeleeComboCount;
    protected bool isFinishCombo;
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

    //audio
    public AudioClip gotHit;

    //networking stuff
    //  protected NetworkInstanceId mynetworkID;


    // Use this for initialization
    protected virtual void Awake()
    {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;


        mymelee = transform.Find("melee trigger box").GetComponent<melee>();

        rb = GetComponent<Rigidbody>();
        isJumping = false;
        isAttack = false;
        isCastMode = false;
        playBlockAnimation = false;
        isLose = false;
        isWalking = false;
        // isFinish = false;
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
        stunRate = 1;//default
        CurrentChargingBar = Mathf.Clamp01(CurrentChargingBar);


        myGameController = GameObject.Find("gameManager").GetComponent<gameController>();
        coolDownRangeTimer = coolDownRangeAttackRate;

        coolDownMeleeTimer = new float[2];
        coolDownMeleeTimer[0] = coolDownMeleeAttackRate;

        isMeleeComboCount = new bool[3];
        for (int i = 0; i < isMeleeComboCount.Length; i++)
            isMeleeComboCount[i] = false;//for melee combo animation

        canMoveTimer = coolDownMoveRate;
        myblockTimer = coolDownBlockTimer;
        stunTimer = coolDownStunRate;
        comboText = combo.GetComponent<Text>();
        comboAnimation = combo.GetComponent<Animation>();

        StartCoroutine(regenMana(0.5f));

    }

    protected virtual void Start()
    {
        healthText.text = "Health: " + currentHealth.ToString();
        manaText.text = "Mana: " + currentMana.ToString();
        comboText.text = "Combo: " + comboCount.ToString();
        chargingBar.fillAmount = CurrentChargingBar;
        if (isAI == false)
            myAnimator = GetComponent<Animator>();
    }
    IEnumerator regenMana(float time)
    {
        while (true)
        {
            if (currentMana < startingMana)
            {
                currentMana += manaRegenRate;
                manaText.text = "Mana: " + currentMana.ToString();
            }


            yield return new WaitForSeconds(time);
        }

    }
    protected virtual void Update()
    {

        checkCoolDown();
        checkComboAnimation();
        Move();
        jump();
        crouch();
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
        damage = damage * (1 + (1 - defFactor));
        // Reduce health
        currentHealth -= damage;

        stunTimer = coolDownStunRate * stunRate;
        isStun = true;
        AudioSource.PlayClipAtPoint(gotHit, new Vector3(5, 1, 2));
        checkDead();
        healthText.text = "Health: " + currentHealth.ToString();


    }
    public void setMana(float amount)
    {
        currentMana += amount;
        manaText.text = "Mana: " + currentMana.ToString();
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
        Debug.Log("in block");
        playBlockAnimation = true;

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
    //public void setisFinish(bool finish)
    //{
    //    isFinish = finish;
    //}
    protected bool shouldTurn(Vector3 myself, Vector3 enemy)
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
        if (isStun == true)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                stunTimer = coolDownStunRate;
                isStun = false;
            }

        }
        if (isAttack == true)
        {
            canMoveTimer -= Time.deltaTime;
            if (canMoveTimer <= 0)
            {
                canMoveTimer = coolDownMoveRate;
                isAttack = false;
            }
        }

        if (playBlockAnimation == true)
        {
            myblockTimer -= Time.deltaTime;
            if (myblockTimer <= 0)
            {
                Debug.Log("in here liao");
                myblockTimer = coolDownBlockTimer;
                playBlockAnimation = false;
            }
        }

        coolDownRangeTimer -= Time.deltaTime;

        if (coolDownRangeTimer <= 0)
            canRangeAttack = true;
        else
            canRangeAttack = false;

        if (canCombo == false)
        {

            coolDownMeleeTimer[0] -= Time.deltaTime;//1st melee attack
            if (coolDownMeleeTimer[0] <= 0)
            {
                isMeleeComboCount[0] = false;
                canMeleeAttack = true;
            }
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
        myAnimator.SetBool("rangeAttack", true);
        StartCoroutine(WaitForAnimation("rangeAttack", 0));
    }

    protected virtual void Move()
    {
        if (isAttack == true)
            return;
        if (isCrouch == true)
            return;

        if (isAI == false)
            horizontal = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Horizontal"))
        {
            if ((Time.time - doubleTapTimer) < tapspeed)
            {


                isWalking = false;
                isDoubleTap = true;
                speed = fastSpeed;


            }

            doubleTapTimer = Time.time;
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            speed = normalSpeed;
            isDoubleTap = false;
            isWalking = false;

        }


        if (isJumping == false && isDoubleTap == false)
        {
            if (playBlockAnimation == false)
            {
                if (horizontal >= 1 || horizontal < 0)
                {
                    Debug.Log("abc");
                    isWalking = true;
                }
                else//equal to 0
                    isWalking = false;
            }
            else
            {

                isWalking = false;
            }


        }
        if (isStun == false)
        {
            if (playBlockAnimation == false)
            {
                movement.x = horizontal * speed;
                rb.velocity = new Vector3(movement.x, rb.velocity.y, rb.velocity.z);
            }
        }

    }
    protected virtual void crouch()
    {
        if (isAttack == true)
            return;
        if (isStun == false)
        {
            if (Input.GetKeyDown("s"))
            {
                highJumpTimer = Time.time;
                isCrouch = true;
                //  myAnimator.SetBool("crouch", true);

            }
            else if (Input.GetKeyUp("s"))
            {
                isCrouch = false;
                //  myAnimator.SetBool("crouch", false);
            }
        }

    }
    protected virtual void jump()
    {
        if (isAttack == true)
            return;

        if (isAI == false)
            jumping = Input.GetAxisRaw("Jump");

        if (Input.GetButtonDown("Jump"))
        {
            if (isStun == false)
            {
                if (Time.time - highJumpTimer < tapspeed)
                {
                    jumpSpeed = highJumpSpeed;
                }
            }
        }
        if (jumping > 0 && check_touchGround(groundMask) == true)//player press jump while on the ground
        {
            // Debug.Log("here jump");
            if (isStun == false)
            {
                jumpingMovement.y = jumpSpeed;
                rb.velocity = new Vector3(rb.velocity.x, jumpingMovement.y,
                                               rb.velocity.z);
                isJumping = true;
                isCrouch = false;
            }
            // myAnimator.SetBool("crouch", false);//reset crouch
            // myAnimator.SetBool("jump", true);

        }
        else if (jumping == 0 && check_touchGround(groundMask) == true)//player reach the ground
        {
            //   Debug.Log("done jump");
            jumpingMovement.y = 0;
            isJumping = false;
            jumpSpeed = lowJumpSpeed;
            // myAnimator.SetBool("jump", false);

        }
        else//player still in the air
        {
            //Debug.Log("in air");
            jumpingMovement.y = -fallSpeed;
            rb.AddForce(jumpingMovement, ForceMode.Acceleration);
            if (transform.position.y < 0)
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        }

        if (check_touchGround(targetMask) == true)//prevent player from standing on top
        {
            Debug.Log("touch");
            rb.AddForce(transform.forward * -speed * 2, ForceMode.VelocityChange);
        }


    }
    protected void rangeAttack(Vector3 position, Vector3 direction, gameController.projectileType myType)
    {
        isAttack = true;
        GameObject temp = myGameController.getPoolObjectInstance(myType).getPoolObject();

        if (temp == null)
            return;
        weaponBase projectile = temp.GetComponent<weaponBase>();
        if (currentMana < projectile.getConsumeMana())//not enough mana to cast spell
            return;

        temp.transform.position = position + direction.normalized;
        temp.SetActive(true);
        projectile.launch(direction);
        projectile.setTag(characterTag);
        setMana(-projectile.getConsumeMana());
        coolDownRangeTimer = coolDownRangeAttackRate;
    }

    protected void meleeAttack()
    {
        isAttack = true;
        if (canMeleeAttack == true)
        {

            mymelee.enabled = true;

            if (canCombo == false)//this mean player is in the 1st melee attack
            {

                isMeleeComboCount[0] = true;
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
            }





        }

    }
    protected void checkComboAnimation()
    {
        if (isMeleeComboCount[2] == true)//reaches final combo
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("melee 1") == false &&
               myAnimator.GetCurrentAnimatorStateInfo(0).IsName("melee 2") == false)//play finish
            {

                if (isInComboAnimation == false)//false by default
                {
                    Debug.Log("zero");
                    isFinishCombo = false;//true by default
                    StartCoroutine(WaitForAnimation("melee 3", 0));
                    isInComboAnimation = true;//prevent keep calling
                }
            }



        }
        else if (isMeleeComboCount[1] == true)
        {
            if (canCombo == true)
            {
                if (Time.time - coolDownMeleeTimer[1] >= coolDownMeleeAttackRate)
                {
                    Debug.Log("here 1");
                    isMeleeComboCount[1] = false;
                    isMeleeComboCount[0] = false;
                    canCombo = false;
                    canMeleeAttack = true;
                    coolDownMeleeTimer[0] = 0;
                }
            }
        }
        else if (isMeleeComboCount[0] == true)
        {
            if (canCombo == true)
            {
                if (Time.time - coolDownMeleeTimer[0] >= coolDownMeleeAttackRate)
                {
                    Debug.Log("here 2");
                    isMeleeComboCount[0] = false;
                    canCombo = false;
                    canMeleeAttack = true;
                    coolDownMeleeTimer[0] = 0;
                }
            }
        }
        //else if(isMeleeComboCount[1] == true)
        //{
        //    if (Time.time - coolDownMeleeTimer[1] >= coolDownMeleeAttackRate)
        //    {
        //        Debug.Log("in");
        //        isMeleeComboCount[0] = false;
        //        isMeleeComboCount[1] = false;
        //    }
        //    //    if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("melee 2") == false)//play finish
        //    //{
        //    //    isMeleeComboCount[0] = false;
        //    //    isMeleeComboCount[1] = false;
        //    // //   StartCoroutine(WaitForAnimation("melee 2", 0));

        //    //}
        //}
        //else if(isMeleeComboCount[0] == true)
        //{
        //    if (canCombo == true)
        //    {
        //        if (Time.time - coolDownMeleeTimer[0] >= coolDownMeleeAttackRate)
        //        {
        //            isMeleeComboCount[0] = false;
        //        }
        //    }
        //}

    }
    protected bool check_touchGround(LayerMask mymask)
    {
        //transform.position +
        if (Physics.Raycast(transform.position, -transform.up, distanceToGround, mymask) == true)//origin,direction,max dist
        {
            //Debug.Log("touch ground");
            return true;//player touch the ground
        }
        else
            return false;
    }
    public int getRandomNum(int min, int max)
    {
        int rand = Random.Range(min, max);
        return rand;
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

        myAnimator.SetBool("meleeAttack1", isMeleeComboCount[0]);
        myAnimator.SetBool("meleeAttack2", isMeleeComboCount[1]);
        myAnimator.SetBool("meleeAttack3", isMeleeComboCount[2]);
        myAnimator.SetBool("finishCombo", !isFinishCombo);
    }
    protected IEnumerator WaitForAnimation(string name, int count)
    {
        yield return new WaitForSeconds(0.05f);
        while (myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == true)//the animation is still running
        {
            //    Debug.Log("in loop");
            yield return null;
        }
        if (name == "rangeAttack")
            myAnimator.SetBool(name, false);
        else if (name == "melee 3")
        {

            if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
            {
                //stunTimer = coolDownStunRate * 3;
                enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 30, ForceMode.Impulse);



            }
            Debug.Log("finish combo");
            canCombo = false;
            canMeleeAttack = true;
            isInComboAnimation = false;
            coolDownMeleeTimer[0] = 0;
            coolDownMeleeTimer[1] = 0;
            isFinishCombo = true;

            for (int i = 0; i < isMeleeComboCount.Length; i++)
                isMeleeComboCount[i] = false;//reset
        }

        yield return null;
    }



}







