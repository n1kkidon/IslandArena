using Assets.Scripts.Saving;
using Assets.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public partial class PlayerMovement : MonoBehaviour, IDataPersistence
{
    bool canDoubleJump = false;
    bool canWaterWalk = false;

    public void ListenForSpecialSkills()
    {
        DoubleJumpLogic();
        JesusAntiGravityMode();
    }

    public void InitializeSpecialSkills()
    {
        //passive      
        //critChance = SkillTree.Instance.skillObjects["critChance"];
        canWaterWalk = SkillTree.Instance.skillObjects["waterWalking"].SkillLevel > 0;
        //movespeedBonus = SkillTree.Instance.skillObjects["moveSpeed"];
        //regen = SkillTree.Instance.skillObjects["regen"].SkillLevel > 0;
        if(canWaterWalk)
            Physics.IgnoreLayerCollision(gameObject.layer, waterLayer, false);
        else Physics.IgnoreLayerCollision(gameObject.layer, waterLayer, true);

        ////active
        canDoubleJump = SkillTree.Instance.skillObjects["doubleJump"].SkillLevel > 0;       
        //slowDownTime = SkillTree.Instance.skillObjects["slowDownTime"].SkillLevel > 0;
        //parry = SkillTree.Instance.skillObjects["parry"].SkillLevel > 0;  
        //invisibility = SkillTree.Instance.skillObjects["invisibility"];   
        //stun = SkillTree.Instance.skillObjects["stun"];


        //initializing passive stats here
        TankinessLogic();
        BonusDamageLogic();
        BonusAttackSpeedLogic();
    }

    public LayerMask waterLayer;
    bool waterGrounded = false;
    void JesusAntiGravityMode()
    {
        if (canWaterWalk)
        {
            var modFunnyPos = funnyPos;
            modFunnyPos.y -= 0.5f;  //these are like this for doubleJump not to sink you down
            waterGrounded = Physics.Raycast(funnyPos, Vector3.down, 0.7f, waterLayer);
            if (waterGrounded)
                rb.useGravity = false;
            else rb.useGravity = true;
        }
    }

    void BonusAttackSpeedLogic()
    {
        var bonusAS = SkillTree.Instance.skillObjects["attackSpeed"];
        modifiedAttackCooldown = baseAttackCooldown / (1 + ((float)bonusAS.SkillLevel / bonusAS.SkillCap / 1.6f));
    }

    void BonusDamageLogic()
    {
        var bonusDmg = SkillTree.Instance.skillObjects["attackDamage"];
        totalAttackDamage = baseAttackDamage * (1 +((float)bonusDmg.SkillLevel / bonusDmg.SkillCap / 1.6f));
    }

    public float takeDamageRatio = 1;
    void TankinessLogic()
    {
        var tankiness = SkillTree.Instance.skillObjects["hp"];
        takeDamageRatio = 1 - ((float)tankiness.SkillLevel / tankiness.SkillCap / 1.6f);      
    }

    bool doubleJumpUsed = false;
    void DoubleJumpLogic()
    {
        if (Input.GetButtonDown("Jump") && (!grounded && !waterGrounded) && canDoubleJump && !doubleJumpUsed)
        {
            doubleJumpUsed = true;
            animator.SetTrigger("Jump");
            Jump();
        }
        if (doubleJumpUsed && (grounded || waterGrounded))
            doubleJumpUsed = false;
    }
}

