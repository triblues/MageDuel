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
    [SerializeField] public int ultimateDamage = 30;

    [Header("armor,active,passive")]
    [SerializeField]
    int[] spellCastCoolDown;
    [SerializeField]
    int[] spellDuration;
   

    ParticleSystem myArmorPS;
    ParticleSystem mySpeedPS;
    ParticleSystem myPassivePS;
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
        mySpeedPS = transform.Find("Fire Trail").GetComponent<ParticleSystem>();
        myPassivePS = transform.Find("OTUSpell").GetComponent<ParticleSystem>();

      
        
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

        //myUltiCamera.setCharacterDetail(transform, 
        //    new Vector3(transform.position.x - transform.forward.x * 2, transform.position.y + 3,transform.position.z), isBlockLeft);
        //myUltiCamera.enabled = true;
        //isCastModeAnimation = true;

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
        base.Update();

        if (shouldTurn(transform.position, enemy.transform.position) == true)//facing left
        {
            isBlockLeft = false;
            rb.rotation = Quaternion.Euler(0, 270, 0);
        }
        else//facing right
        {
            isBlockLeft = true;
            rb.rotation = Quaternion.Euler(0, 90, 0);
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
    //protected IEnumerator main_WaitForAnimation(string name, int count)
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    while (myAnimator.GetCurrentAnimatorStateInfo(count).IsName(name) == true)//the animation is still running
    //    {
            
    //        yield return new WaitForSeconds(0.05f);

    //    }
       
    //    if (name == "melee 3")
    //    {

    //        myAnimator.SetBool("meleeAttack3", false);
    //        Debug.Log("in here?");
    //        canCombo = false;
    //        isAttack = false;
    //    }
    //}

    
    public override void ShapeDraw(drawShape.shape myshape)
    {
        if (isCastModeAnimation == true)
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
                           
                isKnockBack = false;
                canCastSpell[0] = false;
                myArmorPS.Play();
                StartCoroutine(spellDurationTimer(spellType.armor_spell, spellDuration[0]));
                StartCoroutine(spellCoolDown(spellCastCoolDown[0], canCastSpell, 0));

                lastDrawShape = drawShape.shape.no_shape;
            }
            else if(myshape == drawShape.shape.diamond)
            {
                myUltiCamera.setCharacterDetail(transform, transform.forward, isBlockLeft);
                myUltiCamera.enabled = true;
                isCastModeAnimation = true;
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

                canCastSpell[0] = true;
                canCastSpell[1] = true;
                canCastSpell[2] = false;
                myPassivePS.Play();

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
                if (canCastSpell[1] == false)//speed spell
                    return;//in cooldown

                canCastSpell[1] = false;
                mySpeedPS.enableEmission = true;
                normalSpeed = normalSpeed * 2;

                lastDrawShape = drawShape.shape.no_shape;
                StartCoroutine(spellDurationTimer(spellType.active_spell, spellDuration[1]));
                StartCoroutine(spellCoolDown(spellCastCoolDown[1], canCastSpell, 1));
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
            mySpeedPS.enableEmission = false;
        }
        else if (_spell == spellType.passive_spell)
        {
           
        }
        
       
    }
    //[Client]
    //protected void setNetworkIdentify()
    //{
    //    mynetworkID = GetComponent<NetworkIdentity>().netId;
    //}

}

