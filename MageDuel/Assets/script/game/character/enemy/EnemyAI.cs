using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAI : CharacterBase
{
	

	public bool testMode = false;


	int randMin = 1;
	int randMax = 100;
	int randomNum;

	//AI state stuff
	[SerializeField] public float idleTime = 1.0f;//the value mean how long the character will be in this state
	[SerializeField] public float attackTime = 2.0f;
	[SerializeField] public float blockTime = 1.0f;
	[SerializeField] public float randAttributeTime = 1.5f;
	float idleTimer;
	float attackTimer;
	float blockTimer;
	float randomAttTimer;

	bool changeState;
	bool isReverseDirection;
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

    // Use this for initialization
    void Start()
    {


		changeState = false;
		myAIState = AIState.idle;
	//	myAIStateAttack = AIAttack.melee;

		isReverseDirection = false;

		aggressiveLevel = Mathf.Clamp (aggressiveLevel, 1, 6);

    }

    // Update is called once per frame
    void Update()
    {
	


		if(shouldTurn(transform.position,enemy.transform.position) == true)
		{
			rb.rotation = Quaternion.Euler (0, 270, 0);

		}
		else
		{
			rb.rotation = Quaternion.Euler (0, 90, 0);

		}
		if (testMode)
			return;

		base.Update ();//move and jump
		action ();
		AI_Agent ();
	
    }

	void AI_Agent()
	{
		switch(myAIState)
		{
			case AIState.idle:
				idleState();
				break;
			case AIState.attack:
			{
				
				attackState();
				
				switch(myAIStateAttack)
				{
					case AIAttack.melee:
						meleeCombat();
						break;
					case AIAttack.rangeSingle:
						rangeCombat();
						break;
					case AIAttack.rangeMultiple:
						rangeCombat();
						break;
					default:
						break;
				}
			}
			break;
			default:
				break;
		}

        if(isBlocking == true)
        {
            blockState();
        }
	}
	void action()
	{
		if(changeState == true)
		{
			randomNum = getRandomNum(randMin,randMax);
			if(randomNum >= randMax-(aggressiveLevel * 15))//the lower aggreesive level the lower chance to attack
			{
				int offset;
                //myAIState = AIState.attack;
                //randomNum = getRandomNum(randMin, randMax);

                //if (currentMana <= startingMana / 5)//left 20%
                //    offset = 30;//will want to go melee combat as much as possible
                //else
                //    offset = -30;

                //if (randomNum >= randMax / 2 + offset)
                //{
                //    randomNum = getRandomNum(randMin, randMax);
                //    if (randomNum >= randMax / 2)
                //        myAIStateAttack = AIAttack.rangeSingle;
                //    else
                //        myAIStateAttack = AIAttack.rangeMultiple;
                //}
                //else
                //    myAIStateAttack = AIAttack.melee;
                //myAIStateAttack = AIAttack.melee;
                isBlocking = true;

            }
			else
			{
				myAIState = AIState.idle;
			}
		

			changeState = false;
		}

		randomAttribute();//random move and jump




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
		}
	}
	void attackState()
	{

		attackTimer += Time.deltaTime;
		
		if(attackTimer >= attackTime)
		{
			changeState = true;
			attackTimer = 0;//reset
		}
	}
	void blockState()
	{
		blockTimer += Time.deltaTime;

        if (blockTimer >= blockTime)
        {
            changeState = true;
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
            direction = enemy.transform.position - transform.position;
            rangeAttack(transform.position, direction, gameController.projectileType.fireball);
        }
        else
        {
            for (int i = 0; i < 3; i++)//3
            {
                Vector3 newPos = new Vector3(enemy.transform.position.x,
                                             enemy.transform.position.y, enemy.transform.position.z);
                newPos.y = newPos.y + i * 1.5f;
                direction = newPos - transform.position;

                rangeAttack(transform.position, direction, gameController.projectileType.fireball);

            }
        }

		if(Vector3.Distance(transform.position,enemy.transform.position) <= rangeDistance)
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

		if(Vector3.Distance(transform.position,enemy.transform.position) <= meleeDistance)
		{
			horizontal = 0;
            jumping = 0;
            meleeAttack();

        }
		else//AI is far away from player 
		{
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

//	void shootFireBall()
//	{
//		if (getCurrentMana () <= 0)
//			return;
//		if (canRangeAttack == false)
//			return;
//		GameObject temp = myGameController.getPoolObjectInstance("fireball").getPoolObject ();
//		
//		if (temp == null)
//			return;
//		Vector3 direction = player.transform.position - transform.position;
//		fireball projectile = temp.GetComponent<fireball> ();
//		if (currentMana < projectile.getConsumeMana ())//not enough mana to cast spell
//			return;
//
//		temp.transform.position = transform.position + direction.normalized;
//		temp.SetActive (true);
//		projectile.launch (direction);
//		projectile.setTag (characterTag);
//		setMana (-projectile.getConsumeMana ());
//		coolDownRangeTimer = coolDownRangeAttackRate;
//		
//		
//	}
	void randomAttribute()//random move and jump
	{
		randomAttTimer += Time.deltaTime;
		if(randomAttTimer >= randAttributeTime)//random movement every few second
		{
			if(myAIStateAttack != AIAttack.melee)//enemy will go directly to the player
			{
				randomNum = getRandomNum (randMin, randMax);
				if (randomNum >= randMax / 2)
					horizontal = -1;
				else
					horizontal = 1;
			}


//			randomNum = getRandomNum (randMin, randMax);
//			if (randomNum >= randMax / 2)
//				jumping = 1;
//			else
//				jumping = 0;

			jumping = 1;
			if(isJumping == true)
				jumping = 0;//prevent keep jumping
			

			randomAttTimer = 0;
		}


	}

}












