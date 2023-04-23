using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScreenUI : MonoBehaviour
{
    public Slider sliderHP;
    public Slider sliderEXP;
    public GameObject expText;
    public GameObject goldText;

    public void SetMaxHealth(float health)
    {
        sliderHP.maxValue = health;
        sliderHP.value = health;
    }

    public void SetHealth(float health)
    {
        sliderHP.value = health;
    }

    public void SetExp(int exp, int maxExp, int level)
    {
        sliderEXP.maxValue = maxExp;
        sliderEXP.value = exp;
        expText.GetComponent<Text>().text = $"Level {level}: {exp}/{maxExp}";
    }
    public void SetGold(int gold)
    {
        goldText.GetComponent<Text>().text = $"Gold: {gold}";
    }
}
