using Assets.Scripts.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public partial class PlayerMovement : MonoBehaviour, IDataPersistence
{
    bool canDoubleJump = false;
    int tankiness = 0;
    int bonusAD = 0;
    int critChance = 0;
    bool slowDownTime = false;
    bool parry = false;
    int invisibility = 0;
    bool waterWalking = false;
    int movespeedBonus = 0;
    int stun = 0;
    bool regen = false;
    int bonusAS = 0;

    public void InitializeSpecialSkills()
    {
        //passive
        tankiness = SkillTree.Instance.skillObjects["hp"].SkillLevel;
        bonusAD = SkillTree.Instance.skillObjects["attackDamage"].SkillLevel;
        critChance = SkillTree.Instance.skillObjects["critChance"].SkillLevel;
        waterWalking = SkillTree.Instance.skillObjects["waterWalking"].SkillLevel > 0;
        movespeedBonus = SkillTree.Instance.skillObjects["moveSpeed"].SkillLevel;
        regen = SkillTree.Instance.skillObjects["regen"].SkillLevel > 0;
        bonusAS = SkillTree.Instance.skillObjects["attackSpeed"].SkillLevel;

        //active
        canDoubleJump = SkillTree.Instance.skillObjects["doubleJump"].SkillLevel > 0;       
        slowDownTime = SkillTree.Instance.skillObjects["slowDownTime"].SkillLevel > 0;
        parry = SkillTree.Instance.skillObjects["parry"].SkillLevel > 0;  
        invisibility = SkillTree.Instance.skillObjects["invisibility"].SkillLevel;   
        stun = SkillTree.Instance.skillObjects["stun"].SkillLevel;  
    }


    bool doubleJumpUsed = false;
    public void ListenForSpecialSkills()
    {
        DoubleJumpLogic();

    }


    void DoubleJumpLogic()
    {
        if (Input.GetButtonDown("Jump") && !grounded && canDoubleJump && !doubleJumpUsed)
        {
            doubleJumpUsed = true;
            animator.SetTrigger("Jump");
            Jump();
        }
        if (doubleJumpUsed && grounded)
            doubleJumpUsed = false;
    }
}

