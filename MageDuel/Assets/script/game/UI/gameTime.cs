using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTime : MonoBehaviour {

	public int totalTime = 100;
	Text mytext;
	// Use this for initialization
	void Start () {
	
		mytext = GetComponent<Text> ();
		StartCoroutine (countDownTimer (1.0f));
		mytext.text = "Time: " + totalTime.ToString ();
	}

	IEnumerator countDownTimer(float interval)
	{
		while(totalTime > 0)
		{
			totalTime --;
			yield return new WaitForSeconds(interval);
		}
		yield return null;
	}
	// Update is called once per frame
	void Update () {
	
		mytext.text = "Time: " + totalTime.ToString ();
	}
}
