using UnityEngine;
using System.Collections;

public class itemShopController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void buyItem(string name,int amount)
    {
        if(PlayerPrefs.GetInt("coin") >= amount)
        {
            PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") - amount);
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
        }
    }
}
