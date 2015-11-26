using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class infernoPlayerController : CharacterBase
{

    enum spellType
    {
        armor_spell,
        active_spell,
        passive_spell,
        ultimate_spell,
        no_spell
    };
    

    [Header("armor,active,passive")]
    [SerializeField]
    int[] spellCastCoolDown;
    [SerializeField]
    int[] spellDuration;
    

    public delegate void spellDelegate();
    //public static bool canUlti;

   
    //protected drawShape myDrawShape;
    drawShape.shape lastDrawShape;

    


    protected override void Awake()
    {

        healthBar = GameObject.Find("Canvas").transform.Find("player/health/outer/inner").GetComponent<Image>();
        manaBar = GameObject.Find("Canvas").transform.Find("player/mana/outer/inner").GetComponent<Image>();
        ultimateTextAnimator = GameObject.Find("Canvas").transform.Find("player/charging bar outer/Text").GetComponent<Animator>();

        combo = GameObject.Find("Canvas").transform.Find("player/combo text").gameObject;
        chargingBar = GameObject.Find("Canvas").transform.Find("player/charging bar outer/charging bar inner").
            GetComponent<Image>();

        UIarmorCD = GameObject.Find("Canvas").transform.Find("player/armor image outer/armor image inner").GetComponent<UICoolDown>();
        UIActiveCD = GameObject.Find("Canvas").transform.Find("player/active image outer/active image inner").GetComponent<UICoolDown>();

        myArmorPS = transform.Find("armor").GetComponent<ParticleSystem>();//armor
        myActivePS = transform.Find("Fire Trail").GetComponent<ParticleSystem>();//active
        myPassivePS = transform.Find("OTUSpell").GetComponent<ParticleSystem>();//passive
        
        myUltimatePS = transform.parent.Find("fire ultimate").GetComponent<ParticleSystem>();//the particle when player has successfully landed the ultimate
        ultimateObj = transform.parent.Find("ultimate start").gameObject;//the particle to show player has activate the ultimate
        ultimateObj.GetComponent<weaponBase>().setTag(characterTag);


        isInResult = false;
        base.Awake();

        
    }
    protected override void Start()
    {
        base.Start();
        //myDrawShape = GameObject.Find("draw line").GetComponent<drawShape>();

        //canUlti = true;
        enemy = GameObject.FindWithTag("Enemy").gameObject;
        enemyTrans = enemy.GetComponent<Transform>();
        lastDrawShape = drawShape.shape.no_shape;

        //myUltimatePS = GameObject.FindWithTag("Fire Ultimate").GetComponent<ParticleSystem>();

        //myUltiCamera.setCharacterDetail(transform,
        //      new Vector3(transform.position.x - transform.forward.x * 2, transform.position.y + 3, transform.position.z), !isBlockLeft);

       

    }


    protected override void Update()
    {


        setAnimation();
        showResult();
        if (gameController.isFinish == true)
        {
            resetAnimation();
            return;
        }
        base.Update();
        if (isPause == true)
            return;
        checkBlocking();
        
        if (shouldTurn(transform.position, enemy.transform.position) == true)//facing left
        {
            isBlockLeft = false;
            rb.rotation = Quaternion.Euler(0, 270, 0);
           
            spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.D, KeyCode.K);//down right attack
            spellCombo(spellComboActive, 0.5f, activeSpell, KeyCode.W, KeyCode.D, KeyCode.K);
        }
        else//facing right
        {
            isBlockLeft = true;
            rb.rotation = Quaternion.Euler(0, 90, 0);
            spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.A, KeyCode.K);//down left attack
            spellCombo(spellComboActive, 0.5f, activeSpell, KeyCode.W, KeyCode.A, KeyCode.K);
            
        }

        spellCombo(spellComboPassive, 0.2f, passiveSpell, KeyCode.K, KeyCode.L);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(chargingBar.fillAmount >= 1)
            {
                
                ultimateSpell();
            }
        }
      

        if(isInUltimate == false)
            attack();
    }

   //protected void showResult()
   // {
   //     if(isInResult == false)
   //     {
   //         if(gameController.isFinish == true)
   //         {
   //             if (currentHealth <= 0)
   //             {
   //                 myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, false);
                  
   //             }
   //             if (enemy.GetComponent<CharacterBase>().getCurrentHealth() <= 0)
   //             {
   //                 myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, true);
                   
   //             }
   //             if(currentHealth >= enemy.GetComponent<CharacterBase>().getCurrentHealth())
   //             {
   //                 myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, true);
                 
   //             }
   //             else
   //             {
   //                 myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, false);
                   
   //             }
   //             isInResult = true;
   //         }
          
   //     }
   // }
    protected override void attack()
    {
        if (isStun == true)
            return;
        if (currentMana <= 0)
            return;
        if (canRangeAttack == true)
        {
            if (Input.GetKeyDown("k"))//one fireball
            {
                if (canMeleeAttack == false || canRangeAttack == false)//prevent use of range attack
                    return;
               
                Vector3 offsetPos = transform.position;
                Vector3 offsetPos_enemy = enemyTrans.position;
                offsetPos.y = offsetPos.y + 1;
                offsetPos_enemy.y = offsetPos_enemy.y + 1;
                Vector3 direction = offsetPos_enemy - offsetPos;

                rangeAttack(offsetPos, direction,0, myDamageMultipler);
                if (isNotEnoughMana == false)
                    rangeAttackAnimation();
            }
            if (Input.GetKeyDown("l")) //multiple fireball
            {
                if (canMeleeAttack == false || canRangeAttack == false)
                    return;
               
                for (int i = 1; i <= 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemyTrans.position.x,
                                                 enemyTrans.position.y, enemyTrans.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction,0, myDamageMultipler);
                    if (isNotEnoughMana == true)
                    {
                        break;
                    }
                   
                }
                if (isNotEnoughMana == false)
                {
                    rangeAttackAnimation();
                }
                
            }
        }

      


        comboAttack();

    }
    //void comboAttack()
    //{
    //    if (isEndOfRangeAttack == false)//still doing range attack
    //        return;
    //    if (Input.GetKeyDown("o"))//melee attack
    //    {
    //        //if (canCombo == false)
    //        //{
    //        // //   coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
    //        //}
    //        if (isMeleeComboCount[0] == false)//1st attack
    //        {

    //            meleeAttack();
    //            isMeleeComboCount[0] = true;
    //            //myAnimator.SetBool("meleeAttack1", true);
    //            myAnimator.SetTrigger("TmeleeAttack1");
    //            StartCoroutine(WaitForAnimation("melee 1", 0));

    //        }
    //        else
    //        {
    //            if (canCombo == true)
    //            {
    //                if (isMeleeComboCount[1] == false)//haven do 2nd combo
    //                {
    //                   // if (coolDownMeleeTimer[0] > 0)//2nd attack combo
    //                  //  {
    //                        meleeAttack();
    //                        //isInCombo = true;
    //                        isMeleeComboCount[1] = true;
    //                        //coolDownMeleeTimer[1] = coolDownMeleeAttackRate;
    //                        //myAnimator.SetBool("meleeAttack2", true);
    //                        myAnimator.SetTrigger("TmeleeAttack2");
    //                        StartCoroutine(WaitForAnimation("melee 2", 0));

    //                   // }
    //                }
    //                else
    //                {
    //                    if (isMeleeComboCount[2] == false)//haven do final combo
    //                    {

    //                        enemy.GetComponent<CharacterBase>().setStunRate(3.5f);
    //                        meleeAttack();
    //                        isMeleeComboCount[2] = true;
    //                        myAnimator.SetTrigger("TmeleeAttack3");
                              
    //                        myAnimator.SetTrigger("finishCombo");
    //                        Debug.Log("click 3");
    //                        StartCoroutine(WaitForAnimation("melee 3", 0));

    //                    }
    //                }
    //            }

    //        }
    //    }

    //}
    void checkBlocking()
    {
        if (blockCount <= 0)
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

    protected void castModeMeleeCombo()
    {
        
        meleeAttack();//2nd attack

        myAnimator.SetBool("meleeAttack2", true);
        myAnimator.SetBool("meleeAttack3", true);
        myAnimator.SetTrigger("finishCombo");

        StartCoroutine(main_WaitForAnimationStart("melee 3", 0));
    }
    

    protected IEnumerator main_WaitForAnimationStart(string name, int count)
    {
        while (myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == false)//the animation has not run yet
        {

            yield return new WaitForSeconds(0.05f);

        }
        if (name == "melee 3")
        {
            isMeleeComboCount[1] = false;
            isMeleeComboCount[2] = true;
            myAnimator.SetBool("meleeAttack1", false);
            myAnimator.SetBool("meleeAttack2", false);
            StartCoroutine(WaitForAnimation("melee 3", 0));
        }
    }


    
    public override void ShapeDraw(drawShape.shape myshape)
    {
       // if (isCastModeAnimation == true)
           // return;

        if (lastDrawShape == drawShape.shape.horizontal_line)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (canRangeAttack == true && canMeleeAttack == true)
                {

                    //multiple range attack
                    rangeAttackAnimation();
                    for (int i = 1; i <= 3; i++)//3
                    {
                        Vector3 newPos = new Vector3(enemy.transform.position.x,
                                                     enemy.transform.position.y, enemy.transform.position.z);
                        newPos.y = newPos.y + i * 1.5f;
                        Vector3 offsetPos = transform.position;
                        offsetPos.y = offsetPos.y + 1;
                        Vector3 direction = newPos - offsetPos;

                        rangeAttack(offsetPos, direction,0, myDamageMultipler);

                        


                    }
                }
                lastDrawShape = drawShape.shape.no_shape;//reset
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                ShapeDraw(myshape);//call ownself again
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
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                ShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.triangle)
        {
            if (myshape == drawShape.shape.square)
            {
                if (canCastSpell[0] == true)//armor spell
                {
                    isKnockBack = false;
                    canCastSpell[0] = false;
                    myArmorPS.Play();
                    UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);
                    StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));
                  

                    
                }
                lastDrawShape = drawShape.shape.no_shape;
            }
            else if(myshape == drawShape.shape.diamond)//ultimate
            {
                if (chargingBar.fillAmount >= 1)
                {
                //    myUltiCamera.setCharacterDetail(transform,
                //new Vector3(transform.position.x - transform.forward.x * 2, transform.position.y + 3, transform.position.z), !isBlockLeft);
                //    myUltiCamera.enabled = true;
                //    isCastModeAnimation = true;
                //    myUltimatePS.gameObject.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 8, enemy.transform.position.z);
                //    myUltimatePS.Play();
                //    StartCoroutine(spellDurationTimer(spellType.ultimate_spell, 5.0f));
                }
                lastDrawShape = drawShape.shape.no_shape;
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                ShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.square)
        {
            if (myshape == drawShape.shape.diamond)
            {
                if (canCastSpell[2] == true)//instant cooldown spell
                {

                    canCastSpell[0] = true;
                    canCastSpell[1] = true;
                    canCastSpell[2] = false;
                    myPassivePS.Play();
                }
                lastDrawShape = drawShape.shape.no_shape;
               
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                ShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.diamond)
        {
            if (myshape == drawShape.shape.triangle)//speed boost
            {
                if (canCastSpell[1] == true)//speed spell
                {

                    canCastSpell[1] = false;
                    myActivePS.enableEmission = true;
                    normalSpeed = normalSpeed * 2;

                    UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);
                    StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));
                    //StartCoroutine(spellCoolDown(spellCastCoolDown[1], canCastSpell, 1));
                }
                lastDrawShape = drawShape.shape.no_shape;
            }
            else
            {
                lastDrawShape = drawShape.shape.no_shape;
                ShapeDraw(myshape);//call ownself again
            }
        }
        else if (lastDrawShape == drawShape.shape.no_shape)
        {
            if (myshape == drawShape.shape.vertical_line)
            {
                if (canRangeAttack == true && canMeleeAttack == true)
                {
                    //single attack
                    rangeAttackAnimation();
                    Vector3 offsetPos = transform.position;
                    Vector3 offsetPos_enemy = enemy.transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    offsetPos_enemy.y = offsetPos_enemy.y + 1;
                    Vector3 direction = offsetPos_enemy - offsetPos;

                    rangeAttack(offsetPos, direction,0, myDamageMultipler);
                }

                
                lastDrawShape = drawShape.shape.no_shape;//reset

            }
            else if (myshape == drawShape.shape.diagonal_BLTR)//positive
            {
                if (canRangeAttack == true)//check if can use of melee
                {

                    meleeAttack();
                    isMeleeComboCount[0] = true;
                    myAnimator.SetBool("meleeAttack1", true);
                    coolDownMeleeTimer[0] = coolDownMeleeAttackRate;
                    StartCoroutine(WaitForAnimation("melee 1", 0));

                    lastDrawShape = drawShape.shape.diagonal_BLTR;
                }
                else
                    lastDrawShape = drawShape.shape.no_shape;

            }
            else
            {
                lastDrawShape = myshape;
            }
        }


    }

    void ultimateSpell()
    {
        if (canMove == true && isJumping == false && playBlockAnimation == false
            && isCrouch == false && isStun == false)
        {
            if (canCastUltimate == true)
            {
                if (isUnlimitedSpell == false)
                    canCastUltimate = false;
                canMove = false;
                myAnimator.SetTrigger("castUltimate");
                StartCoroutine(WaitForAnimation("cast ultimate", 0));
                ultimateObj.SetActive(true);
                ultimateObj.GetComponent<Transform>().position = new Vector3(enemyTrans.position.x, 0, enemyTrans.position.z);
                //prepare for ultimate to see if ultimate move hit enemy
            }
        }
    }
    public override void ultimateMove()
    {
        //this mean that player successfully use ultimate on enemy
        Debug.Log("fire att");
        //chargingBar.fillAmount = 0;
        
        addCurrentChargingBar(-1.0f);
        myAnimator.SetTrigger("ultimate");
        myUltimatePS.gameObject.transform.position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 8, enemyTrans.position.z);
        myUltimatePS.Play();
        myUltiCamera.setDetail(transform, enemy.transform, isBlockLeft,2.0f);

        isInUltimate = true;
        enemy.GetComponent<CharacterBase>().setisInUltimate(true);
        StartCoroutine(spellDurationTimer(spellType.ultimate_spell, 8.0f));
    }

    void armorSpell()
    {
        if (isInUltimate == true)
            return;
        if (canCastSpell[0] == true)//armor spell
        {
            isKnockBack = false;
            if(isUnlimitedSpell == false)
                canCastSpell[0] = false;
            myArmorPS.Play();

            spellCastCoolDown[0] = spellCastCoolDown[0] * spellCoolDownRate;
            UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);
            StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));

        }
    }
    void activeSpell()
    {
        if (isInUltimate == true)
            return;
        if (canCastSpell[1] == true)//speed spell
        {
            if (isUnlimitedSpell == false)
                canCastSpell[1] = false;
            myActivePS.enableEmission = true;
            normalSpeed = normalSpeed * 2;

            spellCastCoolDown[1] = spellCastCoolDown[1] * spellCoolDownRate;
            UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);
            StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));
            
        }
    }
    void passiveSpell()
    {
        if (isInUltimate == true)
            return;
        if (canCastSpell[2] == true)//instant cooldown spell
        {

            canCastSpell[0] = true;
            canCastSpell[1] = true;

            if (isUnlimitedSpell == false)
                canCastSpell[2] = false;

            UIarmorCD.resetCoolDown();
            UIActiveCD.resetCoolDown();//reset the visual
            myPassivePS.Play();
        }
    }
    void spellCombo(float[] cdrate,float timeNeeded,spellDelegate myspellDelegate, KeyCode kc1, KeyCode kc2, KeyCode kc3 = KeyCode.None)
    {

        if (kc3 != KeyCode.None)
        {
            if (Input.GetKeyDown(kc1))
            {
                cdrate[0] = timeNeeded;

            }
            if (Input.GetKeyDown(kc2))
            {
                if (cdrate[0] > 0)
                {
                    cdrate[1] = timeNeeded;
                }

            }
            if (Input.GetKeyDown(kc3))
            {
                if (cdrate[1] > 0)
                {
                    //do stuff
                    myspellDelegate();
                }
            }
        }
        else
        {
           // Debug.Log("here");
            if (Input.GetKeyDown(kc1))
            { 
                cdrate[0] = timeNeeded;

            }
            if (Input.GetKeyDown(kc2))
            {
                if (cdrate[0] > 0)
                    myspellDelegate();
            }

        }
        if (cdrate[0] > 0)
            cdrate[0] -= Time.deltaTime;
        if (cdrate[1] > 0)
            cdrate[1] -= Time.deltaTime;

    }
   
    IEnumerator spellDurationTimer(spellType _spell, float timeTaken)
    {
        yield return new WaitForSeconds(timeTaken);

        
        if (_spell == spellType.armor_spell)
        {
            isKnockBack = true;
            myArmorPS.Stop();
        }
        else if (_spell == spellType.active_spell)
        {
            normalSpeed = normalSpeed / 2;
            myActivePS.enableEmission = false;
        }
        else if (_spell == spellType.passive_spell)
        {
           
        }
        else if(_spell == spellType.ultimate_spell)
        {
            Debug.Log("end");
            myUltiCamera.removeUltimate();
            isInUltimate = false;
            enemy.GetComponent<CharacterBase>().setisInUltimate(false);
            myUltimatePS.Stop();
            enemy.GetComponent<CharacterBase>().TakesDamage(ultimateDamage);
            ultimateTextAnimator.enabled = false;

        }


    }
    //[Client]
    //protected void setNetworkIdentify()
    //{
    //    mynetworkID = GetComponent<NetworkIdentity>().netId;
    //}

}

