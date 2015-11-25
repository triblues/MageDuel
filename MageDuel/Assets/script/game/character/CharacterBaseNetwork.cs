using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterBaseNetwork : NetworkBehaviour
{
  //  public GameObject projectile;
    public LayerMask groundMask;
    public LayerMask targetMask;

    [Header("armor,active,passive")]
    [SerializeField]
    protected int[] spellCastCoolDown;
    [SerializeField]
    protected int[] spellDuration;

    [SerializeField] [SyncVar]
    protected int characterTag;
    [SerializeField]
    protected float startingHealth = 100.0f;
    [SerializeField]
    protected float startingMana = 100.0f;
    [Tooltip("minimum is 0, maximum is 1")]
    [SerializeField] [SyncVar]
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
    protected float coolDownBlockTimer = 1.5f;
    [SerializeField]
    protected float manaRegenRate = 1.0f;
    [SerializeField]
    float distanceToGround = 0.1f;//the amount of dist to stop falling
    [SerializeField]
    protected int maxBlockCount = 5;
    [SerializeField]
    public int ultimateDamage = 30;

    protected GameObject freezeEffect;
    protected GameObject playerArrow;
    protected GameObject enemy;
    protected Transform enemyTrans;
    protected Text comboText;
    protected Animation comboAnimation;//this is the combo text animation,fade in/out
    [SyncVar(hook ="setComboCountNetwork")]
    protected int comboCount;
    protected int blockCount;//for blocking
    protected float jumpSpeed;
    protected float speed;
    protected meleeNetwork mymelee;
    protected float myDamageMultipler;
    protected bool isKnockBack;
    protected float stunTimer;
    protected float myblockTimer;
    protected float stunRate;
    protected float coolDownRangeTimer;
    protected float[] coolDownMeleeTimer;
    [SyncVar]
    protected float currentHealth;
    [SyncVar]
    protected float currentMana;

    protected int highestComboAchieve;

    [SyncVar]
    protected int spellCoolDownRateNetwork;
    protected int spellCoolDownRate;
    protected Vector3 movement;                   // The vector to store the direction of the player's movement.
    protected Vector3 jumpingMovement;
    protected Rigidbody rb;          // Reference to the player's rigidbody.
    protected float horizontal;
    protected float jumping;
    protected bool isInResult;
    protected bool canRangeAttack;
    protected bool canMeleeAttack;
    protected bool isLose;
    protected bool canMove;
    [SyncVar(hook = "setCanCastUltimate")]
    protected bool canCastUltimate;
    [SyncVar]
    protected bool isStun;
    [SyncVar]
    protected bool isDie;
    protected bool isEndOfRangeAttack;
    protected bool isAttack;
    protected bool playBlockAnimation;
    protected bool hasEnterGameOver;

    [SyncVar(hook = "setisFreezeNetwork")]
    protected bool isFreeze;
    [SyncVar]
    protected bool isWalking;
    [SyncVar]
    protected bool isJumping;
    [SyncVar]
    protected bool isCrouch;
    [SyncVar(hook = "setIsDefend")]
    protected bool isDefend;
    protected bool isNotEnoughMana;

    protected bool isBlockLeft;//this determine the direction the character should be going to activate block
    protected bool isBlocking;
    protected bool isCastMode;

    protected bool shouldWaitAnimationFinish;
    protected bool canCombo;//this check to determine if player hit the enemy the 1st time and let them do combo
    protected bool[] isMeleeComboCount;
    protected bool isFinishCombo;
    protected Animator myAnimator;
    
    //protected gameController myGameController;//for all the spawning object pool


    //ultimate camera
    protected ultimateCameraControllerNetwork myUltiCamera;
  
    [SyncVar(hook = "setIsInUltimateNetwork")]
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
    [SyncVar]
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
    protected bool hasEnterFreeze;
    protected NetworkInstanceId mynetworkID;
    protected serverLogic myserverLogic;
    [SyncVar(hook = "setRangeAttackAnimation")]
    bool networkRangeAttack;
    [SyncVar(hook = "setMelee1AttackAnimation")]
    bool networkMelee1Attack;
    [SyncVar(hook = "setMelee2AttackAnimation")]
    bool networkMelee2Attack;
    [SyncVar(hook = "setMelee3AttackAnimation")]
    bool networkMelee3Attack;
    [SyncVar(hook = "setRangeAttackType")]
    int rangeAttackType;
    [SyncVar]
    protected bool isPSActive;
    [SyncVar]
    protected bool isPSArmor;
    [SyncVar]
    protected bool isPSOTU;
    [SyncVar(hook = "setarmorUICD")]
    protected bool armorUICD;
    [SyncVar(hook = "setactiveUICD")]
    protected bool activeUICD;


    protected float ultimateYValue;
    protected float ultimateYAmount;
    protected bool canPlay = false;
    protected int myCharacterType;
    protected List<GameObject> myprojectile;
    protected customNetworkManager mycustomNetworkManager;

    public override void OnStartLocalPlayer()
    {
        

        base.OnStartLocalPlayer();
        getIdentity();
        setCharacterTag();

        if (isLocalPlayer)
        {
            Debug.Log("wtf");
            Debug.Log("the amount: " + mycustomNetworkManager.getmyobj().Count.ToString());
        }
        else
            Debug.Log("false local");
    }
    
   

    //[Client]
    [ClientCallback]
    void getIdentity()
    {
        mynetworkID = GetComponent<NetworkIdentity>().netId;
       
        CmdSendNameToServer(mynetworkID.ToString());
        Debug.Log(mynetworkID.ToString());
    }
    [Command]
    void CmdSendNameToServer(string _name)
    {
        characterTag = int.Parse(_name);
    }
    void setCharacterTag()
    {
        if (isLocalPlayer == false)
        {
            characterTag = int.Parse(mynetworkID.ToString());
        }
        else
        {
            characterTag = int.Parse(mynetworkID.ToString());
        }
    }
    IEnumerator delay(float wait)
    {
        yield return new WaitForSeconds(wait);
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Main Player");
        foreach (GameObject go in temp)
        {

            if (go != transform.gameObject)
            {
                enemy = go;
                break;
            }
        }
        
        enemyTrans = enemy.GetComponent<Transform>();

      
        //if (characterTag % 2 == 0)//player 1
        if (characterTag < enemy.GetComponent<CharacterBaseNetwork>().getCharacterTag())
        {
            healthBar = GameObject.Find("Canvas").transform.Find("player 1/health/outer/inner").GetComponent<Image>();
            manaBar = GameObject.Find("Canvas").transform.Find("player 1/mana/outer/inner").GetComponent<Image>();

            combo = GameObject.Find("Canvas").transform.Find("player 1/combo text").gameObject;
            chargingBar = GameObject.Find("Canvas").transform.Find("player 1/charging bar outer/charging bar inner").
                GetComponent<Image>();

            UIarmorCD = GameObject.Find("Canvas").transform.Find("player 1/armor image outer/armor image inner").GetComponent<UICoolDown>();
            UIActiveCD = GameObject.Find("Canvas").transform.Find("player 1/active image outer/active image inner").GetComponent<UICoolDown>();
            if(isLocalPlayer == true)
                GameObject.Find("Canvas").transform.Find("player 1/arrow").gameObject.SetActive(true);
            Debug.Log("in playe 1");

        }
        else//player 2
        {
            healthBar = GameObject.Find("Canvas").transform.Find("player 2/health/outer/inner").GetComponent<Image>();
            manaBar = GameObject.Find("Canvas").transform.Find("player 2/mana/outer/inner").GetComponent<Image>();

            combo = GameObject.Find("Canvas").transform.Find("player 2/combo text").gameObject;
            chargingBar = GameObject.Find("Canvas").transform.Find("player 2/charging bar outer/charging bar inner").
                GetComponent<Image>();

            UIarmorCD = GameObject.Find("Canvas").transform.Find("player 2/armor image outer/armor image inner").GetComponent<UICoolDown>();
            UIActiveCD = GameObject.Find("Canvas").transform.Find("player 2/active image outer/active image inner").GetComponent<UICoolDown>();
            if (isLocalPlayer == true)
                GameObject.Find("Canvas").transform.Find("player 2/arrow").gameObject.SetActive(true);
            Debug.Log("in playe 2");
        }
        myAnimator = transform.Find("model").GetComponent<Animator>();
        comboText = combo.GetComponent<Text>();
        comboAnimation = combo.GetComponent<Animation>();

        // Setup player attributes
        healthBar.fillAmount = currentHealth / 100;
        manaBar.fillAmount = currentMana / 100;
        comboText.text = "Combo: " + comboCount.ToString();
        chargingBar.fillAmount = CurrentChargingBar;

        
        if (isLocalPlayer)
            Debug.Log("local tag: " + characterTag.ToString());
        else
            Debug.Log("non local tag: " + characterTag.ToString());

        if(isLocalPlayer == true)
        {
            for(int i=0;i< mycustomNetworkManager.getmyobj().Count;i++)
            {
                Debug.Log("name: " + mycustomNetworkManager.getmyobj()[i].name);
            }
        }
        //if (isServer == true)
        //{
            //myprojectile = new List<GameObject>();
            ////  GameObject[] tempproj = GameObject.FindGameObjectsWithTag("fireball");
            //weaponBaseNetwork[] tempproj = (weaponBaseNetwork[])Resources.FindObjectsOfTypeAll<weaponBaseNetwork>();
            //for (int i = 0; i < tempproj.Length-2; i++)
            //{
            //    myprojectile.Add(tempproj[i].gameObject);
            //    Debug.Log(tempproj[i].gameObject.name);
            //}
            //Debug.Log("total projectile" + myprojectile.Count.ToString());
       // }
        canPlay = true;
    }
    // Use this for initialization
    protected virtual void Awake()
    {
        mycustomNetworkManager = GameObject.Find("networkController").GetComponent<customNetworkManager>();
       
        currentHealth = startingHealth;
        currentMana = startingMana;

        playerArrow = transform.Find("arrow parent").gameObject;
        myblockController = transform.Find("block").GetComponent<blockController>();
        myUltiCamera = GameObject.FindGameObjectWithTag("ultimateCamera").GetComponent<ultimateCameraControllerNetwork>();

        mymelee = transform.Find("melee trigger box").GetComponent<meleeNetwork>();
        myaudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        
        isJumping = false;
        hasEnterGameOver = false;
        isAttack = false;
        isCastMode = false;
        playBlockAnimation = false;
        isLose = false;
        canMove = true;
        isWalking = false;
        isInResult = false;
        canCastUltimate = true;
        isPSActive = false;
        isPSArmor = false;
        isPSOTU = false;
        
        armorUICD = false;

         isNotEnoughMana = false;
        isKnockBack = true;
        isEndOfRangeAttack = true;

        isFinishCombo = true;

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
       // comboCount = 0;
        highestComboAchieve = 0;
        blockCount = maxBlockCount;
        stunRate = 1;//default
        spellCoolDownRate = 1;//how fast the spell cooldown, 1 is normal rate
        spellCoolDownRateNetwork = spellCoolDownRate;
        CurrentChargingBar = Mathf.Clamp01(CurrentChargingBar);


        myserverLogic = GameObject.Find("server logic(Clone)").GetComponent<serverLogic>();
      
        coolDownRangeTimer = 0;
        myDamageMultipler = 1;

        coolDownMeleeTimer = new float[2];
        coolDownMeleeTimer[0] = coolDownMeleeAttackRate;

        isMeleeComboCount = new bool[3];
        for (int i = 0; i < isMeleeComboCount.Length; i++)
            isMeleeComboCount[i] = false;//for melee combo animation


        myblockTimer = coolDownBlockTimer;
        stunTimer = coolDownStunRate;
        
        //comboText = combo.GetComponent<Text>();
        //comboAnimation = combo.GetComponent<Animation>();

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

        

    }

    protected virtual void Start()
    {

     
        //if (characterTag % 2 != 0)//player 1
        //{
        //    healthBar = GameObject.Find("Canvas").transform.Find("player 1/health/outer/inner").GetComponent<Image>();
        //    manaBar = GameObject.Find("Canvas").transform.Find("player 1/mana/outer/inner").GetComponent<Image>();

        //    combo = GameObject.Find("Canvas").transform.Find("player 1/combo text").gameObject;
        //    chargingBar = GameObject.Find("Canvas").transform.Find("player 1/charging bar outer/charging bar inner").
        //        GetComponent<Image>();

        //    UIarmorCD = GameObject.Find("Canvas").transform.Find("player 1/armor image outer/armor image inner").GetComponent<UICoolDown>();
        //    UIActiveCD = GameObject.Find("Canvas").transform.Find("player 1/active image outer/active image inner").GetComponent<UICoolDown>();
        //    Debug.Log("in playe 1");

        //}
        //else//player 2
        //{
        //    healthBar = GameObject.Find("Canvas").transform.Find("player 2/health/outer/inner").GetComponent<Image>();
        //    manaBar = GameObject.Find("Canvas").transform.Find("player 2/mana/outer/inner").GetComponent<Image>();

        //    combo = GameObject.Find("Canvas").transform.Find("player 2/combo text").gameObject;
        //    chargingBar = GameObject.Find("Canvas").transform.Find("player 2/charging bar outer/charging bar inner").
        //        GetComponent<Image>();

        //    UIarmorCD = GameObject.Find("Canvas").transform.Find("player 2/armor image outer/armor image inner").GetComponent<UICoolDown>();
        //    UIActiveCD = GameObject.Find("Canvas").transform.Find("player 2/active image outer/active image inner").GetComponent<UICoolDown>();
        //    Debug.Log("in playe 2");
        //}
        //myAnimator = transform.Find("model").GetComponent<Animator>();
        //comboText = combo.GetComponent<Text>();
        //comboAnimation = combo.GetComponent<Animation>();

        //// Setup player attributes
        //healthBar.fillAmount = currentHealth / 100;
        //manaBar.fillAmount = currentMana / 100;
        //comboText.text = "Combo: " + comboCount.ToString();
        //chargingBar.fillAmount = CurrentChargingBar;

        //if (isServer == true)
        //{
        //    myprojectile = new List<GameObject>();

        //    GameObject[] myprojtemp = GameObject.FindGameObjectsWithTag("projectile");
        //    //weaponBaseNetwork[] tempproj = (weaponBaseNetwork[])Resources.FindObjectsOfTypeAll<weaponBaseNetwork>();
        //    foreach (GameObject temp in myprojtemp)
        //    {
        //        myprojectile.Add(temp);
        //        temp.SetActive(false);
        //        Debug.Log("name: " + temp.gameObject.name);

        //    }

        //    Debug.Log("total projectile" + myprojectile.Count.ToString());
        //}
        //  isFreeze = false;

        hasEnterFreeze = false;
        freezeEffect = transform.Find("Enemy Freeze Effect").gameObject;
        if (isLocalPlayer == true)
            playerArrow.SetActive(true);

        StartCoroutine(delay(4.0f));//delay to let the player spawn

      //  StartCoroutine(regenMana(0.5f));
        StartCoroutine(regenBlockCount(2.0f));

       
      
    }
    IEnumerator regenMana(float time)
    {
        while (true)
        {
            if (currentMana < startingMana)
            {
                currentMana += manaRegenRate;
                transmitMana(currentMana);
                //manaBar.fillAmount = currentMana / 100;

            }


            yield return new WaitForSeconds(time);
        }

    }
    IEnumerator regenBlockCount(float time)
    {
        while (true)
        {
            if (blockCount < maxBlockCount)
            {
                blockCount++;
            }
            yield return new WaitForSeconds(time);
        }
    }
    protected virtual void Update()
    {
        
        if (canPlay == false)
            return;
        checkCoolDown();



        if (myserverLogic.getIsFinish() == false)//haven finsih
        {
            Move();
            jump();
            crouch();

            if(isLocalPlayer == true)
            {
                checkfreeze();
                spellCoolDownRate = enemy.GetComponent<CharacterBaseNetwork>().getspellCoolDownRateNetwork();
            }

            if (currentHealth <= 0)
                myserverLogic.setIsFinish(true);
        }
        else
        {
            if (isLocalPlayer == true)
            {
                if (hasEnterGameOver == false)
                {
                    if (currentHealth <= 0)
                    {
                        isLose = true;
                        myAnimator.SetBool("die", true);
                    }
                    else
                    {
                        isLose = false;
                    }
                    myserverLogic.showGameOver(currentHealth, enemy.GetComponent<CharacterBaseNetwork>().getCurrentHealth(),
                        startingHealth, highestComboAchieve, isLose);
                    hasEnterGameOver = true;
                }
                
            }
        }

        if (currentMana <= 0)
            currentMana = 0;

        //if (comboAnimation.IsPlaying("fade") == false)
        //{
        //    if (comboCount > highestComboAchieve)
        //        highestComboAchieve = comboCount;

        //    comboCount = 0;
        //}
        healthBar.fillAmount = currentHealth / 100;
        manaBar.fillAmount = currentMana / 100;
        chargingBar.fillAmount = CurrentChargingBar;
      //  comboText.text = "Combo: " + comboCount.ToString();

    }
    protected void checkfreeze()
    {
        if (isFreeze == true)
        {
            if (hasEnterFreeze == false)
            {
                hasEnterFreeze = true;

                StartCoroutine(freezeDurationTimer(5.0f));
            }
        }
        else
        {
            hasEnterFreeze = false;
        }
    }
    public void TakesDamage(float damage)
    {
        
        //if (isLocalPlayer == false)
          //  return;

        if (damage <= 0)//mean heal
        {
            currentHealth -= damage;
            if (currentHealth >= startingHealth)
                currentHealth = startingHealth;

            if (isLocalPlayer == true)
                transmitHealth(currentHealth);
        }
        else
        {
            Debug.Log("take damage: " + damage.ToString());
            myaudio.PlayOneShot(gotHitSound);

            damage = damage * (1 + (1 - defFactor));
            // Reduce health
          

              currentHealth -= damage;
            //transmitHealth(currentHealth - damage);
            transmitHealth(currentHealth);
          
            stunTimer = coolDownStunRate * stunRate;

            if (isKnockBack == true)
            {
                isStun = true;
                if(isLocalPlayer == true)
                    transmitOtherAnimation(true, false, false);
              
             //   myAnimator.SetBool("stun", isStun);
            }


            checkDead();

        }
      // healthBar.fillAmount = currentHealth / 100;
      
        
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
        currentMana += amount;
        transmitMana(currentMana);
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
    public int getspellCoolDownRateNetwork()
    {
        return spellCoolDownRateNetwork;
    }
    public void setStunRate(float amount)
    {
        playBlockAnimation = false;
        isDefend = false;
        transmitOtherAnimation(false, true, false);
        //myAnimator.SetBool("defend", false);
        stunRate = amount;
    }
    public void setBlockAnimation()
    {
        blockCount--;
        if (blockCount <= 0)
        {

            playBlockAnimation = false;
            isDefend = false;
            transmitOtherAnimation(false, true, false);
          //  myAnimator.SetBool("defend", false);
            return;
        }
        isWalking = false;
        isDoubleTap = false;
        trasmitAnimation(true, false, false);
        transmitDoubleTap(true);

        //myblockController.animateBlock(blockCount, maxBlockCount);
        //myblockTimer = coolDownBlockTimer;
        //playBlockAnimation = true;

        isDefend = true;
        transmitOtherAnimation(false, true, false);
       // myAnimator.SetBool("defend", true);

    }
    public void setComboCount(int amount)
    {
        if (amount <= 0)
            return;

        if (comboAnimation.IsPlaying("fade") == false)
        {
           // if (comboCount > highestComboAchieve)
             //   highestComboAchieve = comboCount;

            comboCount = 0;
        }
        comboCount += amount;
        transmitComboCount(comboCount);
        //comboText.text = "Combo: " + comboCount.ToString();

        //comboAnimation.Play("fade");
        //comboAnimation["fade"].time = 0;
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
    public void setIsFreeze(bool _isFreeze)
    {
        isFreeze = _isFreeze;
        transmitFreeze(isFreeze);
    }
    public bool getIsFreeze()
    {
        return isFreeze;
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
            if (CurrentChargingBar <= 0)
                CurrentChargingBar = 0;
            transmitBar(CurrentChargingBar);
           // chargingBar.fillAmount = CurrentChargingBar;
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

    protected bool shouldTurn(Vector3 myself, Vector3 enemy)
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
        if (isLocalPlayer == false)
            return;
        if (isStun == true)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {

                stunRate = 1;
                stunTimer = coolDownStunRate;
                isStun = false;
               // if (isLocalPlayer)
                    transmitOtherAnimation(true, false, false);
              //  myAnimator.SetBool("stun", isStun);

            }

        }

        if (playBlockAnimation == true)
        {
            myblockTimer -= Time.deltaTime;
            if (myblockTimer <= 0)
            {
                myblockTimer = coolDownBlockTimer;
                playBlockAnimation = false;

                isDefend = false;
                transmitOtherAnimation(false, true, false);
                //myAnimator.SetBool("defend", false);
            }
        }

        coolDownRangeTimer -= Time.deltaTime;

        if (coolDownRangeTimer <= 0)
        {

            canRangeAttack = true;

        }
        else
            canRangeAttack = false;





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
           
                

            /// isStun = false;
            //  if(isLocalPlayer)
            //  transmitOtherAnimation(true, false, false);
            //  myAnimator.SetBool("stun", isStun);
            isLose = true;
            isDie = true;

            if (isLocalPlayer == true)
            {
                myserverLogic.setIsFinish(true);
                transmitHealth(0);
                transmitOtherAnimation(false, false, true);
            }

            myAnimator.SetBool("die", true);
           // gameController.isFinish = true;



        }
    }
    public bool getIsInUltimate()
    {
        return isInUltimate;
    }
    public void setisInUltimate(bool inUlti)
    {
        isInUltimate = inUlti;
        //if(isLocalPlayer == true)
          //  transmitUltimate(false, true);
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
        //  myAnimator.SetTrigger("rangeAttack");
        //myAnimator.SetBool("rangeAttack", true);
        trasmitAttackAnimation(true, false, false, false);
        StartCoroutine(WaitForAnimation("range attack", 0));

    }
    public virtual void ultimateMove()
    {
        //   Debug.Log("wtf ulti");
    }


    protected virtual void Move()
    {
       
        if (isLocalPlayer == true)
        {
            //if (myserverLogic.getisInUltimateServer() == true)
              //  return;
            if (Input.GetButtonUp("Horizontal"))
            {
                speed = normalSpeed;
                isDoubleTap = false;
                isWalking = false;
                transmitDoubleTap(true);
                trasmitAnimation(true, false, false);


            }
        }


        if (canMove == false || myserverLogic.getisInUltimateServer() == true)
        {

            speed = normalSpeed;
            isDoubleTap = false;
            transmitDoubleTap(true);
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            return;
        }

        if (isCrouch == true)
            return;


        if (isLocalPlayer == true)
            horizontal = Input.GetAxisRaw("Horizontal");



        if (isLocalPlayer == true)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if ((Time.time - doubleTapTimer) < tapspeed)
                {

                    isDoubleTap = true;
                    transmitDoubleTap(true);
                    speed = fastSpeed;

                }

                doubleTapTimer = Time.time;
            }
        }

        if (isLocalPlayer == true)
        {
            if (isJumping == false && isDoubleTap == false)
            {
                if (playBlockAnimation == false)
                {
                    if (horizontal >= 1 || horizontal < 0)
                    {

                        isWalking = true;
                        trasmitAnimation(true,false,false);
                       
                    }
                    else//equal to 0
                    {
                        isWalking = false;
                        trasmitAnimation(true, false, false);

                    }
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
        if (isLocalPlayer == false)
            return;
        
        if (isStun == false && canMove == true)
        {
            if (Input.GetKeyDown("s"))
            {
                highJumpTimer = Time.time;

                if (myserverLogic.getisInUltimateServer()  == false)//isInUltimate
                {
                    isCrouch = true;
                    trasmitAnimation(false, false, true);
                }


            }

        }

        if (Input.GetKeyUp("s"))
        {
            isCrouch = false;
            trasmitAnimation(false, false, true);
        }

    }
    protected virtual void jump()
    {
        if (isLocalPlayer == false)
            return;
        
        jumping = Input.GetAxisRaw("Jump");
        if (myserverLogic.getisInUltimateServer()  == true)//isInUltimate
            jumping = 0;


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
        
        if (jumping > 0)//player press jump while on the ground
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
                    trasmitAnimation(false, true, true);

                    jumpSpeed = lowJumpSpeed;//reset
                    myaudio.PlayOneShot(jumpedSound);
                }
            }
            else
            {
                jumpingMovement.y = -fallSpeed * Time.deltaTime;
                rb.AddForce(jumpingMovement, ForceMode.Impulse);

               
            }

        }
        else if (jumping == 0)//player reach the ground
        {
            if (check_touchGround(groundMask, distanceToGround) == true)
            {

                jumpingMovement.y = 0;
                isJumping = false;
                trasmitAnimation(false, true, false);
                jumpSpeed = lowJumpSpeed;
            }
            else
            {
                jumpingMovement.y = -fallSpeed * Time.deltaTime;
                rb.AddForce(jumpingMovement, ForceMode.Impulse);

               
            }

        }

        if (check_touchGround(targetMask, 0.3f) == true)//prevent player from standing on top
        {
            rb.AddForce(transform.forward * -speed * 5, ForceMode.Impulse);
        }


    }
    protected GameObject getPoolObject(string _tag)
    {
        //mycustomNetworkManager.getmyobj
        for (int i = 0; i < mycustomNetworkManager.getmyobj().Count; i++)
        {
            if (mycustomNetworkManager.getmyobj()[i].activeInHierarchy == false)
            {
                if(mycustomNetworkManager.getmyobj()[i].CompareTag(_tag) == true)
                //return myprojectile[i];
                return mycustomNetworkManager.getmyobj()[i];
            }
        }

      
        return null;
    }
    
   [Command]
    protected void CmdrangeAttack(Vector3 position, Vector3 direction,string _tag, float damageMultipler,int myCharTag)//gameController.projectileType myType
    {
      

        //GameObject temp = transform.gameObject; //myGameController.getPoolObjectInstance().getPoolObject();
        GameObject temp = getPoolObject(_tag);

        if (temp == null)
        {
            Debug.Log("is null");
            return;
        }
        Debug.Log("in shoot");
        weaponBaseNetwork projectile = temp.GetComponent<weaponBaseNetwork>();
        if (currentMana < projectile.getConsumeMana())//not enough mana to cast spell
        {
            isNotEnoughMana = true;
            return;
        }
        else
        {
            //  temp.transform.position = position + direction.normalized;
            temp.transform.position = position;
            projectile.launch(direction);
            projectile.setTag(myCharTag);
            projectile.setMultipler(damageMultipler);
            temp.SetActive(true);
            

            setMana(-projectile.getConsumeMana());


            coolDownRangeTimer = coolDownRangeAttackRate;
            isEndOfRangeAttack = false;
            isDoubleTap = false;
            transmitDoubleTap(true);
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


    protected bool check_touchGround(LayerMask mymask, float dist)
    {
        if (Physics.Raycast(transform.position, -transform.up, dist, mymask) == true)//origin,direction,max dist
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
    protected void resetAnimation()
    {
        isWalking = false;
        isDoubleTap = false;
        isJumping = false;
        isCrouch = false;
        isStun = false;
        isDefend = false;
        currentHealth = 0;
        if (isLocalPlayer == true)
        {
            transmitDoubleTap(true);
            trasmitAnimation(true, true, true);
            transmitOtherAnimation(true, false, false);
            
            transmitOtherAnimation(false, true, false);
        }

        myAnimator.SetBool("walk", false);
        myAnimator.SetBool("dash", false);
        myAnimator.SetBool("jump", false);
        myAnimator.SetBool("crouch", false);
        myAnimator.SetBool("defend", false);
        myAnimator.SetBool("stun", false);
        myAnimator.SetBool("die",isDie);


    }
    protected void setAnimation()
    {

        myAnimator.SetBool("walk", isWalking);
        myAnimator.SetBool("stun", isStun);

        myAnimator.SetBool("dash", isDoubleTap);
        myAnimator.SetBool("jump", isJumping);
        myAnimator.SetBool("crouch", isCrouch);



    }
    protected IEnumerator freezeDurationTimer(float _time)
    {
        yield return new WaitForSeconds(_time);
        isFreeze = false;
        transmitFreeze(isFreeze);
    }
    protected IEnumerator WaitForAnimation(string name, int count)
    {
        yield return new WaitForSeconds(0.5f);
        while (myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == true)//the animation is still running
        {

            yield return new WaitForSeconds(0.2f);
            //	yield return null;
        }
        if (name == "range attack")
        {
            canMove = true;

            isEndOfRangeAttack = true;


        }
        else if (name == "melee 1")
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
        else if (name == "melee 2")
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
            if (enemy.GetComponent<CharacterBaseNetwork>().getIsBlocking() == false)
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


        }
        else if (name == "cast ultimate")
        {

            canMove = true;
            yield return new WaitForSeconds(5.0f);
            canCastUltimate = true;
            transmitUltimate(true, false);
        }

        yield return null;
    }
    protected virtual void comboAttack()
    {
        if (isEndOfRangeAttack == false)//still doing range attack
            return;
        if (Input.GetKeyDown("o"))//melee attack
        {

            if (isMeleeComboCount[0] == false)//1st attack
            {

                meleeAttack();
                isMeleeComboCount[0] = true;
                trasmitAttackAnimation(false, true, false, false);

               
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
                        trasmitAttackAnimation(false, false, true, false);
                        //myAnimator.SetTrigger("TmeleeAttack2");
                        StartCoroutine(WaitForAnimation("melee 2", 0));


                    }
                    else
                    {
                        if (isMeleeComboCount[2] == false)//haven do final combo
                        {

                            enemy.GetComponent<CharacterBaseNetwork>().setStunRate(3.5f);
                            meleeAttack();
                            isMeleeComboCount[2] = true;
                            trasmitAttackAnimation(false, false, false, true);
                            //myAnimator.SetTrigger("TmeleeAttack3");

                          //  myAnimator.SetTrigger("finishCombo");
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
    

    [ClientCallback]//command can only be send from local player
    void trasmitAnimation(bool _walk, bool _jump, bool _crouch)
    {
        if(isLocalPlayer == true)
        {
            if(_walk == true)
                CmdSendWalkingToServer(isWalking);
            if (_jump == true)
                CmdSendJumpToServer(isJumping);
            if (_crouch == true)
                CmdSendCrouchToServer(isCrouch);
        }

    }
    [ClientCallback]
    void transmitDoubleTap(bool _isdoubletap)
    {
        if(isLocalPlayer == true)
        {
            if(_isdoubletap == true)
            {
                CmdSendDoubleTapToServer(isDoubleTap);
            }
        }
    }
    
    [ClientCallback]
    void transmitOtherAnimation(bool _stun, bool _defend, bool _die)
    {
        if (isLocalPlayer == true)
        {
            if (_stun == true)
                CmdSendStunToServer(isStun);
            if (_defend == true)
                CmdSendDefendToServer(isDefend);
            if (_die == true)
                CmdSendDieToServer(isDie);
        }
    }
    [ClientCallback]
    protected void transmitParticleSystem(bool _armor, bool _active, bool _OTU, bool _emitArmor,bool _emitActive)
    {
        if (isLocalPlayer == true)
        {
            if (_armor == true)
                CmdSendArmorPSToServer(_emitArmor);
            if (_active == true)
                CmdSendActivePSToServer(_emitActive);
            if (_OTU == true)
                CmdSendOTUPSToServer(true);
        }
    }
   

    [Command]//a command to send to the server
    void CmdSendWalkingToServer(bool _isWalking)//must have Cmd as the start of the name, this function only run in the server
    {
        isWalking = _isWalking;
    }
    [Command]//a command to send to the server
    void CmdSendJumpToServer(bool _isJumping)//must have Cmd as the start of the name, this function only run in the server
    {
        isJumping = _isJumping;
    }
    [Command]//a command to send to the server
    void CmdSendCrouchToServer(bool _isCrouch)//must have Cmd as the start of the name, this function only run in the server
    {
        isCrouch = _isCrouch;
    }
    [Command]//a command to send to the server
    void CmdSendStunToServer(bool _isStun)//must have Cmd as the start of the name, this function only run in the server
    {
        isStun = _isStun;
        
    }
    [Command]//a command to send to the server
    void CmdSendDefendToServer(bool _isDefend)//must have Cmd as the start of the name, this function only run in the server
    {
        isDefend = _isDefend;

    }
    [Command]//a command to send to the server
    void CmdSendDoubleTapToServer(bool _isDoubleTap)//must have Cmd as the start of the name, this function only run in the server
    {
        isDoubleTap = _isDoubleTap;

    }
    [Command]//a command to send to the server
    void CmdSendDieToServer(bool _isDie)//must have Cmd as the start of the name, this function only run in the server
    {
        
        isDie = _isDie;
        
     
    }
    [ClientCallback]
    protected virtual void setCanCastUltimate(bool _canCastUltimate)
    {
        canCastUltimate = _canCastUltimate;
        if(canCastUltimate == false)
        {
            myAnimator.SetTrigger("castUltimate");

            ultimateObj.GetComponent<Transform>().position = new Vector3(enemyTrans.position.x,
                enemyTrans.position.y + ultimateYValue, enemyTrans.position.z);
            ultimateObj.SetActive(true);
        }
    }
    [ClientCallback]
    protected void setComboCountNetwork(int _comboCount)
    {
        comboCount = _comboCount;
        if (comboCount > highestComboAchieve)
            highestComboAchieve = comboCount;
        if (comboCount > 0)
        {
            comboText.text = "Combo: " + comboCount.ToString();

            comboAnimation.Play("fade");
            comboAnimation["fade"].time = 0;
        }

    }
    [ClientCallback]
    void setarmorUICD(bool _armor)
    {
        if(_armor == true)
        {
            spellCastCoolDown[0] = spellCastCoolDown[0] * spellCoolDownRate;
            UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);


            armorUICD = false;
        }
    }
    [ClientCallback]
    void setactiveUICD(bool _active)
    {
        if (_active == true)
        {
            spellCastCoolDown[1] = spellCastCoolDown[1] * spellCoolDownRate;
            UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);

            activeUICD = false;
        }
    }
    [ClientCallback]
    void setisFreezeNetwork(bool _isFreeze)
    {
        isFreeze = _isFreeze;
        if(_isFreeze == true)
        {
            freezeEffect.SetActive(true);
            setSpeed(0.5f);
        }
        else
        {
            freezeEffect.SetActive(false);
            setSpeed(2.0f);
        }
    }
    [ClientCallback]
    void setIsDefend(bool _isdefend)
    {
        if(_isdefend == true)
        {
            myAnimator.SetBool("defend", true);
            myblockController.animateBlock(blockCount, maxBlockCount);
            myblockTimer = coolDownBlockTimer;
            playBlockAnimation = true;
        }
        else
        {
            myAnimator.SetBool("defend", false);
        }
    }

    [ClientCallback]
    void setIsInUltimateNetwork(bool _isInUltimate)
    {
        //get hit by ultimate
        //isInUltimate = _isInUltimate;
        if(_isInUltimate == true)
        {
            myAnimator.SetTrigger("ultimate");
            myUltimatePS.gameObject.transform.position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + ultimateYAmount, enemyTrans.position.z);
         
            myUltimatePS.Play();
        }
        else
        {
            myUltiCamera.removeUltimate();
            myUltimatePS.Stop();
        }
    }
    [ClientCallback]
    void setRangeAttackType(int _type)
    {
        rangeAttackType = _type;
        if(rangeAttackType == 1)//single
        {

        }
        else if(rangeAttackType == 2)//multiple
        {

        }
        rangeAttackType = 0;//reset
    }

    [ClientCallback]
    void setRangeAttackAnimation(bool _networkRangeAttack)//range hook
    {
        networkRangeAttack = _networkRangeAttack;

        if (networkRangeAttack == true)
        {
            myAnimator.SetTrigger("rangeAttack");
            networkRangeAttack = false;
        }
    }
   
    [ClientCallback]
    protected void transmitAttack(int _num)
    {
        if(isLocalPlayer == true)
        {
            CmdSendRangeAttackToServer(_num);
        }
    }
    [ClientCallback]
    protected void transmitHealth(float _num)
    {
        if (isLocalPlayer == true)
        {
            CmdSendHealthToServer(_num);
        }
    }
    [ClientCallback]
    protected void transmitMana(float _num)
    {
        if(isLocalPlayer == true)
        {
            CmdSendManaToServer(_num);
        }
    }
    [ClientCallback]
    protected void transmitComboCount(int _num)
    {
        if(isLocalPlayer == true)
        {
            CmdSendComboCountToServer(_num);
        }
    }
    [ClientCallback]
    protected void transmitBar(float _num)
    {
        if (isLocalPlayer == true)
        {
            CmdSendChargingBarToServer(_num);
        }
    }
    [ClientCallback]
    protected void transmitFreeze(bool _isFreeze)
    {
        if(isLocalPlayer == true)
        {
            CmdSendIsFreezeToServer(_isFreeze);
        }
    }
    
    [ClientCallback]
    protected void transmitUltimate(bool _isPrep,bool _isUltimate)
    {
        if(isLocalPlayer == true)
        {
            if(_isPrep == true)
            {
                CmdSendUltimatePrepToServer(canCastUltimate);
            }
            if(_isUltimate == true)
            {
                CmdSendUltimatePSToServer(isInUltimate);
            }
        }
    }
    [ClientCallback]
    protected void transmitspellCoolDownRate(int amount)
    {
        if(isLocalPlayer == true)
        {
            CmdSendSpellCDRateToServer(amount);
        }
    }
    [ClientCallback]
    protected void transmitUI(bool _armor,bool _active)
    {
        if(isLocalPlayer == true)
        {
            if(_armor == true)
            {
                CmdSendUIArmorToServer(armorUICD);
            }
            if(_active == true)
            {
                CmdSendUIActiveToServer(activeUICD);
            }
        }
    }

    [ClientCallback]//command can only be send from local player
    protected void trasmitAttackAnimation(bool _range, bool _melee1, bool _melee2, bool _melee3)
    {
        if (isLocalPlayer == true)
        {
            if (_range == true)
            {
                CmdSendRangeAttackAnimationToServer(true);
            }
            if (_melee1 == true)
            {
                CmdSendMelee1AttackAnimationToServer(true);
            }
            if (_melee2 == true)
            {
                CmdSendMelee2AttackAnimationToServer(true);
            }
            if (_melee3 == true)
            {
                CmdSendMelee3AttackAnimationToServer(true);
            }
        }
    }
    [ClientCallback]
    void setMelee1AttackAnimation(bool _meleeattack)//melee 1
    {
        networkMelee1Attack = _meleeattack;

        if (networkMelee1Attack == true)
        {
            myAnimator.SetTrigger("TmeleeAttack1");
            networkMelee1Attack = false;
        }
    }
    [ClientCallback]
    void setMelee2AttackAnimation(bool _meleeattack)//melee 2
    {
        networkMelee2Attack = _meleeattack;

        if (networkMelee2Attack == true)
        {
            myAnimator.SetTrigger("TmeleeAttack2");
            networkMelee2Attack = false;
        }
    }
    [ClientCallback]
    void setMelee3AttackAnimation(bool _meleeattack)//melee 3
    {
        networkMelee3Attack = _meleeattack;

        if (networkMelee3Attack == true)
        {
            myAnimator.SetTrigger("TmeleeAttack3");
            myAnimator.SetTrigger("finishCombo");
            networkMelee3Attack = false;
        }
    }
    [Command]//a command to send to the server
    void CmdSendRangeAttackToServer(int _num)//must have Cmd as the start of the name, this function only run in the server
    {
        rangeAttackType = _num;
    }

    [Command]//a command to send to the server
    void CmdSendRangeAttackAnimationToServer(bool _networkRangeAttack)//must have Cmd as the start of the name, this function only run in the server
    {
        networkRangeAttack = _networkRangeAttack;
    }
    [Command]//a command to send to the server
    void CmdSendMelee1AttackAnimationToServer(bool _meleeattack)//must have Cmd as the start of the name, this function only run in the server
    {
        networkMelee1Attack = _meleeattack;
    }
    [Command]//a command to send to the server
    void CmdSendMelee2AttackAnimationToServer(bool _meleeattack)//must have Cmd as the start of the name, this function only run in the server
    {
        networkMelee2Attack = _meleeattack;
    }
    [Command]//a command to send to the server
    void CmdSendMelee3AttackAnimationToServer(bool _meleeattack)//must have Cmd as the start of the name, this function only run in the server
    {
        networkMelee3Attack = _meleeattack;
    }
    [Command]//a command to send to the server
    void CmdSendHealthToServer(float _health)//must have Cmd as the start of the name, this function only run in the server
    {
        currentHealth = _health;
    }
    [Command]
    void CmdSendComboCountToServer(int _comboCount)
    {
        comboCount = _comboCount;
    }
    [Command]//a command to send to the server
    void CmdSendManaToServer(float _mana)//must have Cmd as the start of the name, this function only run in the server
    {
        currentMana = _mana;
    }
    [Command]//a command to send to the server
    void CmdSendChargingBarToServer(float _bar)//must have Cmd as the start of the name, this function only run in the server
    {
        CurrentChargingBar = _bar;
    }
    [Command]//a command to send to the server
    void CmdSendActivePSToServer(bool _isActive)//must have Cmd as the start of the name, this function only run in the server
    {
        isPSActive = _isActive;
     
    }
    [Command]//a command to send to the server
    void CmdSendArmorPSToServer(bool _isArmor)//must have Cmd as the start of the name, this function only run in the server
    {
        isPSArmor = _isArmor;
        
    }
    [Command]//a command to send to the server
    void CmdSendOTUPSToServer(bool _isOTU)//must have Cmd as the start of the name, this function only run in the server
    {
        isPSOTU = _isOTU;
       
    }
    [Command]//a command to send to the server
    void CmdSendUltimatePrepToServer(bool _isPrep)//must have Cmd as the start of the name, this function only run in the server
    {
        canCastUltimate = _isPrep;
     
    }
    [Command]//a command to send to the server
    void CmdSendUltimatePSToServer(bool _isInUltimate)//must have Cmd as the start of the name, this function only run in the server
    {
        isInUltimate = _isInUltimate;

    }
    [Command]//a command to send to the server
    void CmdSendUIArmorToServer(bool _armor)//must have Cmd as the start of the name, this function only run in the server
    {
        armorUICD = _armor;

    }
    [Command]//a command to send to the server
    void CmdSendUIActiveToServer(bool _active)//must have Cmd as the start of the name, this function only run in the server
    {
        activeUICD = _active;

    }
    [Command]//a command to send to the server
    void CmdSendIsFreezeToServer(bool _isFreeze)//must have Cmd as the start of the name, this function only run in the server
    {
        isFreeze = _isFreeze;

    }
    [Command]//a command to send to the server
    void CmdSendSpellCDRateToServer(int amount)//must have Cmd as the start of the name, this function only run in the server
    {
        spellCoolDownRateNetwork = amount;

    }


}







