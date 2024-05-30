using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    /*
        General purpose mana.  
    */
    public int maxMana = 100; 
    public int currentMana; 
    public ManaBar manaBar; 

    void Start()
    {
        currentMana = maxMana; 
        manaBar.SetMana(currentMana, maxMana); 
    }

    public void DecreaseMana(int dec) {
        currentMana -= dec; 

        if (currentMana < 0) {
            currentMana = 0; 
        }

        // set health bar
        manaBar.SetMana(currentMana);
    }

    public void IncreaseMana(int inc) {
        currentMana += inc; 

        if (currentMana > 100) {
            currentMana = 100; 
        }
        // set health bar
        manaBar.SetMana(currentMana);
    }

    public int GetMana() {
        return currentMana; 
    }
}
