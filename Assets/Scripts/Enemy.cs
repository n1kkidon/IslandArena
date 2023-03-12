using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameManager manager;
    NavMeshAgent agent;
    Transform player;
    public LayerMask groundLayer, playerLayer;
    public Slider healthBar;

    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointRange;
    [Header("Stats")]
    public float AttackCooldown = 0.7f;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;
    public float attackDamage = 20;
    public float tetherRange = 0.5f;

    PlayerHealth playerHealth;
    public float maxHealth = 250f;
    float currentHealth;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        var playertmp = manager.player;
        player = playertmp.transform;
        playerHealth = playertmp.GetComponent<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = CalculateHealth();
    }

    private float CalculateHealth() => currentHealth / maxHealth;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        else
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void ChasePlayer() => agent.SetDestination(player.position);

    private void AttackPlayer()
    {
        if((transform.position - player.position).magnitude < tetherRange) 
            agent.SetDestination(transform.position);
        else agent.SetDestination(player.position);
        transform.LookAt(player);
        if(!alreadyAttacked)
        {
            //Need to put attacking logic here 
            playerHealth.TakeDamage(attackDamage);
            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), AttackCooldown);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = CalculateHealth();
        if (currentHealth <= 0)
            Invoke(nameof(DestroyEnemy), 0.15f);

    }
    private void DestroyEnemy() => Destroy(gameObject);

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            walkPointSet = true;
    }
    private void ResetAttack() => alreadyAttacked = false;

}
