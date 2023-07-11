using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int _health, int _maxHealth)
    {
        slider.maxValue = _maxHealth;
        slider.value = _health;
    }
}
