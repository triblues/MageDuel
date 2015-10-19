using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAI : CharacterBase
{


    public bool testMode = false;

    int meleeComboCount;
    int randMin = 1;
    int randMax = 100;
    int randomNum;

    //AI state stuff
    [SerializeField]
    public float idleTime = 1.0f;//the value mean how long the character will be in this state
    [SerializeField]
    public float attackTime = 2.0f;
    [SerializeField]
    public float blockTime = 1.0f;
    [SerializeField]
    public float randAttributeTime = 1.5f;
    float idleTimer;
    float attackTimer;
    float blockTimer;
    float randomAttTimer;

    bool changeState;
    bool isReverseDirection;
    bool inMeleeCombo;//this check to prevent repeating of doing melee combo
                      //fuzzy logic stuff
    [Tooltip("the lower aggreesive level the lower chance to attack")]
    public int aggressiveLevel = 1;
    public int cruelLevel = 1;
    public float rangeDistance = 1.0f;
    public float meleeDistance = 0.3f;


    public AudioClip jumped;
    public AudioClip impact;
    public AudioClip walking;
    public AudioClip[] sounds;
    public AudioSource audio;

    public AIState myAIState;
    public AIAttack myAIStateAttack;
    public enum AIState
    {
        idle,
        attack

    };
    public enum AIAttack
    {
        melee,
        rangeSingle,
        rangeMultiple
    };
    protected override void Awake()
    {
        healthText = GameObject.Find("Canvas").transform.Find("enemy/health").GetComponent<Text>();
        manaText = GameObject.Find("Canvas").transform.Find("enemy/mana").GetComponent<Text>();

        combo = GameObject.Find("Canvas").transform.Find("enemy/combo text").gameObject;
        chargingBar = GameObject.Find("Canvas").transform.Find("enemy/charging bar outer/charging bar inner").
            GetComponent<Image>();


        base.Awake();
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        audio = (AudioSource)GetComponent<AudioSource>();
        meleeComboCount = 0;
        inMeleeCombo = false;
        changeState = false;
        //myAIState = AIState.attack;
        //myAIStateAttack = AIAttack.rangeSingle;

        isReverseDirection = false;

        aggressiveLevel = Mathf.Clamp(aggressiveLevel, 1, 6);

        GameObject[] temp;
        temp = GameObject.FindGameObjectsWithTag("Main Player");

        foreach (GameObject a in temp)
        {
            if (a.name.Contains("Clone") == true)
                GameObject.Destroy(a);
            else
                enemy = a;
        }


    }

    // Update is called once per frame
    protected override void Update()
    {

        if (gameController.isFinish == true)
            return;

        if (shouldTurn(transform.position, enemy.transform.position) == true)
        {
            rb.rotation = Quaternion.Euler(0, 270, 0);

        }
        else
        {
            rb.rotation = Quaternion.Euler(0, 90, 0);

        }


        base.Update();//move and jump

        if (testMode)
        {
            horizontal = 0;
            jumping = 0;
        }
        else
        {
            action();

        }
        AI_Agent();

    }

    void AI_Agent()
    {
        switch (myAIState)
        {
            case AIState.idle:
                idleState();
                break;
            case AIState.attack:
                {

                    attackState();

                    switch (myAIStateAttack)
                    {
                        case AIAttack.melee:
                            meleeCombat();
                            break;
                        case AIAttack.rangeSingle:
                            rangeCombat();
                            break;
                        case AIAttack.rangeMultiple:
                            rangeCombat();
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }

        if (isBlocking == true)
        {
            blockState();
        }
    }
    void action()
    {
        if (changeState == true)
        {
            randomNum = getRandomNum(randMin, randMax);
            if (randomNum >= randMax - (aggressiveLevel * 15))//the lower aggreesive level the lower chance to attack
            {
                int offset;
                myAIState = AIState.attack;
                randomNum = getRandomNum(randMin, randMax);

                if (currentMana <= startingMana / 5)//left 20%
                    offset = 30;//will want to go melee combat as much as possible
                else
                    offset = -30;

                if (randomNum >= randMax / 2 + offset)
                {
                    randomNum = getRandomNum(randMin, randMax);
                    if (randomNum >= randMax / 2)
                        myAIStateAttack = AIAttack.rangeSingle;
                    else
                        myAIStateAttack = AIAttack.rangeMultiple;
                }
                else
                    myAIStateAttack = AIAttack.melee;
                //   myAIStateAttack = AIAttack.melee;


            }
            else
            {
                myAIState = AIState.idle;
            }


            changeState = false;
        }

        randomAttribute();//random move and jump




    }
    void idleState()
    {
        idleTimer += Time.deltaTime;
        horizontal = 0;
        jumping = 0;
        isReverseDirection = false;
        if (idleTimer >= idleTime)
        {
            changeState = true;
            idleTimer = 0;//reset
        }
    }
    void attackState()
    {

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackTime)
        {
            changeState = true;
            attackTimer = 0;//reset
        }
    }
    void blockState()
    {
        blockTimer += Time.deltaTime;

        if (blockTimer >= blockTime)
        {
            changeState = true;
            blockTimer = 0;
            isBlocking = false;
        }
    }
    void rangeCombat()
    {
        if (currentMana <= 0)
            return;
        if (canRangeAttack == false)
            return;
        Vector3 direction;

        if (myAIStateAttack == AIAttack.rangeSingle)
        {
            Vector3 offsetPos = enemy.transform.position;
            offsetPos.y = offsetPos.y + 1;
            direction = offsetPos - transform.position;
            audio.PlayOneShot(impact);
            rangeAttack(transform.position, direction, gameController.projectileType.fireball);
        }
        else //multiple attack
        {
            for (int i = 0; i < 3; i++)//3
            {
                Vector3 newPos = new Vector3(enemy.transform.position.x,
                                             enemy.transform.position.y, enemy.transform.position.z);
                newPos.y = newPos.y + i * 1.5f;
                direction = newPos - transform.position;
                audio.PlayOneShot(impact);
                rangeAttack(transform.position, direction, gameController.projectileType.fireball);

            }
        }

        if (Vector3.Distance(transform.position, enemy.transform.position) <= rangeDistance)
        {
            if (isReverseDirection == false)
            {
                horizontal = horizontal * -1;//maintain distance from player
                isReverseDirection = true;
            }
        }
        else
            isReverseDirection = false;
    }
    void meleeCombat()
    {

        if (Vector3.Distance(transform.position, enemy.transform.position) <= meleeDistance)
        {
            horizontal = 0;
            jumping = 0;
            if (inMeleeCombo == false)
            {
                inMeleeCombo = true;
                StartCoroutine(meleeComboSequence(0.2f));
            }
            //meleeAttack();
        }
        else//AI is far away from player 
        {
            if (isJumping == false)//when on ground
            {
                if (transform.position.x > enemy.transform.position.x)//at right side
                {
                    horizontal = -1;
                }
                else
                {
                    horizontal = 1;
                }
            }


        }
    }


    void randomAttribute()//random move and jump
    {
        randomAttTimer += Time.deltaTime;
        if (randomAttTimer >= randAttributeTime)//random movement every few second
        {
            if (myAIStateAttack != AIAttack.melee)//enemy will go directly to the player
            {
                randomNum = getRandomNum(randMin, randMax);
                if (randomNum >= randMax / 2)
                    horizontal = -1;
                else
                    horizontal = 1;
            }


            randomNum = getRandomNum(randMin, randMax);
            if (randomNum >= randMax / 2)
                jumping = 1;
            else
                jumping = 0;


            if (isJumping == true)
                jumping = 0;//prevent keep jumping

            randomNum = getRandomNum(randMin, randMax);
            if (currentHealth > startingHealth / 2)//more then half health
            {
                if (randomNum >= randMax / 2)
                {
                    isBlocking = true;
                }
                else
                    isBlocking = false;
            }
            else//lower then half health
            {
                if (randomNum >= randMax / 2 - 30)//higher chance to block
                {
                    isBlocking = true;
                }
                else
                    isBlocking = false;
            }


            randomAttTimer = 0;
        }


    }

    IEnumerator meleeComboSequence(float wait)
    {
        while (true)
        {

            meleeAttack();
            meleeComboCount++;
            if (meleeComboCount >= 3)//reach max combo
            {
                if (enemy.GetComponent<CharacterBase>().getIsBlocking() == false)
                    enemy.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeComboCount = 0;
                changeState = true;
                attackTimer = 0;//reset
                inMeleeCombo = false;
                break;
            }


            yield return new WaitForSeconds(wait);
        }
    }

}












