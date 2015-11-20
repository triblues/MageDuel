using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class radiancePlayerController : CharacterBase
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


    [Header("armor,active,passive")]
    [SerializeField]
    int[] spellCastCoolDown;
    [SerializeField]
    int[] spellDuration;
    int spellPowerRate;
    

    protected float[] teleportSpellComboActiveLeft;
    protected float[] teleportSpellComboActiveRight;

    public delegate void spellDelegate();
    public delegate void teleportSpellDelegate(bool direction);

    //public static bool canUlti;


    //protected drawShape myDrawShape;
    drawShape.shape lastDrawShape;




    protected override void Awake()
    {
        healthBar = GameObject.Find("Canvas").transform.Find("player/health/outer/inner").GetComponent<Image>();
        manaBar = GameObject.Find("Canvas").transform.Find("player/mana/outer/inner").GetComponent<Image>();

        combo = GameObject.Find("Canvas").transform.Find("player/combo text").gameObject;
        chargingBar = GameObject.Find("Canvas").transform.Find("player/charging bar outer/charging bar inner").
            GetComponent<Image>();

        UIarmorCD = GameObject.Find("Canvas").transform.Find("player/armor image outer/armor image inner").GetComponent<UICoolDown>();
        UIActiveCD = GameObject.Find("Canvas").transform.Find("player/active image outer/active image inner").GetComponent<UICoolDown>();

        healObj = transform.Find("heal").gameObject;//armor
        myActivePS = transform.Find("teleport").GetComponent<ParticleSystem>();//teleport
        myPassivePS = transform.Find("OTUSpell").GetComponent<ParticleSystem>();//double spell power

        myUltimatePS = transform.parent.Find("light ultimate").GetComponent<ParticleSystem>();
        ultimateObj = transform.parent.Find("ultimate start").gameObject;
        ultimateObj.GetComponent<weaponBase>().setTag(characterTag);

        spellPowerRate = 1;

        teleportSpellComboActiveLeft = new float[3];
        teleportSpellComboActiveRight = new float[3];
        for (int i =0;i< teleportSpellComboActiveLeft.Length;i++)
        {
            teleportSpellComboActiveLeft[i] = 0;
            teleportSpellComboActiveRight[i] = 0;
        }

        isInResult = false;
        base.Awake();
        //currentHealth = 50;
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
        checkBlocking();
        

        if (shouldTurn(transform.position, enemy.transform.position) == true)//facing left
        {
            isBlockLeft = false;
            rb.rotation = Quaternion.Euler(0, 270, 0);
            spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.D, KeyCode.K);//down right attack
           
        }
        else//facing right
        {
            isBlockLeft = true;
            rb.rotation = Quaternion.Euler(0, 90, 0);
            spellCombo(spellComboArmor, 0.5f, armorSpell, KeyCode.S, KeyCode.A, KeyCode.K);//down left attack
         

        }
        teleportSpellCombo(teleportSpellComboActiveLeft, 0.5f, activeSpell, KeyCode.A, KeyCode.A, KeyCode.K);
        teleportSpellCombo(teleportSpellComboActiveRight, 0.5f, activeSpell, KeyCode.D, KeyCode.D, KeyCode.K);

        spellCombo(spellComboPassive, 0.2f, passiveSpell, KeyCode.K, KeyCode.L);

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (chargingBar.fillAmount >= 1)
            {
             
                ultimateSpell();
            }
        }


        if (isInUltimate == false)
            attack();
    }
   
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
                {
                    if (myDamageMultipler != 1.0f)
                        myDamageMultipler = 1.0f;
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

                    rangeAttack(offsetPos, direction,0, myDamageMultipler);
                    if (isNotEnoughMana == true)
                    {
                        break;
                    }

                }
                if (isNotEnoughMana == false)
                {
                    if (myDamageMultipler != 1.0f)
                        myDamageMultipler = 1.0f;
                    rangeAttackAnimation();
                }
                
            }
        }




        comboAttack();

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
                if (isUnlimitedSpell == false)
                    canCastUltimate = false;
                canMove = false;
                myAnimator.SetTrigger("castUltimate");
                StartCoroutine(WaitForAnimation("cast ultimate", 0));
                ultimateObj.SetActive(true);
                ultimateObj.GetComponent<Transform>().position = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 1.0f, enemyTrans.position.z);
                //prepare for ultimate to see if ultimate move hit enemy
            }
        }
    }
    public override void ultimateMove()
    {
        //this mean that player successfully use ultimate on enemy
        Debug.Log("light att");
        chargingBar.fillAmount = 0;
        myAnimator.SetTrigger("ultimate");
        myUltimatePS.gameObject.transform.position = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
        myUltimatePS.Play();
        myUltiCamera.setDetail(transform, enemy.transform, isBlockLeft, 2.0f);

        isInUltimate = true;
        enemy.GetComponent<CharacterBase>().setisInUltimate(true);
        StartCoroutine(spellDurationTimer(spellType.ultimate_spell, 8.0f));
    }

    void armorSpell()
    {
        if (isInUltimate == true)
            return;
        if (canCastSpell[0] == true)//heal spell
        {
            if (isUnlimitedSpell == false)
                canCastSpell[0] = false;
            healObj.SetActive(true);

            Debug.Log("in heal");
            TakesDamage(-10.0f * spellPowerRate);
            if (spellPowerRate > 1)
                spellPowerRate = 1;//reset

            spellCastCoolDown[0] = spellCastCoolDown[0] * spellCoolDownRate;
            UIarmorCD.startCoolDown(spellCastCoolDown[0], canCastSpell, 0);
            StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));

        }
    }
    void activeSpell(bool isLeft)
    {
        if (isInUltimate == true)
            return;
        if (canCastSpell[1] == true)//teleport spell
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

            if (isUnlimitedSpell == false)
                canCastSpell[1] = false;
            myActivePS.Play();
         

            spellCastCoolDown[1] = spellCastCoolDown[1] * spellCoolDownRate;
            UIActiveCD.startCoolDown(spellCastCoolDown[1], canCastSpell, 1);
          //  StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));

        }
    }
    void passiveSpell()
    {
        if (isInUltimate == true)
            return;
        if (canCastSpell[2] == true)//increase spell power
        {
           
            canCastSpell[2] = false;
            myDamageMultipler = 2.0f;

            spellPowerRate = spellPowerRate * 2;
            myPassivePS.Play();
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
                            if (kc1 == KeyCode.A)
                                myspellDelegate(true);
                            else if (kc1 == KeyCode.D)
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
    IEnumerator spellDurationTimer(spellType _spell, float timeTaken)
    {
        yield return new WaitForSeconds(timeTaken);


        if (_spell == spellType.armor_spell)
        {
           
            healObj.SetActive(false);
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
            Debug.Log("end");
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


