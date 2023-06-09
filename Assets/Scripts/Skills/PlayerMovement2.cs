﻿using System.Collections;
using UnityEngine;


public partial class PlayerMovement
{
    bool canDoubleJump = false;
    bool canWaterWalk = false;
    bool regen = false;
    bool canSlowDownTime = false;
    bool canParry = false;
    bool canBecomeInvis = false;
    bool canStun = false;
    Coroutine healthRegen;
    public FlashImage flashImage;

    public void ListenForSpecialSkills()
    {
        DoubleJumpLogic();
        JesusAntiGravityMode();
        SlowDownTimeLogic();
        ParryLogic();
        InvisibilityLogic();
        StunLogic();
    }

    public void InitializeSpecialSkills()
    { 
        canDoubleJump = SkillTree.Instance.skillObjects["doubleJump"].SkillLevel > 0;
        canSlowDownTime = SkillTree.Instance.skillObjects["slowDownTime"].SkillLevel > 0;
        canWaterWalk = SkillTree.Instance.skillObjects["waterWalking"].SkillLevel > 0;
        canParry = SkillTree.Instance.skillObjects["parry"].SkillLevel > 0;
        regen = SkillTree.Instance.skillObjects["regen"].SkillLevel > 0;
        if (regen)
            healthRegen = StartCoroutine(Regen());
        else if (healthRegen != null)
        {
            StopCoroutine(healthRegen);
            healthRegen = null;
        }
        if(canWaterWalk)
            Physics.IgnoreLayerCollision(gameObject.layer, waterLayer, false);
        else Physics.IgnoreLayerCollision(gameObject.layer, waterLayer, true);


        var stun = SkillTree.Instance.skillObjects["stun"];
        canStun = stun.SkillLevel > 0;
        if (canStun)
        {
            stunCooldown = baseStunCooldown - stun.SkillLevel - 1;
            stunDamageModifier = (float)stun.SkillLevel / stun.SkillCap + 1;

        }

        //initializing passive stats here
        TankinessLogic();
        BonusDamageLogic();
        BonusAttackSpeedLogic();
        BonusMovespeedLogic();
        InvisibilityLogicInit();
    }

    public float baseStunCooldown = 5;
    float stunCooldown;
    bool stunAvailable = true;
    void ResetStunCD() => stunAvailable = true;
    void StunLogic()
    {
        if (canStun && stunAvailable)
        {
            if (Input.GetKeyDown(KeyCode.F) && readyToAttack && CanHitEnemies)
            {
                stunAvailable = false;
                CanRun = false;
                attackSoundEffect.Play();
                animator.SetTrigger("Stun");
                readyToAttack = false;
                var delay = animator.GetCurrentAnimatorStateInfo(0).length;
                Invoke(nameof(AttackTwoHanded), delay*0.5f);
                Invoke(nameof(ResetRunLock), delay);
                Invoke(nameof(ResetStunCD), stunCooldown);
            }
        }
    }

    void InvisibilityLogicInit()
    {
        var invisibility = SkillTree.Instance.skillObjects["invisibility"];
        canBecomeInvis = invisibility.SkillLevel > 0;
    }

    void ToggleInvis(bool state)
    {
        Debug.Log($"player: {gameObject.layer}, enemy: {enemy}");
        if(Physics.GetIgnoreLayerCollision(8, gameObject.layer) != state)
            Physics.IgnoreLayerCollision(8, gameObject.layer, state);
    }
    void InvisibilityLogic()
    {
        if(canBecomeInvis)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ToggleInvis(true);
                flashImage.FlashIn(0.1f, 0.15f, Color.blue);
            }
            else if(Input.GetKeyUp(KeyCode.R))
            {
                ToggleInvis(false);
                flashImage.FlashOut(0.1f, 0.15f, Color.blue);
            }
        }
    }


    public bool CanTakeDamage = true;
    bool CanHitEnemies = true;
    bool CanRun = true;
    void ParryLogic()
    {
        if (canParry)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("Block", true);
                CanHitEnemies = false;
                CanTakeDamage = false;
                CanRun = false;
            }
            else if(Input.GetKeyUp(KeyCode.E))
            {
                animator.SetBool("Block", false);
                var delay = animator.GetCurrentAnimatorStateInfo(0).length;
                Invoke(nameof(ResetCanHitEnemiesAndRun), delay);
            }
        }
    }
    void ResetCanHitEnemiesAndRun() 
    { 
        CanHitEnemies = true;
        CanTakeDamage = true;
        CanRun = true;
    }

    void SlowDownTimeLogic()
    {
        if (canSlowDownTime && Time.timeScale != 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                flashImage.FlashIn(0.1f, 0.15f, Color.gray);
                Time.timeScale = 0.33f;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                flashImage.FlashOut(0.1f, 0.15f, Color.gray);
                Time.timeScale = 1f;
            }
        }
    }

    void BonusMovespeedLogic()
    {
        var movespeedBonus = SkillTree.Instance.skillObjects["moveSpeed"];
        totalMovespeed = moveSpeed * (1 + ((float)movespeedBonus.SkillLevel / movespeedBonus.SkillCap) / 1.6f);
    }
    private IEnumerator Regen()
    {
        var playerHealth = gameObject.GetComponent<PlayerHealth>();
        while (true)
        {
            if (playerHealth.currentHealth < playerHealth.maxHealth)
                playerHealth.healthBar.SetHealth(playerHealth.currentHealth++);
            yield return new WaitForSeconds(2f);
        }
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
            if (waterGrounded && !Input.GetKey(KeyCode.LeftControl))
                rb.useGravity = false;
            else rb.useGravity = true;
        }
    }
    float CritChanceMod(float damage)
    {
        var critPoints = SkillTree.Instance.skillObjects["critChance"].SkillLevel;
        var num = random.Next(1, 11);
        if (num <= critPoints)
            return damage * 2;
        else return damage;
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

