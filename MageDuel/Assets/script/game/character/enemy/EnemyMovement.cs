using UnityEngine;
using System.Collections;


public class EnemyMovement : MonoBehaviour
{

    public Transform target;
    public int moveSpeed;
    public int rotationSpeed;
    public int maxDistance;
    public int jumpSpeed = 100;
    //public EnemyHealth eh;
    public Transform myTransform;
    public int dodgeRate;
    public bool dodge = false;
    //PlayerShooting player;
    public int jumpPower;
    Rigidbody rigid;
    Vector3 movement;
   // public PlayerShooting player;
    public float smooth = 2.0F;
    public float tiltAngle = 30.0F;
    private Quaternion targetRotation;
    private Vector3 targetAngles;
  
    public bool faceLeft = true;
    public bool faceRight = false;
    public EnemyAttack enemy;
    public int rnd;
    public float coolDown = 3.0f;
    public float attackTime;
    public int maxDistance2;



    Random r = new Random();

    void Awake()
    {
        myTransform = transform;
       
    }

    // Use this for initialization
    void Start()
    {


        GameObject go = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        target = go.transform;
        attackTime = coolDown;

    }

    // Update is called once per frame
    void Update()
    {

        //rnd = UnityEngine.Random.Range(1, 4);
        if (attackTime > 0)
            attackTime -= Time.deltaTime;

        if (attackTime < 0)
        {
            attackTime = 0;
            rnd = UnityEngine.Random.Range(1, 4);

            attackTime = coolDown;

        }
        float h = Input.GetAxisRaw("Horizontal");
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, 0f);
        Debug.DrawLine(target.transform.position, myTransform.position, Color.yellow);
        myTransform.LookAt(target);
        //look at target
        // myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);


        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
        {
          
                if (rnd == 4 || rnd == 3)
                {
                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime; //attack
                }
                else 
                {
                  myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime; //retreat and attack
                if (Vector3.Distance(target.position, myTransform.position) > maxDistance2)
                {
                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime; //attack

                }


               }


            
        }

        if (Vector3.Distance(target.position, myTransform.position) < 2)
        {
            myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime;

        }







        if (Vector3.Distance(target.position, myTransform.position) < 10)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rigid.velocity = new Vector3(0f, 7.5f, 0f);
            }
        }

        //transform.localRotation = Quaternion.Euler(0,90,0);
        //if (target.rotation.y <= 270.0f)
        //{
        //    Debug.Log("Enemy face right");
        //    transform.RotateAround(transform.position, transform.up, 180.0f);
        //    faceRight = true;
        //    faceLeft = false;

        //}


        //if (target.rotation.y >= 90.0f)
        //{
        //    Debug.Log("Enemy face left");
        //    transform.RotateAround(transform.position, transform.up, 180.0f);
        //    faceRight = false;
        //    faceLeft = true;
        //}





        //move towards target
        // myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;

        // If the enemy and the player have health left...


    }




}
