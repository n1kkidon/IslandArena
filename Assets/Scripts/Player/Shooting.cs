using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public void Shoot()
    {
        PlayerMovement.instance.Shoot();
    }
}
