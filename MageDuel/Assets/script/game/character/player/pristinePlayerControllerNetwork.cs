using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class pristinePlayerControllerNetwork : CharacterBaseNetwork
{

    enum spellType
    {
        armor_spell,
        active_spell,
        passive_spell,
        ultimate_spell,
        no_spell
    };


    //[Header("armor,active,passive")]
    //[SerializeField]
    //int[] spellCastCoolDown;
    //[SerializeField]
    //int[] spellDuration;

    GameObject iceArmorObj;
    GameObject myactiveSpellObj;

    //network stuff
    


    //network stuff

    public delegate void spellDelegate();



    protected override void Awake()
    {

        base.Awake();

    }
    protected override void Start()
    {
        base.Start();

        iceArmorObj = transform.Find("ice armor").gameObject;//armor
        //myactiveSpellObj = transform.Find("Ice Ball slow").gameObject;
        
        myPassivePS = transform.Find("OTUSpell").GetComponent<ParticleSystem>();//passive

        myUltimatePS = transform.Find("ice ultimate").GetComponent<ParticleSystem>();//the particle when player has successfully landed the ultimate
        ultimateObj = transform.Find("ultimate start").gameObject;//the particle to show player has activate the ultimate
        ultimateObj.GetComponent<weaponBaseNetwork>().setTag(characterTag);
        ultimateYValue = 10.0f;//ulti start
        ultimateYAmount = 15.0f;//ulti hit

        
    }

    protected override void Update()
    {

        if (canPlay == false)
            return;
        base.Update();
        setAnimation();
        showResult();
        if (gameController.isFinish == true)
        {
            resetAnimation();
            return;
        }
        
        checkBlocking();


        if (shouldTurn(transform.position, enemy.transform.position) == true)//facing left
        {
            isBlockLeft = false;
            rb.rotation = Quaternion.Euler(0, 270, 0);
            if (isLocalPlayer == true)
            {
                spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.D, KeyCode.K);//down right attack
                spellCombo(spellComboActive, 0.5f, activeSpell, KeyCode.W, KeyCode.D, KeyCode.K);
            }
        }
        else//facing right
        {
            isBlockLeft = true;
            rb.rotation = Quaternion.Euler(0, 90, 0);
            if (isLocalPlayer == true)
            {
                spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.A, KeyCode.K);//down left attack
                spellCombo(spellComboActive, 0.5f, activeSpell, KeyCode.W, KeyCode.A, KeyCode.K);
            }

        }

        if (isLocalPlayer == true)
        {
            spellCombo(spellComboPassive, 0.2f, passiveSpell, KeyCode.K, KeyCode.L);

            if (Input.GetKeyDown(KeyCode.Space))
            {
              
                if (chargingBar.fillAmount >= 1)
                {

                    ultimateSpell();
                }
            }
            //if (Input.GetKeyDown(KeyCode.B))
            //{

            //    armorSpell();

            //}
            //if (Input.GetKeyDown(KeyCode.N))
            //{

            //    activeSpell();//shoot iceball slow

            //}
            //if (Input.GetKeyDown(KeyCode.M))
            //{

            //    passiveSpell();

            //}

            if (isInUltimate == false)
                attack();

          
           //if(isFreeze == true)
           // {
           //     if (hasEnterFreeze == false)
           //     {
           //         hasEnterFreeze = true;
           //         Debug.Log("start cd");
           //         StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));
           //     }
           // }
           // else
           // {
           //     hasEnterFreeze = false;
           // }

           // spellCoolDownRate = enemy.GetComponent<CharacterBaseNetwork>().getspellCoolDownRateNetwork();

        }
        else//localplayer false
        {
            if (isPSArmor == true)
                iceArmorObj.SetActive(true);
            else
                iceArmorObj.SetActive(false);
          
            if (isPSOTU == true)
            {
                isPSOTU = false;
                myPassivePS.Play();
            }
        }
    }
   
    protected void showResult()
    {
        //if (isInResult == false)
        //{
        //    if (gameController.isFinish == true)
        //    {
        //        if (currentHealth <= 0)
        //        {
        //            myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, false);

        //        }
        //        if (enemy.GetComponent<CharacterBase>().getCurrentHealth() <= 0)
        //        {
        //            myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, true);

        //        }
        //        if (currentHealth >= enemy.GetComponent<CharacterBase>().getCurrentHealth())
        //        {
        //            myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, true);

        //        }
        //        else
        //        {
        //            myGameController.showGameOver(currentHealth, startingHealth, highestComboAchieve, false);

        //        }
        //        isInResult = true;
        //    }

        //}
    }
    protected override void attack()
    {

        if (currentMana <= 0)
            return;
        if (isStun == true)
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

                CmdrangeAttack(offsetPos, direction,"iceball", myDamageMultipler, characterTag);
                if (isNotEnoughMana == false)
                {
                    // if(isServer == false)
                    // transmitAttack(1);
                    rangeAttackAnimation();
                }
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

                    CmdrangeAttack(offsetPos, direction, "iceball", myDamageMultipler, characterTag);
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




        comboAttack();//at base

    }
    protected override void comboAttack()
    {
        if (isEndOfRangeAttack == false)//still doing range attack
            return;
        if (Input.GetKeyDown("o"))//melee attack
        {

            if (isMeleeComboCount[0] == false)//1st attack
            {

                meleeAttack();
                isMeleeComboCount[0] = true;

                trasmitAttackAnimation(false, true, false, false);
                // myAnimator.SetTrigger("TmeleeAttack1");
                StartCoroutine(WaitForAnimation("melee 1", 0));

            }
            else
            {
                Debug.Log("here 2");
                if (canCombo == true)
                {
                    Debug.Log("can combo");
                    if (isMeleeComboCount[1] == false)//haven do 2nd combo
                    {

                        meleeAttack();
                        Debug.Log("here 2");
                        isMeleeComboCount[1] = true;
                        trasmitAttackAnimation(false, false, true, false);
                        //myAnimator.SetTrigger("TmeleeAttack2");
                        StartCoroutine(WaitForAnimation("melee 2", 0));


                    }
                    else
                    {
                        if (isMeleeComboCount[2] == false)//haven do final combo
                        {

                            enemy.GetComponent<CharacterBase>().setStunRate(3.5f);
                            meleeAttack();
                            Debug.Log("here 3");
                            isMeleeComboCount[2] = true;
                            trasmitAttackAnimation(false, false, false, true);
                            //myAnimator.SetTrigger("TmeleeAttack3");

                            //myAnimator.SetTrigger("finishCombo");
                            StartCoroutine(WaitForAnimation("melee 3", 0));

                        }
                    }
                }
                else
                {
                    Debug.Log("cannot combo");
                }

            }
        }

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


    void ultimateSpell()
    {
        if (canMove == true && isJumping == false && playBlockAnimation == false
            && isCrouch == false && isStun == false)
        {
            if (canCastUltimate == true)
            {
                
                canCastUltimate = false;
                transmitUltimate(true, false);
                canMove = false;
                //myAnimator.SetTrigger("castUltimate");
                StartCoroutine(WaitForAnimation("cast ultimate", 0));

               // ultimateObj.SetActive(true);
               // ultimateObj.GetComponent<Transform>().position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + ultimateYValue,
                 //   enemyTrans.position.z);
            }
        }
        //prepare for ultimate to see if ultimate move hit enemy
    }
    public override void ultimateMove()
    {
        //this mean that player successfully use ultimate on enemy
        
        myserverLogic.setisInUltimateServer(true);
        addCurrentChargingBar(-1.0f);
        //myAnimator.SetTrigger("ultimate");
        //myUltimatePS.gameObject.transform.position = new Vector3(enemyTrans.position.x, 15, enemyTrans.position.z);
        //myUltimatePS.Play();
        myUltiCamera.setDetail(transform, enemyTrans, isBlockLeft, 2.0f);

        isInUltimate = true;
        transmitUltimate(false, true);

        //enemy.GetComponent<CharacterBase>().setisInUltimate(true);
        StartCoroutine(spellDurationTimer(spellType.ultimate_spell, 8.0f));
    }

    void armorSpell()
    {
        if (canCastSpell[0] == true)//ice armor spell
        {
            canCastSpell[0] = false;
            defFactor = 1.5f;//increase defense
            iceArmorObj.SetActive(true);
            transmitParticleSystem(true, false, false, true, false);

            armorUICD = true;
            transmitUI(true, false);
            //spellCastCoolDown[0] = spellCastCoolDown[0] * spellCoolDownRate;
            //UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);
            StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));

        }
    }
    void activeSpell()
    {
        if (canCastSpell[1] == true)//slow enemy spell
        {

            canCastSpell[1] = false;

            shootIceBallSLow();
            rangeAttackAnimation();
            activeUICD = true;
            transmitUI(false, true);

            //StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));
            //spellCastCoolDown[1] = spellCastCoolDown[1] * spellCoolDownRate;
            //UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);


        }
    }

    public override void showHitEffect()
    {
        //isFreeze = true;
        //transmitFreeze(isFreeze);

        //freezeEffect.SetActive(true);
        //freezeEffect.transform.SetParent(enemyTrans);
        //freezeEffect.transform.position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 1, enemyTrans.position.z);
        StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));
    }

    void shootIceBallSLow()
    {
        //GameObject temp = getPoolObject("iceballslow");
        //if(temp == null)
        //{
        //    return;
        //}
        Vector3 offsetPos = transform.position;
        Vector3 offsetPos_enemy = enemyTrans.position;
        offsetPos.y = offsetPos.y + 1;
        offsetPos_enemy.y = offsetPos_enemy.y + 1;
        Vector3 direction = offsetPos_enemy - offsetPos;
        CmdrangeAttack(offsetPos, direction, "iceballslow", myDamageMultipler, characterTag);
    }
    void passiveSpell()
    {
        if (canCastSpell[2] == true)//increase enemy cooldown spell
        {
            //enemy.GetComponent<CharacterBaseNetwork>().setSpellCoolDownRate(2);
            transmitspellCoolDownRate(2);
            myPassivePS.Play();
            transmitParticleSystem(false, false, true, false, false);
            StartCoroutine(spellDurationTimer(spellType.passive_spell, spellDuration[2]));
        }
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

    IEnumerator spellDurationTimer(spellType _spell, float timeTaken)
    {
        yield return new WaitForSeconds(timeTaken);


        if (_spell == spellType.armor_spell)
        {
            canCastSpell[0] = true;
            defFactor = 1.0f;
            iceArmorObj.SetActive(false);
            transmitParticleSystem(true, false, false, false, false);
        }
        else if (_spell == spellType.active_spell)
        {
            //return enemy speed to normal speed
            isFreeze = false;
            transmitFreeze(isFreeze);

            //enemy.GetComponent<CharacterBaseNetwork>().setSpeed(1.0f);
           // freezeEffect.SetActive(false);

        }
        else if (_spell == spellType.passive_spell)
        {
            transmitspellCoolDownRate(1);
            //enemy.GetComponent<CharacterBaseNetwork>().setSpellCoolDownRate(1);//return enemy spell cooldown to normal rate
        }
        else if (_spell == spellType.ultimate_spell)
        {
            isInUltimate = false;

            myserverLogic.setisInUltimateServer(false);
            myUltiCamera.removeUltimate();
          

            transmitUltimate(false, true);
            enemy.GetComponent<CharacterBaseNetwork>().TakesDamage(ultimateDamage);





        }


    }


}

