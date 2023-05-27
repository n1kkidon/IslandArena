using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    public PlayerScreenUI ScreenUI;
    public int gold = 500;
    public int level = 1;
    public int experience = 0;
    private int baseXp = 100;
    private int xpScalingFactor = 5;
    private float xpExponentialFactor = 2f;
    public int expToNextLevel = 100;

    private int levelPointsUsed; //you get 1 point per level (for now)
    public int levelPointsAvailable;
    public void LoadData(GameData data)
    {
        gold = data.gold;
        level = data.level;
        experience = data.experience;
        expToNextLevel = data.expToNextLevel;
        levelPointsUsed = data.levelPointsUsed;
        levelPointsAvailable = data.levelPointsAvailable;
        ScreenUI.SetGold(gold);
        ScreenUI.SetExp(experience, expToNextLevel, level);
    }

    public void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.level = level;
        data.experience = experience;
        data.expToNextLevel = expToNextLevel;
        data.levelPointsUsed = levelPointsUsed;
        data.levelPointsAvailable = levelPointsAvailable;
    }

    public void GetMobDrop(MobDrop loot)
    {
        if (loot == null) 
            return;
        AddGold(loot.Gold);
        AddExp(loot.Experience);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        ScreenUI.SetGold(gold);
    }
    public bool RemoveGoldIfAvailable(int amount)
    {
        if(gold > amount)
        {
            gold -= amount;
            ScreenUI.SetGold(gold);
            return true;
        }
        return false;
    }
    public void AddExp(int amount)
    {
        var total = experience + amount;
        while (total >= expToNextLevel)
        {
            total -= expToNextLevel;
            LevelUp();           
        }
        experience = total;
        ScreenUI.SetExp(experience, expToNextLevel, level);
    }
    public void LevelUp()
    {
        level++;
        expToNextLevel = Mathf.RoundToInt(100 + baseXp * Mathf.Pow((float)level / xpScalingFactor, xpExponentialFactor));
        levelPointsAvailable++;
        SkillTree.Instance.UpdatePointsCounter();
    }
}
