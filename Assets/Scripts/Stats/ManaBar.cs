using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public bool isEnemy;

    public void SetMana(float mana, float maxMana = -1) {
        //print("ah" +  mana + " " +  maxMana);
        if (maxMana != -1)
        {
            slider.maxValue = maxMana;
        }
        slider.value = mana; 
    }

    void Awake()
    {
        if (isEnemy)
        {
            Battle.onChangeEnemyMana += SetMana;
        }
        else
        {
            Battle.onChangePlayerMana += SetMana;
        }
    }

    void OnDestroy()
    {
        if (isEnemy)
        {
            Battle.onChangeEnemyMana -= SetMana;
        }
        else
        {
            Battle.onChangePlayerMana -= SetMana;
        }
    }
}
