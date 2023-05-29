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
    public GameObject waveText;

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
    public void SetWave(int wave, int enemies, float time)
    {
        if(time == -1f)
        {
            waveText.GetComponent<Text>().text = $"Waves completed\nCongratulations!";
            return;
        }
        if (enemies <= 0)
        {
            waveText.GetComponent<Text>().text = $"Current wave: {wave}\nTime until next wave: {time}";
        }
        else
        {
            waveText.GetComponent<Text>().text = $"Current wave: {wave}\nEnemies remaining: {enemies}";
        }
    }
}
