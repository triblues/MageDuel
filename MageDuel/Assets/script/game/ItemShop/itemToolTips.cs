using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int maxCount = 0;
    public string description;
    public int cost;
  
    Text mytext;
    Text mytextCost;
    Button mybtn;
    itemShopController myISC;
   
    // Use this for initialization
    void Start () {

        mytext = GameObject.Find("body description").GetComponent<Text>();
        mytextCost = GameObject.Find("cost").GetComponent<Text>();
        mybtn = gameObject.GetComponent<Button>();
        myISC = GameObject.Find("itemShopController").GetComponent<itemShopController>();

        mybtn.onClick.AddListener(() => buyitem());
    }
	void buyitem()
    {
        myISC.clickSound();
        if (maxCount != 0)
        {
            if (PlayerPrefs.GetInt(name) >= maxCount)
            {
                mytext.text = "Upgrade Maxed!!!";
                return;
            }
        }
        if (name.Contains("btn") == false)
        {
            if (PlayerPrefs.GetInt("coin") >= cost)
            {
                PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") - cost);
                PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
            }
            else
            {
                mytext.text = "Not enough coins!!!";
            }
        }
      
    }
    // Update is called once per frame
    void Update () {
	
       
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        mytext.text = description;
        if(cost > 0)
            mytextCost.text = "Cost: " + cost.ToString();

        Debug.Log("The cursor entered the selectable UI element.");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mytext.text = "";
        mytextCost.text = "";
      
       
        Debug.Log("exit");
    }
}
