using UnityEngine;
using System.Collections;

public class iceUltimate : weaponBase {

    CapsuleCollider myCapCollider;
	// Use this for initialization
	void Start () {

        myCapCollider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        totalTime = deSpawn_Time;
    }
    // Update is called once per frame
    protected override void Update () {

        base.Update();
        transform.Translate(Vector3.down * speed * Time.deltaTime);

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
                    
                    Debug.Log("hit ice ulti");
                }

            }
        }
        gameObject.SetActive(false);
    }
}
