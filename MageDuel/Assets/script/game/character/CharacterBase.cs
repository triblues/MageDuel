/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterBase : MonoBehaviour {
	
	[SerializeField] bool isAI = false;
	[SerializeField] protected int characterTag;
	[SerializeField] protected float startingHealth = 100.0f;
	[SerializeField] protected float startingMana = 100.0f;
	[SerializeField] protected float speed = 10.0f;
	[SerializeField] protected float offFactor = 1.0f;//offensive power
	[SerializeField] protected float defFactor = 1.0f;//defensive power 
	[SerializeField] protected float jumpSpeed = 10.0f;
	[SerializeField] protected float fallSpeed = 0.5f;
	[SerializeField] protected float coolDownAttackRate = 0.5f;
	[SerializeField] protected float manaRegenRate = 1.0f;
	[SerializeField] float distanceToGround = 0.1f;//the amount of dist to stop falling
	[SerializeField] float distanceToWall = 0.5f;
	protected float coolDownTimer;
    protected float currentHealth;
    protected float currentMana;
	protected Vector3 movement;                   // The vector to store the direction of the player's movement.
	protected Vector3 jumpingMovement;
	protected Rigidbody rb;          // Reference to the player's rigidbody.
	protected float horizontal;
	protected float jumping;
	protected int turnOffset;
	protected bool isJumping;
	protected bool canAttack;
	protected gameController myGameController;

   // public float healthBarLength;

    // Use this for initialization
    void Awake () {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;

		rb = GetComponent<Rigidbody> ();
		isJumping = false;
		turnOffset = 1;
		canAttack = true;
		myGameController = GameObject.Find ("gameManager").GetComponent<gameController> ();
		coolDownTimer = coolDownAttackRate;

		StartCoroutine (regenMana (1.0f));
    }

    // Function to setup player's special attributes (speed etc)
//    protected virtual void SetupPlayer()
//    {
//        // Properties by default
//        speed = 5.0f;
//        offFactor = 1.0f;
//        defFactor = 1.0f;
//    }
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
		Move ();
		jump ();
		if (currentMana <= 0)
			currentMana = 0;
		//transform.Translate (movement * Time.deltaTime,Space.Self);
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
		coolDownTimer -= Time.deltaTime;
		if (coolDownTimer <= 0)
			canAttack = true;
		else
			canAttack = false;
	}

    /**************************************************************/
//    void OnGUI()
//    {
//        GUI.Box(new Rect(10, 10, healthBarLength, 20), currentHealth + "/" + startingHealth);
//    }

    /*****************SCALE***********************/
   /* Offensive Factor (OffFactor): 0.75 - 1.25 (Weak - Strong)
      Defensive Factor (DefFactor): 0.75 - 1.25 (Weak - Strong)
   */
	public virtual void checkDead()
	{
		if (currentHealth <= 0)
			gameObject.SetActive (false);
	}
	protected virtual void attack()
	{
		
	}
	protected virtual void Move()
	{
		// Store the input axes.
		if(isAI == false)
			horizontal = Input.GetAxisRaw ("Horizontal");
		


		movement.x = horizontal * speed;
		//movement.x = Vector3.forward.x * horizontal * turnOffset * speed;
		//movement.z = Vector3.forward.z * horizontal * turnOffset * speed;

		//transform.Translate (movement * Time.deltaTime,Space.Self);
		rb.velocity = new Vector3 (movement.x, rb.velocity.y, rb.velocity.z);   
		
		
	}
	protected virtual void jump()
	{
		if(isAI == false)
			jumping = Input.GetAxisRaw ("Jump");
		
		if(jumping > 0 && check_touchGround() == true)//player press jump while on the ground
		{
			//movement.y = jumpSpeed;
			jumpingMovement.y = jumpSpeed;
			rb.velocity = new Vector3 (rb.velocity.x, jumpingMovement.y, 
				                           rb.velocity.z);
			isJumping = true;

		}
		else if(jumping == 0 && check_touchGround() == true)//player reach the ground
		{
			//movement.y = 0;
			jumpingMovement.y = 0;
			isJumping = false;
		}
		else//player still in the air
		{
			//movement.y -= fallSpeed;
			jumpingMovement.y = -fallSpeed;
			rb.AddForce(jumpingMovement,ForceMode.Acceleration);
			
		}
		

	}
	protected bool check_touchWall(Vector3 self, Vector3 wall,float dist)
	{
		if(Physics.Raycast(self,wall,dist) == true)//origin,direction,max dist
		{
			return true;//player touch the wall
		}
		else
			return false;
	}
	protected bool check_touchGround()
	{
		if(Physics.Raycast(transform.position,Vector3.down,distanceToGround) == true)//origin,direction,max dist
		{
			//Debug.Log("true");
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

}







