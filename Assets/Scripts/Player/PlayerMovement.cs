using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public partial class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    float totalMovespeed;
    public float jumpForce;
    public float airMultiplier = 0.4f;
    public float jumpCooldown = 0.7f;
    public float sneakMultiplier = 0.4f;
    public float sprintMultiplier = 3f;
    

    public Animator animator;

    public float groundDrag;

    [Header("Ground Check")]
    public CapsuleCollider playerCapsule;
    public LayerMask ground;
    bool grounded;
    bool readyToJump = true;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    private System.Random random;

    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Combat")]
    public float baseAttackCooldown = 0.8f;
    float modifiedAttackCooldown;
    public float baseAttackDamage = 25;
    float totalAttackDamage;
    public float attackRange = 3;
    public float stunDuration = 1.5f;
    float stunDamageModifier = 1;
    public Transform attackPoint;
    public LayerMask enemy;
    public GameObject weaponHolder;
    public GameObject bowHolder;
    public GameObject equipedWeapon;
    public Weapon currentWeapon;
    private bool usingBow=false;
    public GameObject arrowObject;
    public Transform arrowPoint;

    bool readyToAttack;
    void ResetAttackCd() => readyToAttack = true;
    public static PlayerMovement instance;

    [SerializeField] private AudioSource attackSoundEffect;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        instance= this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToAttack = true;
        totalAttackDamage = baseAttackDamage;
        modifiedAttackCooldown = baseAttackCooldown;
        funnyPos = playerCapsule.transform.position;
        random = new System.Random();
        totalMovespeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if(CanRun)
            MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        if (Input.GetButton("Jump") && readyToJump && (grounded || waterGrounded))
        {
            animator.SetTrigger("Jump");
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(Input.GetButton("Fire1") && readyToAttack && CanHitEnemies)
        {
            CanRun = false;
            attackSoundEffect.Play();
            if (usingBow)
            {
                animator.SetTrigger("AttackEnemyBow");
            }
            else animator.SetTrigger("AttackEnemy");
            readyToAttack = false;          
            var delay = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke(nameof(Attack), delay * 0.3f);       
            Invoke(nameof(ResetRunLock), delay * 0.7f);       
        }
        ListenForSpecialSkills();

    }
    void ResetRunLock() => CanRun = true;
    private void Attack()
    { 
        Invoke(nameof(ResetAttackCd), modifiedAttackCooldown);

        var enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemy);
        var nextAttack = CritChanceMod(totalAttackDamage);
        foreach (var item in enemiesHit)
        {
            if (item.gameObject.GetComponent<Enemy>().TakeDamage(nextAttack, out var loot))
            {
                gameObject.GetComponent<PlayerInventory>().GetMobDrop(loot);
            }
        }
    }
    public void Shoot()
    {
        GameObject arrow= Instantiate(arrowObject, arrowPoint.position, arrowPoint.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(arrowPoint.up*25f, ForceMode.Impulse);
    }
    public float ArrowDamage()
    {
        return CritChanceMod(totalAttackDamage);
    }

    private void AttackTwoHanded()
    {
        Invoke(nameof(ResetAttackCd), modifiedAttackCooldown);

        var enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemy);
        var nextAttack = CritChanceMod(totalAttackDamage) * stunDamageModifier;
        foreach (var item in enemiesHit)
        {
            if (item.gameObject.GetComponent<Enemy>().TakeDamage(nextAttack, out var loot, stunDuration))
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

        if(moveDirection == Vector3.zero || (!grounded && !waterGrounded))
        {
            animator.SetBool("Run", false);
        }

        var force = 10f * totalMovespeed * moveDirection.normalized;
        if (!grounded && !waterGrounded)
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
    Vector3 funnyPos;
    void Update()
    {
        funnyPos = playerCapsule.transform.position;
        funnyPos.y += 0.1f;
        grounded = Physics.Raycast(funnyPos, Vector3.down, 0.2f, ground);     
        MyInput();
        SpeedControl();
        if (grounded || waterGrounded)
            rb.drag = groundDrag;
        else rb.drag = 0;
    }
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVelocity.magnitude > totalMovespeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * totalMovespeed;
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
    public void EquipWeapon(Weapon weapon)
    {
        if (weapon.isBow)
        {
            equipedWeapon = Instantiate(weapon.weaponObject, bowHolder.transform);
            usingBow = true;
        }
        else equipedWeapon = Instantiate(weapon.weaponObject, weaponHolder.transform);
        currentWeapon = weapon;
        modifiedAttackCooldown *= weapon.speedMultiplier;
        totalAttackDamage += weapon.damage;
        attackRange += weapon.attackRange;
        Debug.Log($"Weapon damage: {weapon.damage}");
    }
    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Debug.Log("Cringe");
            modifiedAttackCooldown /= currentWeapon.speedMultiplier;
            totalAttackDamage -= currentWeapon.damage;
            attackRange -= currentWeapon.attackRange;
            currentWeapon.type = ItemType.OwnedWeapon;
            if (currentWeapon.isBow)
            {
                usingBow = false;
            }
        }
        Destroy(equipedWeapon);
    }
}
