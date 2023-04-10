using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsAndAttributes : MonoBehaviour
{
    public float playerHealth;
    public float playerMaxHealth = 100f;
    public float playerArmor;
    public float playerMaxArmor = 100f;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = playerMaxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        //healthRegen(0.2f, 0f);
    }

    public void takeDamage(float damageAmmount)
    {
        if(playerArmor <= 0f)
        {
            playerHealth -= damageAmmount;
        }
        else{
            playerArmor -= damageAmmount;
        }
    }

    public void healthRegen(float regenAmmount, float additionalRegenAmmount)
    {
        
        if(playerHealth < playerMaxHealth)
        {
            playerHealth += regenAmmount + additionalRegenAmmount;
        }
    }
}
