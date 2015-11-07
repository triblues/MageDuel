using UnityEngine;
using System.Collections;

public class melee : weaponBase {
	
	BoxCollider myBC;
	// Use this for initialization
	void Awake () {
	
		myBC = GetComponent<BoxCollider> ();

	}

	void OnEnable()
	{
     
        totalTime = deSpawn_Time;
		myBC.enabled = true;

		
	}
	void OnDisable()
	{
		myBC.enabled = false;
	}

	// Update is called once per frame
	protected override void Update () {
	
		if (deSpawn_Time == 0)//unlimited
			return;
        totalTime -= Time.deltaTime;

        if (totalTime <= 0)
        {
			
			gameObject.GetComponent<melee>().enabled = false;
		}
	}

	override protected void OnTriggerEnter(Collider other)
	{

       
       
        if (other.GetComponent<CharacterBase>() != null)//has a body
        {

            if (other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().getCanCombo() == false)
            {

                other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().setCanCombo(true);
                //Debug.Log("touch");
               // other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().setFirstCoolDownMeleeTimer();

            }
           

            if (other.GetComponent<CharacterBase>().getIsBlocking() == false)
            {
                //other.GetComponent<CharacterBase>().setStunRate(5);
            }
            else
            {
                other.GetComponent<CharacterBase>().setBlockAnimation();
            }
            
        }
        base.OnTriggerEnter(other);

        gameObject.GetComponent<melee>().enabled = false;
      
        
    }
}




