using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAI : CharacterBase
{
	

	public bool testMode = false;

    int meleeComboCount;
	int randMin = 1;
	int randMax = 100;
	int randomNum;

	//AI state stuff
	[SerializeField] public float idleTime = 1.0f;//the value mean how long the character will be in this state
	[SerializeField] public float attackRangeTime = 2.0f;
    [SerializeField] public float attackMeleeTime = 2.0f;
	[SerializeField] public float blockTime = 1.0f;
	[SerializeField] public float randAttributeTime = 1.5f;
	float idleTimer;
	float attackRangeTimer;
    float attackMeleeTimer;
    float blockTimer;
	float randomAttTimer;

	bool changeState;
	bool isReverseDirection;
    bool inMeleeCombo;//this check to prevent repeating of doing melee combo
	//fuzzy logic stuff
	[Tooltip("the lower aggreesive level the lower chance to attack")]
	public int aggressiveLevel = 1;
	public int cruelLevel = 1;
	public float rangeDistance = 1.0f;
	public float meleeDistance = 0.3f;

	public AIState myAIState;
	public AIAttack myAIStateAttack;


  	public enum AIState
	{
		idle,
		attack

	};
	public enum AIAttack
	{
		melee,
		rangeSingle,
		rangeMultiple
	};
    protected override void Awake()
    {
        healthBar = GameObject.Find("Canvas").transform.Find("enemy/health/outer/inner").GetComponent<Image>();
        manaBar = GameObject.Find("Canvas").transform.Find("enemy/mana/outer/inner").GetComponent<Image>();

        combo = GameObject.Find("Canvas").transform.Find("enemy/combo text").gameObject;
        //chargingBar = GameObject.Find("Canvas").transform.Find("enemy/charging bar outer/charging bar inner").
         //   GetComponent<Image>();

       
        base.Awake();
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        meleeComboCount = 0;
        inMeleeCombo = false;
        changeState = false;
        //myAIState = AIState.attack;
        //myAIStateAttack = AIAttack.rangeSingle;
       
       // isBlocking = true;
        isReverseDirection = false;

		aggressiveLevel = Mathf.Clamp (aggressiveLevel, 1, 6);

        //GameObject[] temp;
        //temp = GameObject.FindGameObjectsWithTag("Main Player");

        //foreach (GameObject a in temp)
        //{
        //    if (a.name.Contains("Clone") == true)
        //        GameObject.Destroy(a);
        //    else
        //        enemy = a;
        //}

        enemy = GameObject.FindGameObjectWithTag("Main Player");

        if (launchScene.isPractice == true)
            testMode = true;

    }

    // Update is called once per frame
    protected override void Update()
    {
        
        if (gameController.isFinish == true)
        {
            Debug.Log("here");
            resetAnimation();
            return;
        }

           

        
        checkTurn();
  //      if (shouldTurn(transform.position,enemy.transform.position) == true)
  //{
  //	rb.rotation = Quaternion.Euler (0, 270, 0);

        //}
        //else
        //{
        //	rb.rotation = Quaternion.Euler (0, 90, 0);

        //}

        if (isInUltimate == true)
            return;

        base.Update();//move and jump
      
        if (testMode)
        {
            horizontal = 0;
            jumping = 0;
        }
        else
        {
           action();

        }
        AI_Agent();

    }
    protected virtual void checkTurn()
    {
        if (shouldTurn(transform.position, enemy.transform.position) == true)
        {
            rb.rotation = Quaternion.Euler(0, 270, 0);

        }
        else
        {
            rb.rotation = Quaternion.Euler(0, 90, 0);

        }
    }
	void AI_Agent()
	{
      
        switch (myAIState)
		{
			case AIState.idle:
				idleState();
				break;
			case AIState.attack:
			{
				
				switch(myAIStateAttack)
				{
					case AIAttack.melee:
                            {
                                meleeAttackState();
                                meleeCombat();
                            }
						break;
					case AIAttack.rangeSingle:
                            {
                                rangeAttackState();
                                rangeCombat();
                            }
						break;
					case AIAttack.rangeMultiple:
                            {
                                rangeAttackState();
                                rangeCombat();
                            }
						break;
					default:
						break;
				}
			}
			break;
			default:
				break;
		}

        if(isBlocking == true && testMode == false)
        {
            blockState();
        }
	}
	void action()
	{
		if(changeState == true)
		{
            Debug.Log("change");
			randomNum = getRandomNum(randMin,randMax);
			if(randomNum >= randMax-(aggressiveLevel * 15))//the lower aggreesive level the lower chance to attack
			{
				int offset;
                myAIState = AIState.attack;
                randomNum = getRandomNum(randMin, randMax);

                if (currentMana <= startingMana / 5)//left 20%
                    offset = 30;//will want to go melee combat as much as possible
                else
                    offset = -30;

                if (randomNum >= randMax / 2 + offset)
                {
                    randomNum = getRandomNum(randMin, randMax);
                    if (randomNum >= randMax / 2)
                        myAIStateAttack = AIAttack.rangeSingle;
                    else
                        myAIStateAttack = AIAttack.rangeMultiple;
                }
                else
                    myAIStateAttack = AIAttack.melee;

                //myAIStateAttack = AIAttack.melee;


            }
			else
			{
				myAIState = AIState.idle;
			}
		

			changeState = false;
		}

        if (myAIState != AIState.idle)
        {
          //  Debug.Log("in not idle");
            randomAttribute();//random move and jump
        }




	}
	void idleState()
	{
    
        idleTimer += Time.deltaTime;
		horizontal = 0;
		jumping = 0;
		isReverseDirection = false;
		if(idleTimer >= idleTime)
		{
			changeState = true;
			idleTimer = 0;//reset
            //Debug.Log("end idle");
		}
	}
	void rangeAttackState()
	{

        attackRangeTimer += Time.deltaTime;
		
		if(attackRangeTimer >= attackRangeTime)
		{
			changeState = true;
            attackRangeTimer = 0;//reset
		}
	}
    void meleeAttackState()
    {
        if (inMeleeCombo == false)
        {
            attackMeleeTimer += Time.deltaTime;

            if (attackMeleeTimer >= attackMeleeTime)
            {
              //  Debug.Log("in here melee");
                changeState = true;
                attackMeleeTimer = 0;//reset
            }
        }
    }
    void blockState()
	{
        if(blockCount <= 0)
        {
            blockTimer = blockTime;
        }
		blockTimer += Time.deltaTime;

        if (blockTimer >= blockTime)
        {
            //changeState = true;
            blockTimer = 0;
            isBlocking = false;
        }
    }
	void rangeCombat()
	{
		if (currentMana <= 0)
			return;
		if (canRangeAttack == false)
			return;
      
        Vector3 direction;

        if (myAIStateAttack == AIAttack.rangeSingle)
        {


            Vector3 offsetPos = transform.position;
            Vector3 offsetPos_enemy = enemy.transform.position;
            offsetPos.y = offsetPos.y + 1;
            offsetPos_enemy.y = offsetPos_enemy.y + 1;
            direction = offsetPos_enemy - offsetPos;

            rangeAttack(offsetPos, direction,1,myDamageMultipler);
            if (isNotEnoughMana == false)
                rangeAttackAnimation();
        }
        else //multiple attack
        {
            for (int i = 1; i <= 3; i++)//3
            {
                Vector3 offsetPos = transform.position;
                Vector3 offsetPos_enemy = enemy.transform.position;
                offsetPos.y = offsetPos.y + 1;
                offsetPos_enemy.y = offsetPos_enemy.y + 1 * i;
                direction = offsetPos_enemy - offsetPos;

                

                rangeAttack(offsetPos, direction,1, myDamageMultipler);
                if (isNotEnoughMana == true)
                {
                    break;
                }
                
            }
            if (isNotEnoughMana == false)
                rangeAttackAnimation();
        }

        if (Vector3.Distance(transform.position,enemy.transform.position) <= rangeDistance)
		{
			if(isReverseDirection == false)
			{
				horizontal = horizontal * -1;//maintain distance from player
				isReverseDirection = true;
			}
		}
		else
			isReverseDirection = false;
	}
	void meleeCombat()
	{
       
        if (Vector3.Distance(transform.position,enemy.transform.position) <= meleeDistance)
		{
          
            horizontal = 0;
            jumping = 0;
            if (inMeleeCombo == false)
            {
                inMeleeCombo = true;
                myAnimator.SetTrigger("TmeleeAttack1");
                myAnimator.SetTrigger("TmeleeAttack2");
                myAnimator.SetTrigger("TmeleeAttack3");
                myAnimator.SetTrigger("finishCombo");
                StartCoroutine(meleeComboSequence(0.2f));
            }

          
        }
        else//AI is far away from player 
		{
          //  Debug.Log("in far dist");
            if (isJumping == false)//when on ground
			{
				if(transform.position.x > enemy.transform.position.x)//at right side
				{
					horizontal = -1;
				}
				else
				{
					horizontal = 1;
				}
			}


		}
	}


	void randomAttribute()//random move and jump
	{
		randomAttTimer += Time.deltaTime;
		if(randomAttTimer >= randAttributeTime)//random movement every few second
		{
			if(myAIStateAttack != AIAttack.melee)//enemy will go directly to the player
			{
                randomNum = getRandomNum(randMin, randMax);
                if (randomNum >= randMax / 2)
                    horizontal = -1;
                else
                    horizontal = 1;
            }


            randomNum = getRandomNum(randMin, randMax);
            if (randomNum >= randMax / 2)
                jumping = 1;
            else
                jumping = 0;


            if (isJumping == true)
                jumping = 0;//prevent keep jumping


            if (isBlocking == false)
            {
                if (blockCount <= 0)
                {
                    isBlocking = false;
                }
                else
                {
                    randomNum = getRandomNum(randMin, randMax);
                    if (currentHealth > startingHealth / 2)//more then half health
                    {
                        if (randomNum >= randMax / 2)
                        {
                            isBlocking = true;
                        }
                        else
                            isBlocking = false;
                    }
                    else//lower then half health
                    {
                        if (randomNum >= randMax / 2 - 30)//higher chance to block
                        {
                            isBlocking = true;
                        }
                        else
                            isBlocking = false;
                    }
                }
               
            }


            randomAttTimer = 0;
            Debug.Log("in rand");
		}

      //  isBlocking = true;
    }

    IEnumerator meleeComboSequence(float wait)
    {
        while(true)
        {
            
            meleeAttack();
            meleeComboCount++;
            if (meleeComboCount >= 3)//reach max combo
            {
                if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
                {
                    Debug.Log("last hit");
                    enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);
                   
                }
                meleeComboCount = 0;

                attackMeleeTimer = 0;//reset
                inMeleeCombo = false;
                myAIState = AIState.idle;
                idleTimer = 0;
              
                canMove = true;
                break;
            }
               

            yield return new WaitForSeconds(wait);
        }
    }

}












