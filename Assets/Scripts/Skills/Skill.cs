using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string dictionaryKey;

    public TMP_Text text;
    public Image bgImage;

    public void UpdateUI()
    {
        var skillObject = SkillTree.Instance.skillObjects[dictionaryKey];
        text.text = $"{skillObject.SkillLevel}/{skillObject.SkillCap}\n{skillObject.SkillName}";
        
        if (skillObject.SkillLevel >= skillObject.SkillCap) 
        {
            bgImage.color = Color.blue;
            UnlockChildSkills();
        }
    }
    public void UnlockChildSkills()
    {
        var skillObject = SkillTree.Instance.skillObjects[dictionaryKey];
        foreach (var childKey in skillObject.ChildSkillObjects)
        {
            var child = SkillTree.Instance.SkillList[childKey];
            if (child.bgImage.color == Color.gray)
                child.bgImage.color = Color.green;
            child.UpdateUI();
        }
    }

    public void Buy()
    {
        if(SkillTree.Instance.playerInventory.levelPointsAvailable >= 1 && !IsGrey() && !IsBlue())
        {
            SkillTree.Instance.playerInventory.levelPointsAvailable -= 1;
            SkillTree.Instance.skillObjects[dictionaryKey].SkillLevel++;
            SkillTree.Instance.UpdatePointsCounter();
        }
        UpdateUI();
    }

    public bool IsGrey() => bgImage.color == Color.grey;
    public void Initialize()
    {
        var skillObject = SkillTree.Instance.skillObjects[dictionaryKey];
        text.text = $"{skillObject.SkillLevel}/{skillObject.SkillCap}\n{skillObject.SkillName}";
        bgImage.color = SkillTree.Instance.skillObjects[dictionaryKey].Root ? Color.green : Color.gray;
    }
    public bool IsBlue() => bgImage.color == Color.blue;
}
