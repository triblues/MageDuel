using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public GameObject target;
    public float attackTime = 0;
    public float attackTime2 = 0 ;
    public float attackTime3 = 0 ;
    public float coolDown = 2.0f;
    public float coolDown2 = 5.0f;
    public float coolDown3 = 10.0f;
    public float Distance;
    private Transform myTransform;
    public float ShootRange = 25.0f;
    
    public float moveSpeed = 5.0f;
    public float Damping = 6.0f;
    //public float projectile : Rigidbody;
    public float fireRate = 1;
    public float FireRange = 0 - 100;
    public float nextFire = 0.0f;
    //public PlayerAttack player;
    //public EnemyAi enemy;
    public int dodgeRate;
    public float chaseRange = 20.0f;
    public float attackRange = 6.5f;
    public float attackRange2 = 8.5f;
    public float attackRange3 = 10.5f;
    public PlayerController pController;

    Rigidbody rigid;


    void Awake()
    {
        myTransform = transform;

    }


    void Start()
    {
        //attackTime = 0;
        //coolDown = 5.0f;

        //attackTime2 = 0;
        //coolDown2 = 5.0f;

        //attackTime3 = 0;
        //coolDown3 = 10.0f;


        GameObject go = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        



    }


    void Update()
    {
        if (attackTime > 0)
            attackTime -= Time.deltaTime;

        if (attackTime < 0)
            attackTime = 0;


        if (attackTime == 0)
        {
            Attack();
            attackTime = coolDown;
        }

        if (attackTime2 > 0)
            attackTime2 -= Time.deltaTime;

        if (attackTime2 < 0)
            attackTime2 = 0;


        if (attackTime2 == 0)
        {
            Attack2();
            attackTime2 = coolDown;
        }


        if (attackTime3 > 0)
            attackTime3 -= Time.deltaTime;

        if (attackTime3 < 0)
            attackTime3 = 0;


        if (attackTime3 == 0)
        {
            Attack3();
            attackTime3 = coolDown;
        }


    }

    private void Attack()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        Debug.Log("Enemy Attack!!");

        Vector3 dir = (target.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{

        //first attack
        if (distance <= chaseRange)
        {
            if (distance <= attackRange)
            {

                if (direction > 0)
                {
                    //PlayerHealth eh = (PlayerHealth)target.GetComponent("PlayerHealth");
                    //eh.AddjustCurrentHealth(-20);


                }

            }

            else
            {
                //transform.LookAt(player.transform.position);

            }
        }







    }



    private void Attack2()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        Debug.Log("Enemy Attack2!!");

        Vector3 dir = (target.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{

        //first attack
        if (distance <= chaseRange)
        {
            if (distance <= attackRange)
            {

                if (direction > 0)
                {
                    //PlayerHealth eh = (PlayerHealth)target.GetComponent("PlayerHealth");
                    //eh.AddjustCurrentHealth(-20);


                }

            }

            else
            {
                //transform.LookAt(player.transform.position);

            }
        }







    }

    private void Attack3()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        Debug.Log("Enemy Attack3!!");

        Vector3 dir = (target.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{

        //first attack
        if (distance <= chaseRange)
        {
            if (distance <= attackRange)
            {

                if (direction > 0)
                {
                    //PlayerHealth eh = (PlayerHealth)target.GetComponent("PlayerHealth");
                    //eh.AddjustCurrentHealth(-20);


                }

            }

            else
            {
                //transform.LookAt(player.transform.position);

            }
        }







    }


}