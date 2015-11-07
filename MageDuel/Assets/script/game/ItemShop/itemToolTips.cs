using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;
    public int cost;
    Text mytext;
    Text mytextCost;
    Button mybtn;
   
    // Use this for initialization
    void Start () {

        mytext = GameObject.Find("body description").GetComponent<Text>();
        mytextCost = GameObject.Find("cost").GetComponent<Text>();
        mybtn = gameObject.GetComponent<Button>();
     
        mybtn.onClick.AddListener(() => buyitem());
    }
	void buyitem()
    {
        if (PlayerPrefs.GetInt("coin") >= cost)
        {
            PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") - cost);
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
        }
    }
    // Update is called once per frame
    void Update () {
	
       
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        mytext.text = description;
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
