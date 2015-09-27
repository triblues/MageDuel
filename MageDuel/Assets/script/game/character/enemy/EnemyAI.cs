using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAI : CharacterBase
{

//    public Transform target;
////    public int moveSpeed;
////    public int rotationSpeed;
////    public int maxDistance;
////    public int jumpSpeed = 100;
//    private Transform myTransform;
//    public int dodgeRate;
//    public bool dodge = false;
//
//    Rigidbody rigid;
//    Vector3 movement;
//   // public PlayerShooting player;
//    public float smooth = 2.0F;
//    public float tiltAngle = 30.0F;
//    private Quaternion targetRotation;
//    private Vector3 targetAngles;

	public bool testMode = false;
	public Text healthText;
	public Text manaText;
	public GameObject player;
	public float moveRate = 1.0f;
	public float attackRate = 1.0f;
	public float idleRate = 1.0f;
	public float jumpRate = 1.0f;
	public int aggressiveLevel = 1;
	public int cruelLevel = 1;
    public int maxDistance;


    float moveTimer;
	float attackTimer;
	float idleTimer;
	float jumpTimer;
	bool canChangeState;
	bool isReverse;//is the ai moving forward or backward
	bool hasRandom;
	int randomNum;
	int idleCount;
	public AIState myAIState;
    public float healthBarLength;


    public enum AIState
	{
//		move,
		jump,
		attack,
		idle//this state mean to reset

	};

    // Use this for initialization
    void Start()
    {
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();
        
        canChangeState = false;
        player = GameObject.FindGameObjectWithTag("Player");
//        rigid = GetComponent<Rigidbody>();
//        target = go.transform;
		isReverse = false;
		myAIState = AIState.idle;
		hasRandom = false;
		idleCount = 0;
    }

   

    // Update is called once per frame
    void Update()
    {
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();
       
//        float h = Input.GetAxisRaw("Horizontal");
//        // Set the movement vector based on the axis input.
//        movement.Set(h, 0f, 0f);
//        Debug.DrawLine(target.transform.position, myTransform.position, Color.yellow);
//        myTransform.LookAt(target);
//        //look at target
//        // myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
//
//        //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
//
//
//        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
//        {
//
//            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
//
//        }
//
//        if (Vector3.Distance(target.position, myTransform.position) < 2)
//        {
//            myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime;
//
//        }
//
//        if (Vector3.Distance(target.position, myTransform.position) < 5)
//        {
//            if (Input.GetButtonDown("Jump"))
//            {
//                rigid.AddForce(transform.up * jumpPower);
//            }
//        }

        if (shouldTurn(transform.position,player.transform.position) == true)
		{
			rb.rotation = Quaternion.Euler (0, 270, 0);
			//transform.localRotation = Quaternion.Euler (0, 270, 0);
			//turnOffset = 1;
		}
		else
		{
			rb.rotation = Quaternion.Euler (0, 90, 0);
			//transform.localRotation = Quaternion.Euler (0, 90, 0);
			//turnOffset = -1;
		}
		if (testMode == true)
			return;
		checkCoolDown ();
		stateMethod ();
		state ();
		base.Update ();
    }
	void stateMethod()
	{
		if(canChangeState == true)
		{
			randomNum = getRandomNum(1,101);
			if(randomNum >= 50 - idleCount * 10)
			{
				myAIState = AIState.attack;
				idleCount = 0;
			}
			else
				myAIState = AIState.idle;

			canChangeState = false;
			hasRandom = false;
		}
	}

	void state()
	{
		switch(myAIState)
		{
		case AIState.idle:
			idleState();
			break;
		case AIState.attack:
			attackState();
			break;
		default:
			break;
		}

	
	}

	void attackState()
	{
		shootFireBall ();
        meleeAttack();
		attackTimer += Time.deltaTime;
		if(attackTimer > attackRate)
		{
			attackTimer = 0;
			canChangeState = true;
			moveState();
			jumpState();
		}
	}
	void idleState()
	{
//		horizontal = 0;
//		jumping = 0;
		hasRandom = false;

		//canChangeState = false;
		idleTimer += Time.deltaTime;
		if(idleTimer > idleRate)
		{
			idleCount++;
			idleTimer = 0;
			canChangeState = true;

			moveState();
			jumpState();

		}
	}
	void moveState()
	{
		randomNum = Random.Range (1, 101);
		if (randomNum <= 33)
			horizontal = 1;
		else if (randomNum <= 66)
			horizontal = -1;
		else
			horizontal = 0;

//		if(isReverse == false)//move toward player
//		{
//			horizontal = 1;
////			if(shouldTurn(transform.position,player.transform.position) == true)
////			{
////				horizontal = 1;
////			}
////			else
////			{
////				horizontal = -1;
////			}
//		}
//		else//move away from player
//		{
//			horizontal = -1;
//		}
//		moveTimer += Time.deltaTime;
//		if(moveTimer > moveRate)
//		{
//			moveTimer = 0;
//			canChangeState = true;
//		}
	}
	void jumpState()
	{
		randomNum = getRandomNum(1,101);

		if(randomNum >= 50)
			jumping = 1;
		else
			jumping = 0;

//		if(isJumping == false)//char on ground
//		{
//			jumping = 1;
//		}
//		else
//			jumping = 0;
//		jumpTimer += Time.deltaTime;
//		if(jumpTimer > jumpRate)
//		{
//			jumpTimer = 0;
//			canChangeState = true;
//		}
	}


	void shootFireBall()
	{
		if (getCurrentMana () <= 0)
			return;
		if (canAttack == false)
			return;
		GameObject temp = myGameController.getPoolObjectInstance("fireball").getPoolObject ();
		
		if (temp == null)
			return;
		Vector3 direction = player.transform.position - transform.position;
		fireball projectile = temp.GetComponent<fireball> ();
		temp.transform.position = transform.position + direction.normalized;
		temp.SetActive (true);
		projectile.launch (direction);
		projectile.setTag (characterTag);
		setMana (-projectile.getConsumeMana ());
		coolDownTimer = coolDownAttackRate;
		
		
	}

    //No mana needed for this attack
    void meleeAttack()
    {

        
        if (canAttack == false)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < 2.0f)
        {
            GameObject temp = myGameController.getPoolObjectInstance("fireball").getPoolObject();

            if (temp == null)
                return;
            Vector3 direction = player.transform.position - transform.position;
            fireball projectile = temp.GetComponent<fireball>();
            temp.transform.position = transform.position + direction.normalized;
            temp.SetActive(true);
            projectile.launch(direction);
            projectile.setTag(characterTag);
            //setMana(-projectile.getConsumeMana());
            coolDownTimer = coolDownAttackRate;

        }
        


    }


}












