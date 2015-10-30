using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class mainPlayer : CharacterBase {

    enum spellType
    {
        armor_spell,
        active_spell,
        passive_spell,
        ultimate_spell,
        no_spell
    };
    [Header("armor,active,passive")]
    [SerializeField] int[] Inferno_spellCastCoolDown;
    [SerializeField] int[] Pristine_spellCastCoolDown;
    [SerializeField] int[] Radiance_spellCastCoolDown;
    [SerializeField] int[] Inferno_spellDuration;
    [SerializeField] int[] Pristine_spellDuration;
    [SerializeField] int[] Radiance_spellDuration;
    
    ParticleSystem myArmorPS;
    protected drawShape myDrawShape;
    drawShape.shape lastDrawShape;
    
    bool[] canCastSpell;
   
    
    protected override void Awake()
    {

        healthBar = GameObject.Find("Canvas").transform.Find("player/health/outer/inner").GetComponent<Image>();
        manaBar = GameObject.Find("Canvas").transform.Find("player/mana/outer/inner").GetComponent<Image>();

        combo = GameObject.Find("Canvas").transform.Find("player/combo text").gameObject;
        chargingBar = GameObject.Find("Canvas").transform.Find("player/charging bar outer/charging bar inner").
            GetComponent<Image>();


        myArmorPS = transform.Find("armor").GetComponent<ParticleSystem>();
        
        canCastSpell = new bool[3];
        for (int i = 0; i < canCastSpell.Length; i++)
            canCastSpell[i] = true;
        

        base.Awake();
    }
    protected override void Start()
	{
        base.Start();
        myDrawShape = GameObject.Find("draw line").GetComponent<drawShape>();

        enemy = GameObject.FindWithTag("Enemy").gameObject;
        lastDrawShape = drawShape.shape.no_shape;


        //StartCoroutine(castingModeCountDown(1.0f));
        
       
    }
  
   
	protected override void Update()
    {
        
        setAnimation();
        if (gameController.isFinish == true)
        {
            resetAnimation();
            return;
        }
       
        checkBlocking();
        base.Update ();

		if(shouldTurn (transform.position, enemy.transform.position) == true)//facing left
		{
			isBlockLeft = false;
			rb.rotation = Quaternion.Euler (0, 270, 0);
		}
		else//facing right
		{
			isBlockLeft = true;
			rb.rotation = Quaternion.Euler (0, 90, 0);
		}

        //if (isCastMode == false)
        //{
        //    if(isCastModeAnimation == false)
        //    {
        //        if(characterSelectManager.selectedCharacter == (int)characterSelectManager.mage.Inferno)
        //        {
        //          //  attack();
        //        }
        //        else if (characterSelectManager.selectedCharacter == (int)characterSelectManager.mage.Pristine)
        //        {
        //           // attack();
        //        }
        //        else if (characterSelectManager.selectedCharacter == (int)characterSelectManager.mage.Radiance)
        //        {
        //            //if (Input.GetKeyDown("space"))
        //            //{
        //            //  //  castingModeAnimation();
        //            //  //activateCastingMode();
        //            //}
        //           // RadianceSpell();//keyboard spell
        //           // comboAttack();
        //        }
        //    }
                
        //}
        

	}
    void checkBlocking()
    {
        if(blockCount <= 0)
        {
            isBlocking = false;
            return;
        }
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
        if (Input.GetKeyDown("space"))
        {
            //castingModeAnimation();
            activateCastingMode();
        }

        if (currentMana <= 0)
			return;
        if (canRangeAttack == true)
        {
            if (Input.GetKeyDown("k"))//one fireball
            {
                if (isAttack == true)//prevent use of range attack
                    return;
                rangeAttackAnimation();
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = enemy.transform.position - offsetPos;

                rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
            }
            if (Input.GetKeyDown("l")) //multiple fireball
            {
                if (isAttack == true)
                    return;
                rangeAttackAnimation();
                for (int i = 0; i < 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemy.transform.position.x,
                                                 enemy.transform.position.y, enemy.transform.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction, gameController.projectileType.fireball);

                }
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

      
        comboAttack();

    }
 
    void comboAttack()
    {
        if (isEndOfRangeAttack == false)//still doing range attack
            return;
        if (Input.GetKeyDown("o"))//melee attack
        {
            if(canCombo == false)
            {
                
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
            }
            if (isMeleeComboCount[0] == false)//1st attack
            {
               
                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                StartCoroutine(WaitForAnimation("melee 1", 0));

            }
            else
            {
                if (canCombo == true)
                {          
                    if (isMeleeComboCount[1] == false)//haven do 2nd combo
                    {
                        if (coolDownMeleeTimer[0] > 0)//2nd attack combo
                        {
                            meleeAttack();
                            isMeleeComboCount[1] = true;
                            coolDownMeleeTimer[1] = coolDownMeleeAttackRate;
                            myAnimator.SetBool("meleeAttack2", true); 
                        }
                    }
                    else
                    {
                        if (isMeleeComboCount[2] == false)//haven do final combo
                        {
                            if (coolDownMeleeTimer[1] > 0)//final combo
                            {
                                meleeAttack();
                                isMeleeComboCount[2] = true;
                                myAnimator.SetBool("meleeAttack3", true);
                                myAnimator.SetTrigger("finishCombo");
                                //myAnimator.SetBool("finishCombo", true);
                                StartCoroutine(WaitForAnimation("melee 3", 0));
                              
                            }
                        }
                    }
                }

            }
        }
       
    }

    public void checkShapeDraw(drawShape.shape myshape)
    {
        if (canRangeAttack == false)//prevent use of melee
            return;

        if (lastDrawShape == drawShape.shape.horizontal_line)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (isAttack == true)
                    return;
             
                //multiple attack
                rangeAttackAnimation();
                for (int i = 0; i < 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemy.transform.position.x,
                                                 enemy.transform.position.y, enemy.transform.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
                    lastDrawShape = drawShape.shape.no_shape;//reset

                }
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                checkShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.diagonal_BLTR)//positive
        {
            //in combo
           
            if (myshape == drawShape.shape.diagonal_TLBR)//negative
            {
                Debug.Log("can combo: " + canCombo.ToString());
                if (canCombo == true)
                {
                    //  canCombo = true;
                    castModeMeleeCombo();
                }

                lastDrawShape = drawShape.shape.no_shape;
            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {
                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                StartCoroutine(WaitForAnimation("melee 1", 0));

                lastDrawShape = drawShape.shape.diagonal_BLTR;
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                checkShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.triangle)
        {
            if(myshape == drawShape.shape.square)
            {
              
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                checkShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.square)
        {

        }
        else if (lastDrawShape == drawShape.shape.diamond)
        {

        }
        else if (lastDrawShape == drawShape.shape.no_shape)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (isAttack == true)
                    return;
                //single attack
                rangeAttackAnimation();
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = enemy.transform.position - offsetPos;

                rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
                lastDrawShape = drawShape.shape.no_shape;//reset
                Debug.Log("in vert");
            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {             
                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                StartCoroutine(WaitForAnimation("melee 1", 0));

                lastDrawShape = drawShape.shape.diagonal_BLTR;

                Debug.Log("do not come in twice");
            }
            else
            {
                lastDrawShape = myshape;
            }
        }

       
    }

    protected void castModeMeleeCombo()
    {
        meleeAttack();//2nd attack
      
        myAnimator.SetBool("meleeAttack2", true);
        myAnimator.SetBool("meleeAttack3", true);
        myAnimator.SetTrigger("finishCombo");
        
        StartCoroutine(main_WaitForAnimationStart("melee 3", 0));
    }
    protected void castingModeAnimation()
    {
        if (isCastMode == true || isCastModeAnimation == true)
            return;

        if (chargingBar.fillAmount >= 1)//max
        {
           // myUltiCamera.setCharacterDetail(transform, isBlockLeft);
            myUltiCamera.enabled = true;
            isCastModeAnimation = true;
        
            myAnimator.SetBool("castingMode", true);

            Camera.main.enabled = false;
            StartCoroutine(main_WaitForAnimation("castingMode", 0));
        }
      
    }
    protected void activateCastingMode()
    {
       
        myUltiCamera.enabled = false;
     
        isCastMode = true;
      //  canMeleeCast = true;
        myDrawShape.enabled = true;
        StartCoroutine(castingModeCountDown(1.0f));
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
              //  canMeleeCast = false;
                CurrentChargingBar = 0;
                break;
            }

            yield return new WaitForSeconds(wait);
        }
    }

    protected IEnumerator main_WaitForAnimationStart(string name,int count)
    {
        while (myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == false)//the animation has not run yet
        {
           
            yield return new WaitForSeconds(0.05f);

        }
        if(name == "melee 3")
        {
            isMeleeComboCount[1] = false;
            isMeleeComboCount[2] = true;
            myAnimator.SetBool("meleeAttack1", false);
            myAnimator.SetBool("meleeAttack2", false);
            StartCoroutine(WaitForAnimation("melee 3", 0));
        }
    }
    protected IEnumerator main_WaitForAnimation(string name, int count)
    {
        yield return new WaitForSeconds(1.0f);
        while (myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == true)//the animation is still running
        {
            if (name == "castingMode")
            {
                myAnimator.SetBool("castingMode", false);
            }
            yield return new WaitForSeconds(0.05f);

        }
        if (name == "castingMode")
        {
            isCastModeAnimation = false;
            activateCastingMode();
        }
        else if(name == "melee 3")
        {
        
            myAnimator.SetBool("meleeAttack3", false);
            Debug.Log("in here?");
            canCombo = false;
        }
    }

    void RadianceSpell()
    {
        if (currentMana <= 0)
            return;
        if (canRangeAttack == true)
        {
            if (Input.GetKeyDown("k"))//one fireball
            {
                if (isAttack == true)//prevent use of range attack
                    return;
                rangeAttackAnimation();
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = enemy.transform.position - offsetPos;

                rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
            }
            if (Input.GetKeyDown("l")) //multiple fireball
            {
                if (isAttack == true)
                    return;
                rangeAttackAnimation();
                for (int i = 0; i < 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemy.transform.position.x,
                                                 enemy.transform.position.y, enemy.transform.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction, gameController.projectileType.fireball);

                }
            }
        }
    }
    public void RadianceShapeDraw(drawShape.shape myshape)
    {
        

        if (lastDrawShape == drawShape.shape.horizontal_line)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (isAttack == true)
                    return;

                //multiple attack
                rangeAttackAnimation();
                for (int i = 0; i < 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemy.transform.position.x,
                                                 enemy.transform.position.y, enemy.transform.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
                    lastDrawShape = drawShape.shape.no_shape;//reset

                }
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                RadianceShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.diagonal_BLTR)//positive
        {
            //in combo

            if (myshape == drawShape.shape.diagonal_TLBR)//negative
            {
             //   Debug.Log("can combo: " + canCombo.ToString());
                if (canCombo == true)
                {
                  
                    castModeMeleeCombo();
                }

                lastDrawShape = drawShape.shape.no_shape;
            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {
               
                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                StartCoroutine(WaitForAnimation("melee 1", 0));

                lastDrawShape = drawShape.shape.diagonal_BLTR;
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                RadianceShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.triangle)
        {
            if (myshape == drawShape.shape.square)
            {
                if (Radiance_spellCastCoolDown[0] > 0)
                    return;//in cooldown
                Debug.Log("in armor");
                defFactor = 1.5f;//increase defense 
               
                StartCoroutine(startSpell(spellType.armor_spell, 8.0f));
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                RadianceShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.square)
        {

        }
        else if (lastDrawShape == drawShape.shape.diamond)
        {

        }
        else if (lastDrawShape == drawShape.shape.no_shape)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (isAttack == true)
                    return;
                
                //single attack
                rangeAttackAnimation();
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = enemy.transform.position - offsetPos;

                rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
                lastDrawShape = drawShape.shape.no_shape;//reset
             
            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {
                if (canRangeAttack == false)//prevent use of melee
                    return;
                Debug.Log("melee att 1");
                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                StartCoroutine(WaitForAnimation("melee 1", 0));

                lastDrawShape = drawShape.shape.diagonal_BLTR;

            }
            else
            {
                lastDrawShape = myshape;
            }
        }


    }
    public void InfernoShapeDraw(drawShape.shape myshape)
    {


        if (lastDrawShape == drawShape.shape.horizontal_line)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (isAttack == true)
                    return;

                //multiple attack
                rangeAttackAnimation();
                for (int i = 0; i < 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemy.transform.position.x,
                                                 enemy.transform.position.y, enemy.transform.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
                    lastDrawShape = drawShape.shape.no_shape;//reset

                }
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                InfernoShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.diagonal_BLTR)//positive
        {
            //in combo

            if (myshape == drawShape.shape.diagonal_TLBR)//negative
            {
                //   Debug.Log("can combo: " + canCombo.ToString());
                if (canCombo == true)
                {

                    castModeMeleeCombo();
                }

                lastDrawShape = drawShape.shape.no_shape;
            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {

                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                StartCoroutine(WaitForAnimation("melee 1", 0));

                lastDrawShape = drawShape.shape.diagonal_BLTR;
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                InfernoShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.triangle)
        {
            if (myshape == drawShape.shape.square)
            {
                if (canCastSpell[0] == false)//armor spell
                    return;//in cooldown
                Debug.Log("in armor");
                isKnockBack = false;
                canCastSpell[0] = false;
                myArmorPS.Play();
                StartCoroutine(startSpell(spellType.armor_spell, Inferno_spellDuration[0]));
                StartCoroutine(spellCoolDown(Inferno_spellCastCoolDown[0], canCastSpell, 0));
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                InfernoShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.square)
        {

        }
        else if (lastDrawShape == drawShape.shape.diamond)
        {

        }
        else if (lastDrawShape == drawShape.shape.no_shape)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (isAttack == true)
                    return;

                //single attack
                rangeAttackAnimation();
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = enemy.transform.position - offsetPos;

                rangeAttack(offsetPos, direction, gameController.projectileType.fireball);
                lastDrawShape = drawShape.shape.no_shape;//reset

            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {
                if (canRangeAttack == false)//prevent use of melee
                    return;
                Debug.Log("melee att 1");
                meleeAttack();
                isMeleeComboCount[0] = true;
                myAnimator.SetBool("meleeAttack1", true);
                coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                StartCoroutine(WaitForAnimation("melee 1", 0));

                lastDrawShape = drawShape.shape.diagonal_BLTR;

            }
            else
            {
                lastDrawShape = myshape;
            }
        }


    }

    IEnumerator spellCoolDown(int waitTime,bool[] cd,int num)
    {
      
        yield return new WaitForSeconds(waitTime);
        cd[num] = true;//cool down finish
     
    }
    IEnumerator startSpell(spellType _spell,float timeTaken)
    {
        yield return new WaitForSeconds(timeTaken);

        if (characterSelectManager.selectedCharacter == (int)characterSelectManager.mage.Inferno)
        {
            if (_spell == spellType.armor_spell)
            {
                isKnockBack = true;
                myArmorPS.Stop();
            }
            else if (_spell == spellType.active_spell)
            {
                //defFactor = 1;//reset
            }
            else if (_spell == spellType.passive_spell)
            {
                //defFactor = 1;//reset
            }
        }
        else if (characterSelectManager.selectedCharacter == (int)characterSelectManager.mage.Pristine)
        {
            if (_spell == spellType.armor_spell)
            {
                defFactor = 1;//reset
            }
            else if (_spell == spellType.passive_spell)
            {
                //defFactor = 1;//reset
            }
        }
        else if (characterSelectManager.selectedCharacter == (int)characterSelectManager.mage.Radiance)
        {
            if (_spell == spellType.armor_spell)
            {
                defFactor = 1;//reset
            }
            else if (_spell == spellType.passive_spell)
            {
                //defFactor = 1;//reset
            }
        }
    }
    //[Client]
    //protected void setNetworkIdentify()
    //{
    //    mynetworkID = GetComponent<NetworkIdentity>().netId;
    //}

}
