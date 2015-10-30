using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICoolDown : MonoBehaviour {

    Image myimage;
    Coroutine co;
	// Use this for initialization
	void Start () {

        myimage = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void startCoolDown(float cdTime, bool[] cd, int num)
    {
        if (co != null)
            StopCoroutine(co);
        myimage.fillAmount = 0;
        co = StartCoroutine(coolDown(cdTime,cd,num));
    }
    IEnumerator coolDown(float cdTime, bool[] cd, int num)
    {
        while(myimage.fillAmount < 1)
        {
            myimage.fillAmount += 0.1f / cdTime;
            if (myimage.fillAmount >= 1)
                break;
            yield return new WaitForSeconds(0.1f);
        }
        cd[num] = true;
        Debug.Log("in end");
       // myimage.fillAmount = 1;
    }
}
