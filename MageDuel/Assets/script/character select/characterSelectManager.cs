using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class characterSelectManager : MonoBehaviour {

    enum mage
    {
        Inferno,
        Pristine,
        Radiance,
        no_one
    };
    public static int selectedCharacter;
    public GameObject[] allCharacter;
    public RectTransform target;
    public GameObject charInfo;
    public Text detailText;
    mage mymage;
    bool isSelect;
    bool isMoving;
    Vector3[] startingPos;
    // Use this for initialization
    void Start () {

        mymage = mage.no_one;
        isSelect = false;
        isMoving = false;

        startingPos = new Vector3[3];
        for(int i=0;i<startingPos.Length;i++)
        {
            startingPos[i] = allCharacter[i].transform.position;
        }

    }
	
	// Update is called once per frame
	void Update () {

        //if (isMoving == true)
        //    changePosition(false);
        //else
        //    changePosition(true);
        
	}

    public void selectCharacter(string name)
    {
        if (isSelect == true)
            return;

        isSelect = true;
       // isMoving = true;
        if (name == "Inferno")
            mymage = mage.Inferno;
        else if (name == "Pristine")
            mymage = mage.Pristine;
        else if (name == "Radiance")
            mymage = mage.Radiance;
        else
            mymage = mage.no_one;

        characterInfo();
        runAnimation(false);
        StartCoroutine(changingPos(false));
    }

    void characterInfo()
    {
        if(mymage == mage.Inferno)
            detailText.text = "Inferno is a fire mage who has a very explosive and aggressive combat style.";
        else if (mymage == mage.Pristine)
            detailText.text = "Pristine is an ice mage who has a defensive combat style.";
        else if (mymage == mage.Radiance)
            detailText.text = "Radiance is a light mage who is pretty well - balanced.";




    }
    void runAnimation(bool isReverse)
    {
        for (int i = 0; i < allCharacter.Length; i++)
        {
            if (allCharacter[i].name != mymage.ToString())
            {
                if (isReverse == false)
                {
                    allCharacter[i].GetComponent<Animation>()["fade"].time = 0;
                    allCharacter[i].GetComponent<Animation>()["fade"].speed = 1;
                    allCharacter[i].GetComponent<Animation>().Play("fade");
                }
                else
                {
                    allCharacter[i].GetComponent<Animation>()["fade"].time = allCharacter[i].GetComponent<Animation>()["fade"].length;
                    allCharacter[i].GetComponent<Animation>()["fade"].speed = -1;
                    allCharacter[i].GetComponent<Animation>().Play("fade");
                }
            }
        }
    }

    void changePosition(bool isReverse)
    {
        if (isSelect == false)//haven select
            return;

        if (isReverse == false)
        {
            allCharacter[(int)mymage].transform.position =
            Vector3.MoveTowards(allCharacter[(int)mymage].transform.position, target.position, 3);

            if (Vector3.Distance(allCharacter[(int)mymage].transform.position, target.position) <= 1.0f)
            {
                allCharacter[(int)mymage].transform.position = target.position;
                isMoving = false;
                charInfo.SetActive(true);
            }
        }
        else
        {
            allCharacter[(int)mymage].transform.position =
            Vector3.MoveTowards(allCharacter[(int)mymage].transform.position, startingPos[(int)mymage], 3);

            if (Vector3.Distance(allCharacter[(int)mymage].transform.position, startingPos[(int)mymage]) <= 1.0f)
            {
                allCharacter[(int)mymage].transform.position = startingPos[(int)mymage];
               
               
                isSelect = false;
                mymage = mage.no_one;
            }
            
        }

        
            
    }
    IEnumerator changingPos(bool isReverse)
    {
        while(true)
        {
            if(isReverse == false)
            {
                allCharacter[(int)mymage].transform.position =
                 Vector3.MoveTowards(allCharacter[(int)mymage].transform.position, target.position, 3);

                if (Vector3.Distance(allCharacter[(int)mymage].transform.position, target.position) <= 1.0f)
                {
                    allCharacter[(int)mymage].transform.position = target.position;
                    //isMoving = false;
                    charInfo.SetActive(true);
                    break;
                }
            }
            else
            {
                allCharacter[(int)mymage].transform.position =
                Vector3.MoveTowards(allCharacter[(int)mymage].transform.position, startingPos[(int)mymage], 3);

                if (Vector3.Distance(allCharacter[(int)mymage].transform.position, startingPos[(int)mymage]) <= 1.0f)
                {
                    allCharacter[(int)mymage].transform.position = startingPos[(int)mymage];


                    isSelect = false;
                    mymage = mage.no_one;
                    break;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    public void backBtn()
    {
        //isSelect = false;
        //  isMoving = false;
        StartCoroutine(changingPos(true));
        runAnimation(true);
        charInfo.SetActive(false);
    }
    public void saveCharacter()
    {
        selectedCharacter = (int)mymage;
        //PlayerPrefs.SetString("character", mymage.ToString());
    }
}
