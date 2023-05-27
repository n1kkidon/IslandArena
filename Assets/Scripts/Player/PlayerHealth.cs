using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public float maxHealth = 100;
    public float currentHealth;

    public PlayerScreenUI healthBar;
    public GameManager manager;

    Animator animator;
    PlayerMovement playerMovement;
    bool isDead = false;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(20);
        //}
    }

    public void TakeDamage(float damage)
    {
        
        animator.SetTrigger("GetHit");
        currentHealth -= damage * playerMovement.takeDamageRatio;

        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0 && !isDead)
        {
            isDead = true;
            animator.SetBool("Died", true);
            manager.GameOver();
        }
    }

    public void LoadData(GameData data)
    {
        currentHealth = data.currentHealth;
        maxHealth = data.maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    public void SaveData(ref GameData data)
    {
        data.currentHealth = currentHealth;
        data.maxHealth = maxHealth;
    }
}
