using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    /*
        General Health Script
    */

    public int maxHealth = 100; 
    public int currentHealth; 
    public HealthBar healthBar; 

    void Start()
    {
        currentHealth = maxHealth; 
        healthBar.SetMaxHealth(maxHealth); 
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage; 

        if (currentHealth < 0) {
            currentHealth = 0; 
        }

        // set health bar
        healthBar.SetHealth(currentHealth);
    }

    public void Heal(int heal) {
        currentHealth += heal; 

        if (currentHealth > 100) {
            currentHealth = 100; 
        }
        // set health bar
        healthBar.SetHealth(currentHealth);
    }

    // check if Rosencrantz and Guildenstern are Dead
    public bool IsDead() {
        return (currentHealth > 0) ? false : true; 
    }

    public float GetHealth() {
        return currentHealth; 
    }
}
