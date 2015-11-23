using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cinematicText : MonoBehaviour {

    string[] dialog;
    Text mytext;
    Coroutine co;
	// Use this for initialization
	void Start () {

        dialog = new string[2];
        mytext = GetComponent<Text>();
        mytext.fontSize = Screen.width / 25;

        if(characterSelectManager.selectedCharacter == 0)//fire
        {
            dialog[0] = "I am gonna burn you to ashes!!!";
        }
        else if (characterSelectManager.selectedCharacter == 1)//ice
        {
            dialog[0] = "Get ready, the winter is coming early this year!";
        }
        else if (characterSelectManager.selectedCharacter == 2)//light
        {
            dialog[0] = "Let my light glow within the darkest part of your soul!";
        }

        if(levelSelectController.selectedLevel == 1)
        {
            dialog[1] = "Once you are inside, there is no way out";
        }
        else if (levelSelectController.selectedLevel == 2)
        {
            dialog[1] = "i am tree";
        }
        else if (levelSelectController.selectedLevel == 3)
        {
            dialog[1] = "I am gonna crush you!";
        }
        else if (levelSelectController.selectedLevel == 4)
        {
            dialog[1] = "i am turtle";
        }
        else if (levelSelectController.selectedLevel == 5)
        {
            dialog[1] = "Welcome to the darkness, child";
        }

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
