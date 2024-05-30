using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMana(float mana, float maxMana = -1) {
        if (maxMana != -1)
        {
            slider.maxValue = maxMana;
        }
        slider.value = mana; 
    }

    void OnEnable()
    {
        Battle.onChangePlayerMana += SetMana;
    }

    void OnDisable()
    {
        Battle.onChangePlayerMana -= SetMana;
    }
}
