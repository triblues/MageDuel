using UnityEngine;
using System.Collections;

public class baneBossController : EnemyAI
{

    protected override void Awake()
    {

        base.Awake();
    }
    // Use this for initialization
    protected override void Start()
    {

        base.Start();
        StartCoroutine(regenHealth(1.0f));

    }

    // Update is called once per frame
    protected override void Update()
    {
        setAnimation();
        base.Update();
    }
  
    IEnumerator regenHealth(float _time)
    {
        while (true)
        {
            if (currentHealth < startingHealth)
            {
                TakesDamage(-1.0f);
            

            }
            yield return new WaitForSeconds(_time);
        }
    }
}
