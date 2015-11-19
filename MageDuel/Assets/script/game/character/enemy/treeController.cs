using UnityEngine;
using System.Collections;

public class treeController : EnemyAI {

    protected override void Awake()
    {

        base.Awake();
    }
    // Use this for initialization
    protected override void Start()
    {

        base.Start();
        StartCoroutine(regenHealth(5.0f));//every 5 sec heal 1 health
       
    }

    // Update is called once per frame
    protected override void Update()
    {
        setAnimation();
        base.Update();
    }
    //protected override void checkTurn()
    //{
    //    if (shouldTurn(transform.position, enemy.transform.position) == true)
    //    {
    //        rb.rotation = Quaternion.Euler(0, 200, 0);

    //    }
    //    else
    //    {
    //        rb.rotation = Quaternion.Euler(0, 120, 0);

    //    }
    //}
    IEnumerator regenHealth(float _time)
    {
        while(true)
        {
            if(currentHealth < startingHealth)
            {
                TakesDamage(-1.0f);
               // currentHealth += 1.0f;

            }
            yield return new WaitForSeconds(_time);
        }
    }
}
