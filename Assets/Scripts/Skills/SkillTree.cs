using Assets.Scripts.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using TMPro;
using Assets.Scripts.Saving;

public class SkillTree : MonoBehaviour, IDataPersistence
{

    public Dictionary<string, SkillObject> skillObjects;
    public Dictionary<string, Skill> SkillList;
    public PlayerInventory playerInventory;
    public static SkillTree Instance;
    public TMP_Text pointsCounter;
    public PlayerMovement playerMovement;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("more than 1 instance of SkillTree!");
        }
        Instance = this;
        var json = File.ReadAllText("islandArena_skills.json");
        skillObjects = JsonConvert.DeserializeObject<Dictionary<string, SkillObject>>(json);
        SkillList = GetComponentsInChildren<Skill>().ToDictionary(x => x.dictionaryKey);
    }

    private void Start()
    {
        UpdateAllSkillUI();
        UpdatePointsCounter();
    }
    public void UpdatePointsCounter()
    {
        pointsCounter.text = $"Available skill points: {playerInventory.levelPointsAvailable}";
    }
    public void UpdateAllSkillUI()
    {
        foreach (var skill in SkillList.Values)
        {
            skill.Initialize();
            
        }
    }

    public void LoadData(GameData data)
    {
        skillObjects = JsonConvert.DeserializeObject<Dictionary<string, SkillObject>>(data.SkillTreeDictionaryJson);
        SkillList = GetComponentsInChildren<Skill>().ToDictionary(x => x.dictionaryKey);
        UpdateAllSkillUI();
        UpdatePointsCounter();
    }

    public void SaveData(ref GameData data)
    {
        data.SkillTreeDictionaryJson = JsonConvert.SerializeObject(skillObjects);
    }
}
