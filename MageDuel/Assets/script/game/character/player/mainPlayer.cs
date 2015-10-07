using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class mainPlayer : CharacterBase {





	void Start()
	{
	
		



	}

	void Update()
	{
		base.Update ();

		if(shouldTurn (transform.position, enemy.transform.position) == true)//facing right
		{
			isBlockLeft = false;
			rb.rotation = Quaternion.Euler (0, 270, 0);
		}
		else//facing right
		{
			isBlockLeft = true;
			rb.rotation = Quaternion.Euler (0, 90, 0);
		}

		attack ();

	}

	protected override void attack()
	{
		if (currentMana <= 0)
			return;
		if (canRangeAttack == false)
			return;
		if(Input.GetKeyDown("k"))//one fireball
		{


			Vector3 direction = enemy.transform.position - transform.position;
			//shootFireBall(transform.position,direction);
			rangeAttack(transform.position,direction,gameController.projectileType.fireball);
		}
		if (Input.GetKeyDown ("l")) //multiple fireball
		{

			for(int i=0;i<3;i++)//3
			{
				Vector3 newPos = new Vector3(enemy.transform.position.x,
				                             enemy.transform.position.y,enemy.transform.position.z);
				newPos.y = newPos.y + i*1.5f;
				Vector3 direction = newPos - transform.position;

				rangeAttack(transform.position,direction,gameController.projectileType.fireball);
				//shootFireBall(transform.position,direction);
			}
		}
//		if(Input.GetKeyDown ("n"))//one iceball
//		{
//			Vector3 mypos = enemy.transform.position;
//			mypos.y = mypos.y * 8;
//			//Vector3 direction = enemy.transform.position - transform.position;
//
//			rangeAttack(mypos,-transform.up,gameController.projectileType.iceball);
//		}
		if(Input.GetKeyDown("o"))//melee attack
		{
			meleeAttack();
		}
	}

//	void shootFireBall(Vector3 position, Vector3 direction)
//	{
//		if (currentMana <= 0)
//			return;
//		if (canRangeAttack == false)
//			return;
//		GameObject temp = myGameController.getPoolObjectInstance("fireball").getPoolObject ();
//
//		if (temp == null)
//			return;
//		//Vector3 direction = enemy.transform.position - transform.position;
//		fireball projectile = temp.GetComponent<fireball> ();
//		if (currentMana < projectile.getConsumeMana ())//not enough mana to cast spell
//			return;
//
//		temp.transform.position = position + direction.normalized;
//		temp.SetActive (true);
//		projectile.launch (direction);
//		projectile.setTag (characterTag);
//		setMana (-projectile.getConsumeMana ());
//		coolDownRangeTimer = coolDownRangeAttackRate;
//
//
//
//	}


}
