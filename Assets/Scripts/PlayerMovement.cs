using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float airMultiplier = 0.4f;
    public float jumpCooldown = 0.25f;

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
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(Input.GetButton("Fire1") && readyToAttack)
        {
            readyToAttack = false;
            Attack();
            Invoke(nameof(ResetAttackCd), attackCooldown);
        }

    }
    private void Attack()
    {
        //play animation here

        var enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemy);
        foreach(var item in enemiesHit)
        {
            item.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }


    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(playerCapsule.transform.position, Vector3.down, playerCapsule.height * 0.5f + 0.2f, ground);
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
