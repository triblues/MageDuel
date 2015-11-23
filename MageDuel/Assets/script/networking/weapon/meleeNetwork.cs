using UnityEngine;
using System.Collections;

public class meleeNetwork : weaponBaseNetwork
{

    BoxCollider myBC;
    // Use this for initialization
    void Awake()
    {

        myBC = GetComponent<BoxCollider>();

    }
    protected override void Start()
    {
        damageMultipler = 1;//default
        comboCount = 1;//default
       
    }
    protected override void OnEnable()
    {

        totalTime = deSpawn_Time;
        myBC.enabled = true;


    }
    protected override void OnDisable()
    {
      //  base.OnDisable();
        myBC.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (deSpawn_Time == 0)//unlimited
            return;
        totalTime -= Time.deltaTime;

        if (totalTime <= 0)
        {

            gameObject.GetComponent<meleeNetwork>().enabled = false;
        }
    }

    override protected void OnTriggerEnter(Collider other)
    {



        if (other.GetComponent<CharacterBaseNetwork>() != null)//has a body
        {

            if (other.GetComponent<CharacterBaseNetwork>().getEnemy().GetComponent<CharacterBaseNetwork>().getCanCombo() == false)
            {

                other.GetComponent<CharacterBaseNetwork>().getEnemy().GetComponent<CharacterBaseNetwork>().setCanCombo(true);
              
            }


           
            if (other.GetComponent<CharacterBaseNetwork>().getIsBlocking() == true)
            {
                other.GetComponent<CharacterBaseNetwork>().setBlockAnimation();
            }

        }
        base.OnTriggerEnter(other);

        gameObject.GetComponent<meleeNetwork>().enabled = false;


    }
}




