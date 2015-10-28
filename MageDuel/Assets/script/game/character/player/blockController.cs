using UnityEngine;
using System.Collections;

public class blockController : MonoBehaviour {

    SpriteRenderer mySR;
    Animator myanimator;
    float alpha;
    Coroutine co;
    // Use this for initialization
    void Start () {

        mySR = GetComponent<SpriteRenderer>();
        myanimator = GetComponent<Animator>();
       
    }

    public void animateBlock(int currentBlockCount,int maxCount)
    {
        if (co != null)
            StopCoroutine(co);

        if(currentBlockCount > maxCount / 2)//more then half
        {
            mySR.color = new Color(1,1,1,1);//white
        }
        else if (currentBlockCount == 1)
        {
            mySR.color = new Color(1, 0, 0, 1);//red
        }
        //else if (currentBlockCount == 0)//block count is 0
        //{
        //    mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 0);
        //}
        else
        {
            mySR.color = new Color(1, 1, 0, 1);//yellow
        }
        
        //mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 1);
        alpha = mySR.color.a;
        myanimator.enabled = true;
        co = StartCoroutine(fadeOut());
    }

    IEnumerator fadeOut()
    {
        while(alpha > 0)
        {
            alpha-=0.05f;
            mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, alpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        myanimator.enabled = false;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
