using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class mainPlayer : CharacterBase {

    protected drawShape myDrawShape;
    
    protected override void Awake()
    {
        
        healthText = GameObject.Find("Canvas").transform.Find("player/health").GetComponent<Text>();
        manaText = GameObject.Find("Canvas").transform.Find("player/mana").GetComponent<Text>();

        combo = GameObject.Find("Canvas").transform.Find("player/combo text").gameObject;
        chargingBar = GameObject.Find("Canvas").transform.Find("player/charging bar outer/charging bar inner").
            GetComponent<Image>();

           

        
        base.Awake();
    }
    protected override void Start()
	{
        base.Start();
        myDrawShape = GetComponent<drawShape>();
       
        enemy = GameObject.FindWithTag("Enemy").gameObject;
        // Debug.Log(enemy.name);



    }

	protected override void Update()
	{
      
        setAnimation();
        if (gameController.isFinish == true)
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
            rangeAttackAnimation();
            Vector3 offsetPos = transform.position;
            offsetPos.y = offsetPos.y + 1;
            Vector3 direction = enemy.transform.position - offsetPos;

         rangeAttack(offsetPos, direction,gameController.projectileType.fireball);
		}
		if (Input.GetKeyDown ("l")) //multiple fireball
		{
            rangeAttackAnimation();
            for (int i=0;i<3;i++)//3
			{
				Vector3 newPos = new Vector3(enemy.transform.position.x,
				                             enemy.transform.position.y,enemy.transform.position.z);
				newPos.y = newPos.y + i*1.5f;
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = newPos - offsetPos;
               
                rangeAttack(offsetPos, direction,gameController.projectileType.fireball);
			
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
                //if (coolDownMeleeTimer[1] <= 0)
                if (isMeleeComboCount[1] == false)
                {
                    if (Time.time - coolDownMeleeTimer[0] < coolDownMeleeAttackRate)
                    {
                        
                     //   if(enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
                         //   enemy.GetComponent<CharacterBase>().TakesDamage(3.0f);

                        coolDownMeleeTimer[1] = Time.time;//this check if player press fast enough for the next combo
                        isMeleeComboCount[1] = true;//this is for melee 2nd attack animation
                        //Debug.Log("in 2nd");
                    }
                    //else
                    //{
                    //    Debug.Log("in here");
                    //    canCombo = false;
                    //    canMeleeAttack = true;
                    //    coolDownMeleeTimer[0] = 0;
                    //    isMeleeComboCount[0] = false;
                    //}
                }
                else
                {
                    if (isMeleeComboCount[2] == false)
                    {
                        if (Time.time - coolDownMeleeTimer[1] < coolDownMeleeAttackRate)
                        {
                            //  stunTimer = coolDownStunRate * 3;
                            //if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
                            //{
                            //    enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                            //    enemy.GetComponent<CharacterBase>().TakesDamage(3.0f);
                            //}
                           
                            isMeleeComboCount[2] = true;//this is for melee 3rd attack animation
                            meleeAttack();//do last combo
                           // Debug.Log("in 3rd");

                        }
                        //else
                        //{
                        //    canCombo = false;
                        //    canMeleeAttack = true;
                        //    coolDownMeleeTimer[0] = 0;
                        //    coolDownMeleeTimer[1] = 0;
                        //    isMeleeComboCount[0] = false;
                        //    isMeleeComboCount[1] = false;
                        //}
                    }
                  
                    

                }
            }
         

            if (isMeleeComboCount[2] == false)//cannot attack when in final combo
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

    //[Client]
    //protected void setNetworkIdentify()
    //{
    //    mynetworkID = GetComponent<NetworkIdentity>().netId;
    //}

}
