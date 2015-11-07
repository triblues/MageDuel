﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class pristinePlayerController : CharacterBase
{
    public delegate void spellDelegate();
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
    bool isInResult;


    protected drawShape myDrawShape;
    drawShape.shape lastDrawShape;
    GameObject iceArmorObj;
    


    protected override void Awake()
    {

        healthBar = GameObject.Find("Canvas").transform.Find("player/health/outer/inner").GetComponent<Image>();
        manaBar = GameObject.Find("Canvas").transform.Find("player/mana/outer/inner").GetComponent<Image>();

        combo = GameObject.Find("Canvas").transform.Find("player/combo text").gameObject;
        chargingBar = GameObject.Find("Canvas").transform.Find("player/charging bar outer/charging bar inner").
            GetComponent<Image>();

        UIarmorCD = GameObject.Find("Canvas").transform.Find("player/armor image outer/armor image inner").GetComponent<UICoolDown>();
        UIActiveCD = GameObject.Find("Canvas").transform.Find("player/active image outer/active image inner").GetComponent<UICoolDown>();

        iceArmorObj = transform.Find("ice armor").gameObject;//armor
        //need active
        //myActivePS
        myPassivePS = transform.Find("OTUSpell").GetComponent<ParticleSystem>();//passive

        myUltimatePS = transform.parent.Find("ice ultimate").GetComponent<ParticleSystem>();//the particle when player has successfully landed the ultimate
        ultimateObj = transform.parent.Find("ultimate start").gameObject;//the particle to show player has activate the ultimate
        ultimateObj.GetComponent<weaponBase>().setTag(characterTag);

        canCastSpell = new bool[3];
        for (int i = 0; i < canCastSpell.Length; i++)
            canCastSpell[i] = true;

        isInResult = false;
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        //myDrawShape = GameObject.Find("draw line").GetComponent<drawShape>();

        enemy = GameObject.FindWithTag("Enemy").gameObject;
        enemyTrans = enemy.GetComponent<Transform>();
        lastDrawShape = drawShape.shape.no_shape;

     

    }

    protected void showResult()
    {
        if (isInResult == false)
        {
            if (gameController.isFinish == true)
            {
                if (currentHealth <= 0)
                {
                    myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, false);

                }
                if (enemy.GetComponent<CharacterBase>().getCurrentHealth() <= 0)
                {
                    myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, true);

                }
                if (currentHealth >= enemy.GetComponent<CharacterBase>().getCurrentHealth())
                {
                    myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, true);

                }
                else
                {
                    myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, false);

                }
                isInResult = true;
            }

        }
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
            if (chargingBar.fillAmount >= 1)
            {
                //ultimateMove();
                ultimateSpell();
            }
        }

        if (isInUltimate == false)
            attack();

    }
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

                    rangeAttack(offsetPos, direction);
                    lastDrawShape = drawShape.shape.no_shape;//reset

                }
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
                if (canCastSpell[0] == false)//armor spell
                    return;//in cooldown

                

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
                if (canCastSpell[2] == false)//instant cooldown spell
                    return;//in cooldown

              
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
                if (canCastSpell[1] == false)//speed spell
                    return;//in cooldown

             
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
                if (isAttack == true)
                {
                    Debug.Log("in here att");
                    return;
                }

                //single attack
                rangeAttackAnimation();
                Vector3 offsetPos = transform.position;
                offsetPos.y = offsetPos.y + 1;
                Vector3 direction = enemy.transform.position - offsetPos;

                rangeAttack(offsetPos, direction);
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


    protected override void attack()
    {
        if (currentMana <= 0)
            return;
        if (canRangeAttack == true)
        {
            if (Input.GetKeyDown("k"))//one iceball
            {
                if (canMeleeAttack == false && canRangeAttack == false)//prevent use of range attack
                    return;

                Vector3 offsetPos = transform.position;
                Vector3 offsetPos_enemy = enemyTrans.position;
                offsetPos.y = offsetPos.y + 1;
                offsetPos_enemy.y = offsetPos_enemy.y + 1;
                Vector3 direction = offsetPos_enemy - offsetPos;

                rangeAttack(offsetPos, direction);
                if (isNotEnoughMana == false)
                    rangeAttackAnimation();
            }
            if (Input.GetKeyDown("l")) //multiple iceball
            {
                if (canMeleeAttack == false && canRangeAttack == false)
                    return;

                for (int i = 1; i <= 3; i++)//3
                {
                    Vector3 newPos = new Vector3(enemyTrans.position.x,
                                                 enemyTrans.position.y, enemyTrans.position.z);
                    newPos.y = newPos.y + i * 1.5f;
                    Vector3 offsetPos = transform.position;
                    offsetPos.y = offsetPos.y + 1;
                    Vector3 direction = newPos - offsetPos;

                    rangeAttack(offsetPos, direction);
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

    void spellCombo(float[] cdrate, float timeNeeded, spellDelegate myspellDelegate, KeyCode kc1, KeyCode kc2, KeyCode kc3 = KeyCode.None)
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
                    Debug.Log("yea");
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

    void armorSpell()
    {
        if (canCastSpell[0] == true)//armor spell
        {
            
            canCastSpell[0] = false;
            defFactor = 1.5f;//increase defense
            iceArmorObj.SetActive(true);
            
            
            spellCastCoolDown[0] = spellCastCoolDown[0] * spellCoolDownRate;
            UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);
            StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));

        }
    }
    void activeSpell()
    {
        if (canCastSpell[1] == true)//slow enemy spell
        {

            canCastSpell[1] = false;
            enemy.GetComponent<CharacterBase>().setSpeed(0.5f);//enemy speed become half
            
            //need put shoot ice particle

            spellCastCoolDown[1] = spellCastCoolDown[1] * spellCoolDownRate;
            UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);
            StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));

        }
    }
    void passiveSpell()
    {
        if (canCastSpell[2] == true)//increase enemy cooldown spell
        {
            enemy.GetComponent<CharacterBase>().setSpellCoolDownRate(2);
            
            myPassivePS.Play();
            StartCoroutine(spellDurationTimer(spellType.passive_spell, spellDuration[2]));
        }
    }
    void ultimateSpell()
    {
        if (canMove == true && isJumping == false && playBlockAnimation == false
            && isCrouch == false && isStun == false)
        {
            if (canCastUltimate == true)
            {
                canCastUltimate = false;
                canMove = false;
                myAnimator.SetTrigger("castUltimate");
                StartCoroutine(WaitForAnimation("cast ultimate", 0));

                ultimateObj.SetActive(true);
                ultimateObj.GetComponent<Transform>().position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 10.0f, enemyTrans.position.z);
            }
        }
        //prepare for ultimate to see if ultimate move hit enemy
    }
    public override void ultimateMove()
    {
        //this mean that player successfully use ultimate on enemy
        Debug.Log("ice att");
        chargingBar.fillAmount = 0;
        myAnimator.SetTrigger("ultimate");
        myUltimatePS.gameObject.transform.position = new Vector3(enemyTrans.position.x, 15, enemyTrans.position.z);
        myUltimatePS.Play();
        myUltiCamera.setDetail(transform, enemy.transform, isBlockLeft, 2.0f);

        isInUltimate = true;
        enemy.GetComponent<CharacterBase>().setisInUltimate(true);
        StartCoroutine(spellDurationTimer(spellType.ultimate_spell, 8.0f));
    }

    IEnumerator spellDurationTimer(spellType _spell, float timeTaken)
    {
        yield return new WaitForSeconds(timeTaken);


        if (_spell == spellType.armor_spell)
        {
            canCastSpell[0] = true;
            defFactor = 1.0f;
            iceArmorObj.SetActive(false);
        }
        else if (_spell == spellType.active_spell)
        {
            //return enemy speed to normal speed
            enemy.GetComponent<CharacterBase>().setSpeed(1.0f);
        }
        else if (_spell == spellType.passive_spell)
        {
            enemy.GetComponent<CharacterBase>().setSpellCoolDownRate(1);//return enemy spell cooldown to normal rate
        }
        else if (_spell == spellType.ultimate_spell)
        {
           
            myUltiCamera.removeUltimate();
            isInUltimate = false;
            enemy.GetComponent<CharacterBase>().setisInUltimate(false);
            myUltimatePS.Stop();
            enemy.GetComponent<CharacterBase>().TakesDamage(ultimateDamage);


        }


    }
    //[Client]
    //protected void setNetworkIdentify()
    //{
    //    mynetworkID = GetComponent<NetworkIdentity>().netId;
    //}

}

