using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class radiancePlayerControllerNetwork : CharacterBaseNetwork
{
    GameObject healObj;

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
    int spellPowerRate;

    protected float[] teleportSpellComboActiveLeft;
    protected float[] teleportSpellComboActiveRight;


    public delegate void spellDelegate();
    public delegate void teleportSpellDelegate(bool direction);


    protected override void Awake()
    {

        base.Awake();

    }
    protected override void Start()
    {
        base.Start();

        healObj = transform.Find("heal").gameObject;//armor
        myActivePS = transform.Find("teleport").GetComponent<ParticleSystem>();//teleport
        myPassivePS = transform.Find("OTUSpell").GetComponent<ParticleSystem>();//passive

        myUltimatePS = transform.Find("light ultimate").GetComponent<ParticleSystem>();
        ultimateObj = transform.Find("ultimate start").gameObject;
        ultimateObj.GetComponent<weaponBaseNetwork>().setTag(characterTag);
        spellPowerRate = 1;
        ultimateYValue = 1.0f;
        ultimateYAmount = 0;

        teleportSpellComboActiveLeft = new float[3];
        teleportSpellComboActiveRight = new float[3];
        for (int i = 0; i < teleportSpellComboActiveLeft.Length; i++)
        {
            teleportSpellComboActiveLeft[i] = 0;
            teleportSpellComboActiveRight[i] = 0;
        }
    }

    protected override void Update()
    {

        if (canPlay == false)
            return;
     

        if (myserverLogic.getIsFinish() == true)
        {
            if (isDie == true)
            {
                resetAnimation();//only the player that lose will have die animation
                return;
            }
        }

        setAnimation();
        showResult();

        base.Update();
        checkBlocking();


        if (shouldTurn(transform.position, enemy.transform.position) == true)//facing left
        {
            isBlockLeft = false;
            rb.rotation = Quaternion.Euler(0, 270, 0);
            if (isLocalPlayer == true)
            {
                spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.D, KeyCode.K);//down right attack
            }
        }
        else//facing right
        {
            isBlockLeft = true;
            rb.rotation = Quaternion.Euler(0, 90, 0);
            if (isLocalPlayer == true)
            {
                spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.A, KeyCode.K);//down left attack
            }

        }

        if (isLocalPlayer == true)
        {
            //teleportSpellCombo(teleportSpellComboActiveLeft, 0.5f, activeSpell, KeyCode.A, KeyCode.A, KeyCode.K);
            //teleportSpellCombo(teleportSpellComboActiveRight, 0.5f, activeSpell, KeyCode.D, KeyCode.D, KeyCode.K);

            //spellCombo(spellComboPassive, 0.2f, passiveSpell, KeyCode.K, KeyCode.L);
            spellCombo(spellComboPassive, 0.2f, passiveSpell, KeyCode.K, KeyCode.L);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ultimateSpell();
                if (chargingBar.fillAmount >= 1)
                {

                    ultimateSpell();
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {

                armorSpell();

            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                
                activeSpell(true);//teleport left

            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                activeSpell(false);//teleport right
            }
            if (Input.GetKeyDown(KeyCode.M))
            {

                passiveSpell();

            }


            if (isInUltimate == false)
                attack();
        }
        else
        {
            if (isPSArmor == true)
                healObj.SetActive(true);
            else
                healObj.SetActive(false);

            if (isPSActive == true)
            {
                myActivePS.Play();//teleport particle
                isPSActive = false;
            }

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

                CmdrangeAttack(offsetPos, direction, "lightray", myDamageMultipler, characterTag);
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

                    CmdrangeAttack(offsetPos, direction, "lightray", myDamageMultipler, characterTag);
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

               
                StartCoroutine(WaitForAnimation("cast ultimate", 0));

                //ultimateObj.SetActive(true);
                //ultimateObj.GetComponent<Transform>().position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 1, enemyTrans.position.z);
                //prepare for ultimate to see if ultimate move hit enemy
            }
        }
    }
    public override void ultimateMove()
    {
        //this mean that player successfully use ultimate on enemy

        
        isInUltimate = true;
        myserverLogic.setisInUltimateServer(true);
        addCurrentChargingBar(-1.0f);
        transmitUltimate(false, true);

        //myAnimator.SetTrigger("ultimate");
        //myUltimatePS.gameObject.transform.position = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
        //myUltimatePS.Play();
        myUltiCamera.setDetail(transform, enemyTrans, isBlockLeft, 2.0f);

       
       // enemy.GetComponent<CharacterBase>().setisInUltimate(true);
        StartCoroutine(spellDurationTimer(spellType.ultimate_spell, 8.0f));
    }

    void armorSpell()
    {
        if (canCastSpell[0] == true)//armor heal spell
        {
            canCastSpell[0] = false;
            healObj.SetActive(true);

            transmitParticleSystem(true, false, false, true, false);
            TakesDamage(-10.0f * spellPowerRate);//heal 10
            if (spellPowerRate > 1)
                spellPowerRate = 1;//reset

            armorUICD = true;
            transmitUI(true, false);
            //spellCastCoolDown[0] = spellCastCoolDown[0] * spellCoolDownRate;
            //UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);
            StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));

        }
    }
    void activeSpell(bool isLeft)
    {
        if (canCastSpell[1] == true)//speed spell
        {

            if (isLeft == true)//facing right
            {
                float temp = transform.position.x - 5;
                if (temp <= -23.0f)
                    temp = -23.0f;

                transform.position = new Vector3(temp, transform.position.y, transform.position.z);

            }
            else
            {
                float temp = transform.position.x + 5;
                if (temp >= 23.0f)
                    temp = 23.0f;
                transform.position = new Vector3(temp, transform.position.y, transform.position.z);

            }


            canCastSpell[1] = false;
            myActivePS.Play();
            transmitParticleSystem(false, true, false, false, true);

            activeUICD = true;
            transmitUI(false, true);
            //spellCastCoolDown[1] = spellCastCoolDown[1] * spellCoolDownRate;
            //UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);

        }
    }
    void passiveSpell()
    {
        if (canCastSpell[2] == true)//double spell power
        {
            canCastSpell[2] = false;
            myDamageMultipler = 2.0f;

            spellPowerRate = spellPowerRate * 2;
            myPassivePS.Play();
            transmitParticleSystem(false, false, true, true, false);
        }
    }
    void teleportSpellCombo(float[] cdrate, float timeNeeded, teleportSpellDelegate myspellDelegate, KeyCode kc1, KeyCode kc2, KeyCode kc3 = KeyCode.None)
    {


        if (kc3 != KeyCode.None)
        {
            if (cdrate[0] <= 0)
            {
                if (Input.GetKeyDown(kc1))
                {
                    cdrate[0] = timeNeeded;
                }
            }
            else
            {
                if (cdrate[1] <= 0)
                {
                    if (Input.GetKeyDown(kc2))
                    {
                        if (cdrate[0] > 0)
                        {
                            cdrate[1] = timeNeeded;
                        }

                    }
                }
                else
                {
                    if (Input.GetKeyDown(kc3))
                    {
                        if (cdrate[1] > 0)
                        {
                            //do stuff
                            if (kc1 == KeyCode.A)//teleport left
                                myspellDelegate(true);
                            else if (kc1 == KeyCode.D)//teleport right
                                myspellDelegate(false);
                            // Debug.Log("here spell");
                        }
                    }
                }
            }

        }

        if (cdrate[0] > 0)
            cdrate[0] -= Time.deltaTime;
        if (cdrate[1] > 0)
            cdrate[1] -= Time.deltaTime;

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
            healObj.SetActive(false);
            transmitParticleSystem(true, false, false, false, false);
        }
        else if (_spell == spellType.active_spell)
        {
          //teleport
        }
        else if (_spell == spellType.passive_spell)
        {

        }
        else if (_spell == spellType.ultimate_spell)
        {
          
           
            isInUltimate = false;
            myserverLogic.setisInUltimateServer(false);
            myUltiCamera.removeUltimate();

            //enemy.GetComponent<CharacterBase>().setisInUltimate(false);
            //myUltimatePS.Stop();

            transmitUltimate(false, true);
            enemy.GetComponent<CharacterBaseNetwork>().TakesDamage(ultimateDamage);


        }


    }


}


