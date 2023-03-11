using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody body;
    [SerializeField] float Movement_speed = 3;
    [SerializeField] float Jump_power = 3;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
            body.AddForce(new Vector3(0, Jump_power, 0), ForceMode.VelocityChange);

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        body.velocity = new Vector3(horizontal * Movement_speed, body.velocity.y, vertical * Movement_speed);

    }
}
