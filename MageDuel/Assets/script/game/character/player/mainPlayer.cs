﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class mainPlayer : CharacterBase {

    protected drawShape myDrawShape;

    void Start()
	{
        myDrawShape = GetComponent<drawShape>();
    }

	protected override void Update()
	{
        if (isFinish == true)
            return;
        checkBlocking();
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

        if(isCastMode == false)
		    attack ();
        else
        {
            checkShapeDraw();
        }

	}
    void checkBlocking()
    {
        if (isBlockLeft == true)
        {
            if (horizontal < 0)//going left side
                isBlocking = true;
            else
                isBlocking = false;
        }
        else
        {
            if (horizontal > 0)//going right side
                isBlocking = true;
            else
                isBlocking = false;
        }
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
			
			}
		}
        if(Input.GetKeyDown("space"))
        {
            activateCastingMode();
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
            if(canCombo == true)
            {
                if (coolDownMeleeTimer[1] <= 0)
                {
                    if (Time.time - coolDownMeleeTimer[0] < coolDownMeleeAttackRate)
                    {
                        
                        if(enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
                            enemy.GetComponent<CharacterBase>().TakesDamage(3.0f);
                        coolDownMeleeTimer[1] = Time.time;

                    }
                    else
                    {
                        canCombo = false;
                        canMeleeAttack = true;
                        coolDownMeleeTimer[0] = 0;
                    }
                }
                else
                {
                    if (Time.time - coolDownMeleeTimer[1] < coolDownMeleeAttackRate)
                    {
                        stunTimer = coolDownStunRate * 3;
                        if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
                        {
                            enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                            enemy.GetComponent<CharacterBase>().TakesDamage(3.0f);
                        }
                       

                    }
                    canCombo = false;
                    canMeleeAttack = true;
                    coolDownMeleeTimer[0] = 0;
                    coolDownMeleeTimer[1] = 0;
                }
            }
         

            meleeAttack();

        }


      
    }
  
    void checkShapeDraw()
    {
      
        if (canRangeAttack == false)
            return;
        if (myDrawShape.getShape() == drawShape.shape.horizontal_line)
        {
            Vector3 direction = enemy.transform.position - transform.position;

            rangeAttack(transform.position, direction, gameController.projectileType.fireball);
           
        }
   
        myDrawShape.setShape();
    }
    protected void activateCastingMode()
    {
        if (isCastMode == true)
            return;
        if (chargingBar.fillAmount >= 1)//max
        {
            isCastMode = true;
            myDrawShape.enabled = true;
            StartCoroutine(castingModeCountDown(1.0f));
        }
    }
    IEnumerator castingModeCountDown(float wait)
    {
        while (true)
        {
            chargingBar.fillAmount -= 0.05f;
            if (chargingBar.fillAmount <= 0)
            {
                myDrawShape.enabled = false;
                isCastMode = false;
                CurrentChargingBar = 0;
                break;
            }

            yield return new WaitForSeconds(wait);
        }
    }

}
