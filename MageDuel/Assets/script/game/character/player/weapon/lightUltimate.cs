using UnityEngine;
using System.Collections;

public class lightUltimate : weaponBase {

  
    SphereCollider mySC;
    // Use this for initialization
    void Awake()
    {

        mySC = GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

      
    }

    void OnEnable()
    {

        
        totalTime = deSpawn_Time;
        StartCoroutine(delay(2.0f));

    }
    void OnDisable()
    {
        mySC.enabled = false;
    }
 
    IEnumerator delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mySC.enabled = true;
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
