using UnityEngine;
using System.Collections;

public class fireUltimate : weaponBase
{

    bool canIncrease;
    BoxCollider myBC;
    // Use this for initialization
    void Awake()
    {

        myBC = GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (canIncrease == true)
        {
            
            myBC.center = new Vector3(myBC.center.x, myBC.center.y + 0.5f*Time.deltaTime, myBC.center.z);
            myBC.size = new Vector3(myBC.size.x, myBC.size.y + 1.0f*Time.deltaTime, myBC.size.z);
        }
    }
   
    void OnEnable()
    {
        
        myBC.center = new Vector3(0, -0.2f, 0);
        myBC.size = new Vector3(1, 0, 1);
        canIncrease = false;
        totalTime = deSpawn_Time;
        StartCoroutine(delay(1.0f));

    }
    void OnDisable()
    {
      
    }

    IEnumerator delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canIncrease = true;
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterBase>() != null)//has this script
        {
            if (other.GetComponent<CharacterBase>().getCharacterTag() != numTag)//prevent hit ownself
            {
                if (other.GetComponent<CharacterBase>().getIsBlocking() == false)
                {
                    other.GetComponent<CharacterBase>().getEnemy().GetComponent<CharacterBase>().ultimateMove();
                    gameObject.SetActive(false);
                    Debug.Log("hit ulti");
                }

            }
        }
    }
}
