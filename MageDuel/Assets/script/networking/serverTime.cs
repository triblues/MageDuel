using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class serverTime : MonoBehaviour {

    serverLogic myserverLogic;
    Text mytext;
	// Use this for initialization
	void Start () {

        mytext = GetComponent<Text>();
        mytext.fontSize = Screen.width / 30;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(myserverLogic != null)
        {
            mytext.text = "Time: " + myserverLogic.getGameTime().ToString();
        }
	}
    public void startCountDown()
    {
        myserverLogic = GameObject.Find("server logic(Clone)").GetComponent<serverLogic>();
        //StartCoroutine(countDown());
    }
    //IEnumerator countDown()
    //{
    //    while(true)
    //    {
    //        mytext.text = "Time: " + myserverLogic.getGameTime().ToString();
    //        yield return new WaitForSeconds(1.0f);
    //    }
    //}
}
