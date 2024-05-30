using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float health, float maxHealth = -1)
    {
        if (maxHealth != -1)
        {
            slider.maxValue = maxHealth;
        }
        slider.value = health;
    }

    void OnEnable()
    {
        Battle.onChangePlayerHealth += SetHealth;
    }

    void OnDisable()
    {
        Battle.onChangePlayerHealth -= SetHealth;
    }
}
