/*This is the base class of all players classes
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerBase : MonoBehaviour {

    protected float startingHealth = 100.0f;
    protected float currentHealth;
    protected float startingMana = 100.0f;
    protected float currentMana;

    protected float speed;
    protected float offFactor;
    protected float defFactor;

    public float healthBarLength;

    // Use this for initialization
    void Awake () {
        // Setup player attributes
        currentHealth = startingHealth;
        currentMana = startingMana;
       
        SetupPlayer();
    }

    // Function to setup player's special attributes (speed etc)
    protected virtual void SetupPlayer()
    {
        // Properties by default
        speed = 5.0f;
        offFactor = 1.0f;
        defFactor = 1.0f;
    }
    
    public void TakesDamage(float damage)
    {
        // Damage will be adjusted
        // Normally, damage will have value of 10. If the player has a defensive factor value of 0.75:
        // damage = 10 * (1 + (1 - 0.75))
        //        = 10 * (1.25) 
        //        = 12.5
        // The player will take more damage (12.5) because of its low defensive factor
        damage = damage * (1 + (1 - defFactor));
        // Reduce health
        currentHealth -= damage;
    }

    /*****************GETTER**************************************/
    // This function returns speed
    public float GetSpeed()
    {
        return speed;
    }
    // This function returns off factor
    public float GetOffFactor()
    {
        return offFactor;
    }
    // This function returns def factor
    public float GetDefFactor()
    {
        return defFactor;
    }




    /**************************************************************/
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, healthBarLength, 20), currentHealth + "/" + startingHealth);
    }

    /*****************SCALE***********************/
   /* Offensive Factor (OffFactor): 0.75 - 1.25 (Weak - Strong)
      Defensive Factor (DefFactor): 0.75 - 1.25 (Weak - Strong)
   */

}
