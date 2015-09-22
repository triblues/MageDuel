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
    private Transform myTransform;
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


    }

    // Update is called once per frame
    void Update()
    {
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

            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;

        }

        if (Vector3.Distance(target.position, myTransform.position) < 2)
        {
            myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime;

        }

        if (Vector3.Distance(target.position, myTransform.position) < 5)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rigid.AddForce(transform.up * jumpPower);
            }
        }


        //if (target.rotation.y < 270.0f)
        //{
        //    transform.RotateAround(transform.position, transform.up, 180.0f);
        //}


        //move towards target
        // myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;

        // If the enemy and the player have health left...


    }




}
