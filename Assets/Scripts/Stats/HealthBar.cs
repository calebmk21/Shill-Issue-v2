using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public bool isEnemy;

    public void SetHealth(float health, float maxHealth = -1)
    {
        if (maxHealth != -1)
        {
            slider.maxValue = maxHealth;
        }
        slider.value = health;
    }

    void Awake()
    {
        if (isEnemy)
        {
            Battle.onChangeEnemyHealth += SetHealth;
        }
        else
        {
            Battle.onChangePlayerHealth += SetHealth;
        }
    }

    void OnDestroy()
    {
        if (isEnemy)
        {
            Battle.onChangeEnemyHealth -= SetHealth;
        }
        else
        {
            Battle.onChangePlayerHealth -= SetHealth;
        }
    }
}
