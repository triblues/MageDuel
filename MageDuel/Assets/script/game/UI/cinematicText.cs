using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cinematicText : MonoBehaviour {

    public string[] dialog;
    Text mytext;
    Coroutine co;
	// Use this for initialization
	void Start () {

        mytext = GetComponent<Text>();
        mytext.fontSize = Screen.width / 25;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void resetText()
    {
        mytext.text = "";
    }
    public void startText(int index)
    {
        if (co != null)
            StopCoroutine(co);
       

        co = StartCoroutine(showText(index));
    }
    IEnumerator showText(int index)
    {
        for(int i =0;i<=dialog[index].Length;i++)
        {
            mytext.text = dialog[index].Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
