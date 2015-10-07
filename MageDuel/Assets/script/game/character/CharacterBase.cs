/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour {

	public LayerMask groundMask;
	public LayerMask targetMask;
	public Text healthText;
	public Text manaText;
	public Image chargingBar;
	[SerializeField] public GameObject enemy;
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
	[SerializeField] protected float manaRegenRate = 1.0f;
	[SerializeField] float distanceToGround = 0.1f;//the amount of dist to stop falling

	protected float jumpSpeed;
	protected float speed;
	protected melee mymelee;
	protected float coolDownRangeTimer;
	protected float coolDownMeleeTimer;
    protected float currentHealth;
    protected float currentMana;
	protected Vector3 movement;                   // The vector to store the direction of the player's movement.
	protected Vector3 jumpingMovement;
	protected Rigidbody rb;          // Reference to the player's rigidbody.
	protected float horizontal;
	protected float jumping;
	protected int turnOffset;
	protected bool isJumping;
	protected bool canRangeAttack;
	protected bool canMeleeAttack;
	protected bool isWin;
	protected bool isBlockLeft;//this determine the direction the character should be going to activate block
	protected bool isBlocking;
	protected bool shouldWaitAnimationFinish;
	protected Animator myAnimator;
	protected gameController myGameController;//for all the spawning object pool
	float doubleTapTimer;
	float highJumpTimer;
	float tapspeed = 0.3f;
	bool isDoubleTap;
   // public float healthBarLength;

    // Use this for initialization
    void Awake () {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;

		mymelee = transform.Find ("melee trigger box").GetComponent<melee> ();
		rb = GetComponent<Rigidbody> ();
		isJumping = false;
		isWin = false;
		speed = normalSpeed;
		jumpSpeed = lowJumpSpeed;
		shouldWaitAnimationFinish = false;
		turnOffset = 1;
		CurrentChargingBar = Mathf.Clamp01 (CurrentChargingBar);

		isBlocking = false;
		isDoubleTap = false;
		canRangeAttack = true;
		canMeleeAttack = true;
		myGameController = GameObject.Find ("gameManager").GetComponent<gameController> ();
		coolDownRangeTimer = coolDownRangeAttackRate;
		coolDownMeleeTimer = coolDownMeleeAttackRate;
		//myAnimator = GetComponent<Animator> ();
		StartCoroutine (regenMana (0.5f));

    }

	void Start()
	{
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();
		chargingBar.fillAmount = CurrentChargingBar ;
	}
	IEnumerator regenMana(float time)
	{
		while(true)
		{
			if(currentMana < startingMana)
				currentMana += manaRegenRate;

			yield return new WaitForSeconds(time);
		}
		yield return null;
	}
    protected void Update()
	{
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();
		chargingBar.fillAmount = CurrentChargingBar;

		checkCoolDown ();   
		Move ();
		jump ();
		crouch ();
		if (currentMana <= 0)
			currentMana = 0;

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
    }
	public void setMana(float amount)
	{
		currentMana += amount;
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
	public GameObject getEnemy()
	{
		return enemy;
	}
	public bool getIsBlocking()
	{
		return isBlocking;
	}
	public void addCurrentChargingBar(float amount)
	{
		CurrentChargingBar += amount;
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
		coolDownMeleeTimer -= Time.deltaTime;
		if (coolDownRangeTimer <= 0)
			canRangeAttack = true;
		else
			canRangeAttack = false;

		if (coolDownMeleeTimer <= 0)
			canMeleeAttack = true;
		else
			canMeleeAttack = false;
	}

    /*****************SCALE***********************/
   /* Offensive Factor (OffFactor): 0.75 - 1.25 (Weak - Strong)
      Defensive Factor (DefFactor): 0.75 - 1.25 (Weak - Strong)
   */
	public virtual void checkDead()
	{
		if (currentHealth <= 0)
		{
			//myAnimator.SetBool ("die", true);
			//gameObject.SetActive (false);
			isWin = true;
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

		if(isBlockLeft == true)
		{
			if(horizontal < 0)//going left side
				isBlocking = true;
			else
				isBlocking = false;
		}
		else
		{
			if(horizontal > 0)//going right side
				isBlocking = true;
			else
				isBlocking = false;
		}

		
//		if (horizontal >= 1 || horizontal < 0)
//			myAnimator.SetBool ("walk", true);
//		else//equal to 0
//			myAnimator.SetBool ("walk", false);

		movement.x = horizontal * speed;

		rb.velocity = new Vector3 (movement.x, rb.velocity.y, rb.velocity.z);   
		//rb.velocity = transform.forward * horizontal * speed;
		
	}
	protected virtual void crouch()
	{
		if(Input.GetKeyDown("s"))
		{
			highJumpTimer = Time.time;
			Debug.Log("ss");
		}

	}
	protected virtual void jump()
	{
		if(isAI == false)
			jumping = Input.GetAxisRaw ("Jump");

		if(Input.GetButtonDown("Jump"))
		{
			if(Time.time - highJumpTimer < tapspeed)
			{
				jumpSpeed = highJumpSpeed;
			}
		}
		if(jumping > 0 && check_touchGround(groundMask) == true)//player press jump while on the ground
		{

			jumpingMovement.y = jumpSpeed;
			rb.velocity = new Vector3 (rb.velocity.x, jumpingMovement.y, 
				                           rb.velocity.z);
			isJumping = true;


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
			Debug.Log("in melee");
			mymelee.enabled = true;
			coolDownMeleeTimer = coolDownMeleeAttackRate;
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







