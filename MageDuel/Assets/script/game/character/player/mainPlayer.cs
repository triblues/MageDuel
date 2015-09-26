using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class mainPlayer : CharacterBase {

	public Text healthText;
	public Text manaText;
	public GameObject enemy;



	void Start()
	{
	
		
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();
	//	transform.localRotation = Quaternion.Euler(0,270,0);

	}

	void Update()
	{
		base.Update ();
		healthText.text = "Health: " + currentHealth.ToString ();
		manaText.text = "Mana: " + currentMana.ToString ();

		if(shouldTurn (transform.position, enemy.transform.position) == true)
		{
			//transform.localRotation = Quaternion.Euler (0, 270, 0);
			//turnOffset = -1;
			rb.rotation = Quaternion.Euler (0, 270, 0);
		}
		else
		{
			//transform.localRotation = Quaternion.Euler (0, 90, 0);
			//turnOffset = 1;
			rb.rotation = Quaternion.Euler (0, 90, 0);
		}

		checkCoolDown ();
		attack ();

	}

	protected override void attack()
	{
		if (Input.GetButtonDown("Attack1"))
		{
			shootFireBall();
		}
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
		Vector3 direction = enemy.transform.position - transform.position;
		fireball projectile = temp.GetComponent<fireball> ();
		temp.transform.position = transform.position + direction.normalized;
		temp.SetActive (true);
		projectile.launch (direction);
		projectile.setTag (characterTag);
		setMana (-projectile.getConsumeMana ());
		coolDownTimer = coolDownAttackRate;



	}


}
