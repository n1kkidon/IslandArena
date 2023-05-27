using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float airMultiplier = 0.4f;
    public float jumpCooldown = 0.7f;
    public float sneakMultiplier = 0.4f;
    public float sprintMultiplier = 3f;

    Animator animator;

    public float groundDrag;

    [Header("Ground Check")]
    public CapsuleCollider playerCapsule;
    public LayerMask ground;
    bool grounded;
    bool readyToJump = true;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Combat")]
    public float attackCooldown = 0.8f;
    public float attackDamage = 25;
    public float attackRange = 3;
    public Transform attackPoint;
    public LayerMask enemy;

    bool readyToAttack;
    void ResetAttackCd() => readyToAttack = true;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToAttack = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        if (Input.GetButton("Jump") && readyToJump && grounded)
        {
            Debug.Log("jumping");
            animator.SetTrigger("Jump");
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(Input.GetButton("Fire1") && readyToAttack)
        {
            animator.SetTrigger("AttackEnemy");
            readyToAttack = false;
            var delay = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke(nameof(Attack), delay * 0.3f);
            
        }

    }
    private void Attack()
    {
        Invoke(nameof(ResetAttackCd), attackCooldown);

        var enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemy);
        foreach (var item in enemiesHit)
        {
            if (item.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage, out var loot))
            {
                gameObject.GetComponent<PlayerInventory>().GetMobDrop(loot);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void MovePlayer()
    {
        
        animator.SetBool("Run", true);

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(moveDirection == Vector3.zero || !grounded)
        {
            animator.SetBool("Run", false);
        }

        var force = 10f * moveSpeed * moveDirection.normalized;
        if (!grounded)
            force *= airMultiplier;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool("Sneak", true);
            force *= sneakMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Sprint", true);
            force *= sprintMultiplier;
        }
        else
        {
            animator.SetBool("Sneak", false);
            animator.SetBool("Sprint", false);
        }
            
        
        rb.AddForce(force, ForceMode.Force);
    }


    // Update is called once per frame
    void Update()
    {
        var funnyPos = playerCapsule.transform.position;
        funnyPos.y += 0.1f;
        grounded = Physics.Raycast(funnyPos, Vector3.down, 0.2f, ground);
        MyInput();
        SpeedControl();
        if (grounded)
            rb.drag = groundDrag;
        else rb.drag = 0;
    }
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up *jumpForce, ForceMode.Impulse);
    }
    private void ResetJump() => readyToJump = true;

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition.ToVector3();
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position.ToFloatArray();
    }
}
