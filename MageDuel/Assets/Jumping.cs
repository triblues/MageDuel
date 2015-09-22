using UnityEngine;
using System.Collections;

public class Jumping : MonoBehaviour
{
    public bool grounded = true;
    public int jumpPower;
    private bool hasJumped = false;
    Rigidbody rigid;

    // Use this for initialization
    void Start()
    {

        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do something

        //if (!grounded && rigid.velocity.y == 0)
        //{
        //    grounded = true;
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump triggered");
            rigid.AddForce(transform.up * jumpPower);
            grounded = false;
            hasJumped = false;
        }
    }

    void FixedUpdate()
    {
        //if (hasJumped)
        //{
        //    rigid.AddForce(transform.up * jumpPower);
        //    grounded = false;
        //    hasJumped = false;
        //}
    }
}