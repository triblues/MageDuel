using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTime : MonoBehaviour {

    public static bool isTimeOut;
	public int totalTime = 100;
	Text mytext;
    Coroutine co;
	// Use this for initialization
	void Start () {

        isTimeOut = false;
        mytext = GetComponent<Text> ();
		
		mytext.text = "Time: " + totalTime.ToString ();
	}
    public void startTimer()
    {
        if (launchScene.isPractice == false)
        {
            if(co == null)
                co = StartCoroutine(countDownTimer(1.0f));
            Debug.Log("here timer");
        }
    }
	IEnumerator countDownTimer(float interval)
	{
		while(totalTime > 0 && gameController.isFinish == false)
		{
			totalTime --;
            mytext.text = "Time: " + totalTime.ToString();
            yield return new WaitForSeconds(interval);
		}
        gameController.isFinish = true;
        isTimeOut = true;
        // Application.LoadLevel("timeOut");
        yield return null;
	}
	// Update is called once per frame
	void Update () {
	
		
	}
}
