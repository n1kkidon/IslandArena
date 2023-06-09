using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("references")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rb;

    public float rotationSpeed;
    public static ThirdPersonCam instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.shopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            var viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDirection.normalized;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 inputDirection = orientation.forward * vertical + orientation.right * horizontal;

            if (inputDirection != Vector3.zero)
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
        
    }
    public Vector3 ViewDirection()
    {
        return player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
    }
}
